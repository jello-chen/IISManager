using Xunit;
using IISManager.Core.Helper;
using System;
using System.IO;
using IISManager.Core.Utils;
using System.Xml.Serialization;
using IISManager.Core.Configuration;
using System.Xml;
using System.Linq;

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

        [Fact]
        public void TestGroupScriptParsing()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Publish");

            ZipUtil.UnZip(Path.Combine(path, "Publish.zip"), path);
            string xmlPath = Path.Combine(path, "Publish.xml");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Publish));
            using (XmlReader xmlReader = new XmlTextReader(xmlPath))
            {
                Publish publish = (Publish)xmlSerializer.Deserialize(xmlReader);
                int groupScriptCount = publish.Operations
                    .Where(p => p.Type == OperationType.Execute)
                    .SelectMany(p => p.Scripts.Where(s => s.GetType() == typeof(GroupScript)))
                    .Count();
                Assert.True(groupScriptCount == 1);
            }
        }
    }
}
