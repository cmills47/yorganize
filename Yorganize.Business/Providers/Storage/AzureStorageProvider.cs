using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Yorganize.Business.Exceptions;


namespace Yorganize.Business.Providers.Storage
{
    public class AzureStorageProvider : StorageProviderBase
    {
        private string StorageConnectionString { get; set; }
        private CloudStorageAccount StorageAccount { get; set; }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);

            if (config["storageConnectionString"] == null)
                throw new System.Configuration.Provider.ProviderException(
                    "AzureStorageLoggingProvider: No storage connection string specified.");

            StorageConnectionString = config["storageConnectionString"];

            StorageAccount = CloudStorageAccount.Parse(StorageConnectionString);
        }

        #region Queue

        private CloudQueue GetQueue(string name)
        {
            CloudQueueClient queueStorage = StorageAccount.CreateCloudQueueClient();
            var queue = queueStorage.GetQueueReference(name);
            queue.CreateIfNotExists();

            return queue;
        }

        public override void AddEventToQueue(string queue, IQueueMessage message)
        {
            CloudQueue cloudQueue = GetQueue(queue);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            CloudQueueMessage cloudMessage = new CloudQueueMessage(serializer.Serialize(message));

            cloudQueue.AddMessage(cloudMessage);
        }

        public override T GetEventFromQueue<T>(string queue)
        {
            CloudQueue cloudQueue = GetQueue(queue);
            CloudQueueMessage cloudMessage = cloudQueue.GetMessage();

            if (cloudMessage != null)
            {
                T message;
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                try
                {
                    message = serializer.Deserialize<T>(cloudMessage.AsString);
                }
                catch (Exception ex)
                {
                    throw new BusinessException("Could not add deserialize event message.", ex);
                }

                message.MessageId = cloudMessage.Id;
                message.PopReceipt = cloudMessage.PopReceipt;
                message.DequeueCount = cloudMessage.DequeueCount;

                return message;
            }

            return default(T);
        }

        public override void RemoveEventFromQueue(string queue, IQueueMessage message)
        {
            CloudQueue cloudQueue = GetQueue(queue);
            cloudQueue.DeleteMessage(message.MessageId, message.PopReceipt);
        }

        #endregion

        #region Files

        public override void UploadFile(Stream source, string path, string contentType = null)
        {
            string containerName, blobName;
            GetStorageContainerAndBlobNames(path, out containerName, out blobName);

            CloudBlobClient client = StorageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = client.GetContainerReference(containerName);
            container.CreateIfNotExists();
            var blob = container.GetBlockBlobReference(blobName);
            
            if(!string.IsNullOrEmpty(contentType))
                blob.Properties.ContentType = contentType;

            source.Seek(0, SeekOrigin.Begin);
            blob.UploadFromStream(source);
          
        }

        public override Stream DownloadFile(string path)
        {
            string containerName, blobName;
            GetStorageContainerAndBlobNames(path, out containerName, out blobName);

            CloudBlobClient client = StorageAccount.CreateCloudBlobClient();

            var container = client.GetContainerReference(containerName);
            var blob = container.GetBlockBlobReference(blobName);

            Stream stream = new MemoryStream();
            blob.DownloadToStream(stream);

            return stream;
        }

        public override bool DeleteFile(Uri uri)
        {
            CloudBlobClient client = StorageAccount.CreateCloudBlobClient();
            var blob = client.GetBlobReferenceFromServer(uri);

            return blob.DeleteIfExists();
        }

        public override uint DeleteFiles(IEnumerable<Uri> uris)
        {
            uint deleted = 0;

            foreach (var uri in uris)
                if (DeleteFile(uri))
                    deleted++;

            return deleted;
        }

        public override Uri GetFileUri(string path)
        {
            string containerName, blobName;
            GetStorageContainerAndBlobNames(path, out containerName, out blobName);

            CloudBlobClient client = StorageAccount.CreateCloudBlobClient();

            var container = client.GetContainerReference(containerName);
            var blob = container.GetBlockBlobReference(blobName);

            return blob.Uri;
        }

        public override Uri GetFileAccessUri(string path, AccessPolicy policy, TimeSpan valid)
        {
            string containerName, blobName;
            GetStorageContainerAndBlobNames(path, out containerName, out blobName);

            CloudBlobClient client = StorageAccount.CreateCloudBlobClient();

            var container = client.GetContainerReference(containerName);
            var blob = container.GetBlockBlobReference(blobName);

            string signiature = blob.GetSharedAccessSignature(new SharedAccessBlobPolicy()
            {
                SharedAccessStartTime = DateTime.Now,
                SharedAccessExpiryTime = DateTime.Now.Add(valid)
            }, policy.ToString());

            return new Uri(string.Format("{0}{1}", blob.Uri.AbsoluteUri, signiature));
        }

        #endregion

        public override bool EnsureAccessPolicy(string path, AccessPolicy policy)
        {
            string containerName, blobName;
            GetStorageContainerAndBlobNames(path, out containerName, out blobName);

            CloudBlobClient client = StorageAccount.CreateCloudBlobClient();

            var container = client.GetContainerReference(containerName);
            // var blob = container.GetBlockBlobReference(blobName); // not used in this scenario

            try
            {
                bool persist = false;

                BlobContainerPermissions permissions = container.GetPermissions();
                if (policy == AccessPolicy.None && permissions.SharedAccessPolicies.Count > 0) // remove all policies if any was set
                {
                    permissions.SharedAccessPolicies.Clear();
                    persist = true;
                }
                else
                {
                    if ((policy & AccessPolicy.Read) == AccessPolicy.Read && !permissions.SharedAccessPolicies.ContainsKey(AccessPolicy.Read.ToString())) // add read policy if does not exist
                    {
                        permissions.SharedAccessPolicies.Add(AccessPolicy.Read.ToString(), new SharedAccessBlobPolicy() { Permissions = SharedAccessBlobPermissions.Read });
                        persist = true;
                    }

                    if ((policy & AccessPolicy.Write) == AccessPolicy.Write && !permissions.SharedAccessPolicies.ContainsKey(AccessPolicy.Write.ToString())) // add write policy if does not exist
                    {
                        permissions.SharedAccessPolicies.Add(AccessPolicy.Write.ToString(), new SharedAccessBlobPolicy() { Permissions = SharedAccessBlobPermissions.Write });
                        persist = true;
                    }

                    // other policies would be List and Delete, but for now they are not used by this provider
                }

                if (persist) // save shared accesspolicies
                    container.SetPermissions(permissions, AccessCondition.GenerateIfMatchCondition(container.Properties.ETag));
            }
            catch (Exception ex)
            {
                throw new BusinessException(string.Format("Failed to ensure access policy for path: {0}", path), ex);
            }

            return true;
        }

        /// <summary>
        /// Returns the container and the blob names from a full path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="container">Returns the container portion of the path or full path if path is already a container.</param>
        /// <param name="blob">Returns the blob portion of the path from a full path.</param>
        private void GetStorageContainerAndBlobNames(string path, out string container, out string blob)
        {
            if (string.IsNullOrEmpty(path))
                throw new BusinessException("Cannot extract container and blob paths from an empty path.");

            blob = string.Empty;

            int end = path.IndexOf('/');

            if (end < 0) // the path is a container in itself
            {
                container = path;
                return;
            }

            container = path.Substring(0, end);

            if (end < path.Length - 1) // first slash is not the last character of the path
                blob = path.Substring(end + 1, path.Length - end - 1);
        }
    }
}
