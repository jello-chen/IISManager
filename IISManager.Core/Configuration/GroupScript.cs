using System.Collections.Generic;
using System.Xml.Serialization;

namespace IISManager.Core.Configuration
{
    public class GroupScript : Script
    {
        [XmlElement("script", typeof(Script))]
        public List<Script> Group { get; set; }
    }
}
