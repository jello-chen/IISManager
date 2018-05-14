using System.Xml.Serialization;

namespace IISManager.Core.Configuration
{
    public enum OperationType
    {
        [XmlEnum("revert")]
        Revert,
        [XmlEnum("backup")]
        Backup,
        [XmlEnum("replace")]
        Replace,
        [XmlEnum("skip")]
        Skip,
        [XmlEnum("add")]
        Add,
        [XmlEnum("delete")]
        Delete,
        [XmlEnum("execute")]
        Execute
    }
}
