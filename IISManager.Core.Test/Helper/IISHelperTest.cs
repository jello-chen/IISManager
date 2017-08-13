using Xunit;
using IISManager.Core.Helper;

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
    }
}
