using System.Xml.Serialization;

namespace IISManager.Core.Configuration
{
    [XmlType("resource")]
    public class Resource
    {
        [XmlAttribute("path")]
        public string Path { get; set; }
    }
}
