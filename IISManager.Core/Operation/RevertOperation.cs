using IISManager.Core.Configuration;
using IISManager.Core.Utils;
using System.IO;

namespace IISManager.Core
{
    public class RevertOperation : OperationBase
    {
        public RevertOperation(Publish publish) : base(publish) { }
        public override bool Execute(Operation context)
        {
            string revertVersion = string.IsNullOrWhiteSpace(context.Version) ? Publish.Version.Previous : context.Version;
            string revertVersionPath = Path.Combine(RootPath, revertVersion + ".zip");
            if (!File.Exists(revertVersionPath)) return false;
            string currentVersionDirectory = Path.Combine(RootPath, Globals.WEB_ROOT);
            return ZipUtil.UnZip(revertVersionPath, currentVersionDirectory);
        }
    }
}
