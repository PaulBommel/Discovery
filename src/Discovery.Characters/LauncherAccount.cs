using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Discovery.Characters
{
    [XmlRoot("account")]
    public record LauncherAccount
    {
        [XmlText]
        public string Name { get; init; }
        [XmlAttribute("description")]
        public string Description { get; init; }
        [XmlAttribute("category")]
        public string Category { get; init; }
        [XmlAttribute("code")]
        public string Code { get; init; }
        [XmlAttribute("signature")]
        public string Signature { get; init; }
    }
}
