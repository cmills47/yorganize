using System;
using System.Collections.Generic;
using System.Configuration.Provider;

namespace Yorganize.Business.Providers.Storage
{
    public abstract class StorageProviderBase : ProviderBase
    {
        // queues
        public abstract void AddEventToQueue(string queue, IQueueMessage message);
        public abstract T GetEventFromQueue<T>(string queue) where T : IQueueMessage;
        public abstract void RemoveEventFromQueue(string queue, IQueueMessage message);

        // files
        public abstract void UploadFile(System.IO.Stream source, string path, string contentType = null);
        public abstract System.IO.Stream DownloadFile(string path);
        public abstract bool DeleteFile(Uri uri);
        public abstract uint DeleteFiles(IEnumerable<Uri> uris);

        // url and shared access signiatures
        public abstract Uri GetFileUri(string path);
        public abstract Uri GetFileAccessUri(string path, AccessPolicy policy, TimeSpan valid);

        public abstract bool EnsureAccessPolicy(string path, AccessPolicy policy);
    }
}
