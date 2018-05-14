using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IISManager.Core.Configuration
{
    [XmlRoot("publish")]
    public class Publish
    {
        [XmlArray("resources")]
        public List<Resource> Resources { get; set; }

        [XmlArray("operations")]
        public List<Operation> Operations { get; set; }

        [XmlElement("version")]
        public Version Version { get; set; }
    }
}
