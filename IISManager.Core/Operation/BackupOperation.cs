using IISManager.Core.Configuration;
using IISManager.Core.Utils;
using System.IO;

namespace IISManager.Core
{
    public class BackupOperation : OperationBase
    {
        public BackupOperation() { }
        public BackupOperation(Publish publish) : base(publish){}
        public override string Execute(Operation context) => Execute(Publish.Version.Previous);

        public string Execute(string version)
        {
            string wp = Path.Combine(RootPath, Globals.WEB_ROOT);
            return ZipUtil.Zip(wp, Path.Combine(RootPath, version + ".zip"));
        }
    }
}
