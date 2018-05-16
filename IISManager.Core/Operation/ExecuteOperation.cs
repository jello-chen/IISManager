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
using System.Threading;
using System.Collections.Concurrent;

namespace IISManager.Core
{
    public class ExecuteOperation : OperationBase
    {
        static Regex regex = new Regex(".+;.+;.+;.*", RegexOptions.Compiled);
        public ExecuteOperation(Publish publish) : base(publish) { }
        public override string Execute(Operation context)
        {
            ConcurrentBag<string> results = new ConcurrentBag<string>();
            List<Script> scripts = context.Scripts;
            Task.WaitAll(scripts.Select(s => Task.Run(() =>
            {
                string sp = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, s.Path);
                string cs = regex.IsMatch(s.Database) ? s.Database : GetConnectionStringFromConfig(s.Database);
                string result = ExecuteScript(cs, sp);
                if (!string.IsNullOrWhiteSpace(result))
                    results.Add(result);
            })).ToArray());
            return results.Count == 0 ? string.Empty : string.Join("\n", results);
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

        private string ExecuteScript(string cs, string scriptFile)
        {
            // SQLCMD can not be used without installing sql server
            //SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(cs);
            //string cmd = $"SQLCMD -S {builder.DataSource} -U {builder.UserID} -P {builder.Password} -d {builder.InitialCatalog} -i {script}";
            //return CmdUtil.RunCmd(cmd);

            try
            {
                string script = File.ReadAllText(scriptFile);

                // split script on GO command
                IEnumerable<string> commandStrings = Regex.Split(script, @"^\s*GO\s*$",
                    RegexOptions.Multiline | RegexOptions.IgnoreCase);
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    connection.Open();
                    foreach (string commandString in commandStrings)
                    {
                        if (commandString.Trim() != "")
                        {
                            using (var command = new SqlCommand(commandString, connection))
                            {
                                try
                                {
                                    command.ExecuteNonQuery();
                                }
                                catch (SqlException ex)
                                {
                                    string spError = commandString.Length > 100 ? commandString.Substring(0, 100) + " ...\n..." : commandString;
                                    return string.Format("Please check the SqlServer script.\nFile: {0} \nLine: {1} \nError: {2} \nSQL Command: \n{3}", scriptFile, ex.LineNumber, ex.Message, spError);
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }

    }
}
