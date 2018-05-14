using System.Xml.Serialization;

namespace IISManager.Core.Configuration
{
    [XmlType("version")]
    public class Version
    {
        [XmlAttribute("current")]
        public string Current { get; set; }
        [XmlAttribute("previous")]
        public string Previous { get; set; }
    }
}
