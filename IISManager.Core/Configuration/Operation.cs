using System.Collections.Generic;
using System.Xml.Serialization;

namespace IISManager.Core.Configuration
{
    [XmlType("operation")]
    public class Operation
    {
        [XmlAttribute("type")]
        public OperationType Type { get; set; }

        [XmlAttribute("src")]
        public string Src { get; set; }

        [XmlAttribute("target")]
        public string Target { get; set; }

        [XmlAttribute("exclude")]
        public string Exclude { get; set; } = string.Empty;

        [XmlArray("scripts")]
        [XmlArrayItem("script", typeof(Script))]
        [XmlArrayItem("group", typeof(GroupScript))]
        public List<Script> Scripts { get; set; }

        [XmlArray("rollbackScripts")]
        public List<Script> RollbackScripts { get; set; }

        [XmlAttribute("isConfig")]
        public bool IsConfig { get; set; }

        [XmlAttribute("autoAdd")]
        public bool AutoAdd { get; set; }

        [XmlAttribute("path")]
        public string Path { get; set; }

        [XmlAttribute("version")]
        public string Version { get; set; }
    }
}
