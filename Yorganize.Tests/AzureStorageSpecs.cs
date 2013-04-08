﻿using NUnit.Framework;
using System.Drawing.Imaging;
using System.IO;
using Yorganize.Business.Providers.Storage;

namespace Yorganize.Showcase.Tests
{
    [TestFixture]
    public class AzureStorageSpecs
    {
        private StorageProviderBase _provider;

        [Test]
        public void UploadFile()
        {
            _provider = StorageProviderManager.Provider;
            MemoryStream ms = new MemoryStream();
            Resources.yorganize_png.Save(ms, ImageFormat.Png);
            _provider.UploadFile(ms,"showcase/blog/posts/images/logo.png");
        }

        [Test]
        public void DownloadFile()
        {
         //   var stream = _provider.DownloadFile("showcase/blog/posts/images/logo.png");
        }
    }
}
