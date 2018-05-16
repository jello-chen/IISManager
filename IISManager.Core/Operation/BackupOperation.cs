using IISManager.Core.Configuration;
using IISManager.Core.Utils;
using System.IO;

namespace IISManager.Core
{
    public class BackupOperation : OperationBase
    {
        public BackupOperation(Publish publish) : base(publish){}
        public override string Execute(Operation context)
        {
            string previousVersion = Publish.Version.Previous;
            string wp = Path.Combine(RootPath, Globals.WEB_ROOT);
            return ZipUtil.Zip(wp, Path.Combine(RootPath, previousVersion + ".zip"));
        }
    }
}
