using IISManager.Core;
using Nancy;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IISManager.Workbench.Modules
{
    public class ApiModule: NancyModule
    {
        public ApiModule(): base("/Api")
        {
            Get["/GetVersions", true] = async (x, ct) => await GetVersions();
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
    }
}
