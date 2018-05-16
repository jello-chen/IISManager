using IISManager.Core.Configuration;
using IISManager.Core.Utils;
using System;
using System.IO;
using System.Xml;

namespace IISManager.Core
{
    public class ReplaceOperation : OperationBase
    {
        public ReplaceOperation(Publish publish) : base(publish) { }
        public override string Execute(Operation context)
        {
            string tp = Path.Combine(RootPath, context.Target);
            string sp = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, context.Src);
            if (context.IsConfig)
            {
                try
                {
                    XmlDocument srcDoc = new XmlDocument();
                    XmlDocument targetDoc = new XmlDocument();
                    srcDoc.Load(sp);
                    targetDoc.Load(tp);

                    string[] paths = context.Path.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var path in paths)
                    {
                        XmlNode sNode = srcDoc.SelectSingleNode(path);
                        if (sNode == null) continue;
                        XmlElement newNode = targetDoc.CreateElement(sNode.Name);
                        foreach (XmlAttribute attr in sNode.Attributes)
                        {
                            newNode.SetAttribute(attr.Name, attr.Value);
                        }
                        XmlNode tNode = targetDoc.SelectSingleNode(path);
                        string parentPath = path.Substring(0, path.LastIndexOf('/'));
                        XmlNode pNode = targetDoc.SelectSingleNode(parentPath);
                        if (tNode == null)
                        {
                            if (context.AutoAdd)
                            {
                                pNode.AppendChild(newNode);
                            }
                        }
                        else
                        {
                            pNode.ReplaceChild(newNode, tNode);
                        }
                    }
                    targetDoc.Save(tp);
                    return string.Empty;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            else
            {
                //if (string.IsNullOrWhiteSpace(context.Exclude))
                //{
                //    string cmd = $"XCOPY {sp} {tp} /S /E /Y";
                //    return CmdUtil.RunCmd(cmd);
                //}
                //else
                //{
                //    return CmdUtil.RunCmd($"ECHO {context.Exclude} > EXCLUDE.TXT") &
                //           CmdUtil.RunCmd($"XCOPY {sp} {tp} /S /E /Y /EXCLUDE:EXCLUDE.TXT") &
                //           CmdUtil.RunCmd("DEL EXCLUDE.TXT");
                //}
                return IoUtil.CopyDirectory(sp, tp, ignoreFunc: f => f == context.Exclude);
            }
        }
    }
}
