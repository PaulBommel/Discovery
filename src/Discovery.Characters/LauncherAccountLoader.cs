using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Discovery.Characters
{
    public static class LauncherAccountLoader
    {
        public static IEnumerable<LauncherAccount> GetLauncherAccounts(string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException("File not found.", fileName);
            var doc = new XmlDocument();
            var accountList = doc.ReadNode(XmlReader.Create(fileName, new() { IgnoreComments = true, IgnoreWhitespace = true }));
            foreach(XmlNode node in accountList.ChildNodes)
                yield return XmlSerializer<LauncherAccount>.Deserialize(node.OuterXml);
        }
    }
}
