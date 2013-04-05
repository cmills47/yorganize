using System;
using System.Configuration;
using System.Web.Configuration;

namespace Yorganize.Business.Providers.Storage
{
    public class StorageProviderManager
    {
        private static StorageProviderBase defaultProvider;
        private static StorageProviderCollection providers;

        static StorageProviderManager()
        {
            Initialize();
        }

        private static void Initialize()
        {
            ProviderConfiguration configuration = (ProviderConfiguration)ConfigurationManager.GetSection("storage-providers");

            if (configuration == null)
                throw new ConfigurationErrorsException
                    ("The storage-providers configuration section is not set correctly.");

            providers = new StorageProviderCollection();

            ProvidersHelper.InstantiateProviders(configuration.Providers, providers, typeof(StorageProviderBase));

            providers.SetReadOnly();

            defaultProvider = providers[configuration.Default];

            if (defaultProvider == null)
                throw new Exception("No default storage provider is defined for the storage-providers section.");
        }

        public static StorageProviderBase Provider
        {
            get
            {
                return defaultProvider;
            }
        }

        public static StorageProviderCollection Providers
        {
            get
            {
                return providers;
            }
        }
    }
}
