using System.Xml.Serialization;

namespace IISManager.Core.Configuration
{
    [XmlType("script")]
    public class Script
    {
        [XmlAttribute("path")]
        public string Path { get; set; }
        [XmlAttribute("database")]
        public string Database { get; set; }
    }
}
