using IISManager.Core;
using Nancy;
using Nancy.ModelBinding;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace IISManager.Workbench.Modules
{
    public class ApiModule: NancyModule
    {
        public ApiModule(): base("/Api")
        {
            Get["/GetVersions", true] = async (_, ct) => await GetVersions();
            Post["/Backup", true] = async (_, ct) => await Backup();
            Post["/Delete", true] = async (_, ct) => await Delete();
        }

        private Task<string> GetVersions()
        {
            StringBuilder builder = new StringBuilder();
            Regex versionRegex = new Regex(@"^\d+\.\d+\.\d+$", RegexOptions.Compiled);
            string rootPath = Globals.RootPath;
            foreach (var file in Directory.GetFiles(rootPath, "*.zip", SearchOption.TopDirectoryOnly))
            {
                string filename = Path.GetFileNameWithoutExtension(file);
                if(versionRegex.IsMatch(filename))
                {
                    builder.Append(filename);
                    builder.Append('\n');
                }
            }
            return Task.FromResult(builder.ToString().TrimEnd('\n'));
        }

        private async Task<string> Backup()
        {
            var parameter = this.Bind<BackupModel>();
            var version = parameter.Version;
            if (string.IsNullOrWhiteSpace(version))
                return "not a specified version";
            BackupOperation backupOperation = new BackupOperation();
            return await Task.FromResult(backupOperation.Execute(version));
        }

        private async Task<string> Delete()
        {
            var parameter = this.Bind<DeleteModel>();
            var path = parameter.Path;
            if (string.IsNullOrWhiteSpace(path))
                return "not a specified path";
            DeleteOperation deleteOperation = new DeleteOperation();
            return await Task.FromResult(deleteOperation.Execute(path));
        }
    }

    public class BackupModel
    {
        public string Version { get; set; }
    }
    public class DeleteModel
    {
        public string Path { get; set; }
    }
}
