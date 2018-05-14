using IISManager.Core.Configuration;
using IISManager.Core.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace IISManager.Core
{
    public class ExecuteOperation : OperationBase
    {
        static Regex regex = new Regex(".+;.+;.+;.*", RegexOptions.Compiled);
        public ExecuteOperation(Publish publish) : base(publish) { }
        public override bool Execute(Operation context)
        {
            List<Script> scripts = context.Scripts;
            Task.WaitAll(scripts.Select(s => Task.Run(() =>
            {
                string sp = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, s.Path);
                string cs = regex.IsMatch(s.Database) ? s.Database : GetConnectionStringFromConfig(s.Database);
                ExecuteScript(cs, sp);
            })).ToArray());
            return true;
        }

        private string GetConnectionStringFromConfig(string path)
        {
            string[] slices = path.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
            if (slices.Length != 2) throw new ArgumentException("The configuration on database of script is invalid.");
            string config = Path.Combine(RootPath, slices[0]);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(config);
            XmlNode csn = xmlDocument.SelectSingleNode(slices[1]);
            return csn.Attributes["connectionString"].Value;
        }

        private bool ExecuteScript(string cs, string script)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(cs);
            string cmd = $"SQLCMD -S {builder.DataSource} -U {builder.UserID} -P {builder.Password} -d {builder.InitialCatalog} -i {script}";
            return CmdUtil.RunCmd(cmd);
        }

    }
}
