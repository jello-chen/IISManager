using Xunit;
using IISManager.Core.Helper;
using System;
using System.IO;
using IISManager.Core.Utils;

namespace IISManager.Core.Test.Helper
{
    public class IISHelperTest
    {
        [Fact]
        public void TestIsIIS7AndHigher()
        {
            Assert.True(IISHelper.IsIIS7AndHigher());
        }

        [Fact]
        public void TestGetIISVersion()
        {
            Assert.True(IISHelper.GetIISVersion() == Domain.IISVersion.SevenOrUpper);
        }

        [Fact]
        public void TestPublishXmlIsExisting()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Publish");
            ZipUtil.UnZip(Path.Combine(path, "Publish.zip"), path);
            Assert.True(File.Exists(Path.Combine(path, "Publish.xml")));
        }
    }
}
