using IISManager.Core.Configuration;
using System.IO;

namespace IISManager.Core
{
    public class DeleteOperation : OperationBase
    {
        public DeleteOperation(Publish publish) : base(publish) { }
        public override bool Execute(Operation context)
        {
            if (string.IsNullOrEmpty(context.Path)) return false;
            string fullPath = Path.Combine(RootPath, context.Path);
            if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, true);
                return true;
            }
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }
            return false;
        }
    }
}
