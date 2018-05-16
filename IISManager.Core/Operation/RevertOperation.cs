using IISManager.Core.Configuration;
using IISManager.Core.Utils;
using System.IO;

namespace IISManager.Core
{
    public class RevertOperation : OperationBase
    {
        public RevertOperation(Publish publish) : base(publish) { }
        public override string Execute(Operation context)
        {
            string revertVersion = string.IsNullOrWhiteSpace(context.Version) ? Publish.Version.Previous : context.Version;
            string revertVersionPath = Path.Combine(RootPath, revertVersion + ".zip");
            if (!File.Exists(revertVersionPath)) return $"Reverting {revertVersionPath} is not found.";
            string currentVersionDirectory = Path.Combine(RootPath, Globals.WEB_ROOT);
            string result = ZipUtil.UnZip(revertVersionPath, currentVersionDirectory);
            if (!string.IsNullOrWhiteSpace(result)) return result;
            return new ExecuteOperation(Publish).ExecuteScript(context.RollbackScripts);
        }
    }
}
