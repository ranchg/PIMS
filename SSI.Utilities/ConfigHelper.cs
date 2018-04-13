using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace SSI.Utilities
{
    public class ConfigHelper
    {
        public static string AppSettings(string key)
        {
            return ConfigurationManager.AppSettings[key].ToString().Trim();
        }

        public static string ConnectionStrings(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString.Trim();
        }

        public static void SetValue(string key, string value)
        {
            XmlDocument document = new XmlDocument();
            document.Load(HttpContext.Current.Server.MapPath("/XmlConfig/Config.xml"));
            XmlNode node = document.SelectSingleNode("//appSettings");
            XmlElement element = (XmlElement)node.SelectSingleNode("//add[@key='" + key + "']");
            if (element != null)
            {
                element.SetAttribute("value", value);
            }
            else
            {
                XmlElement newChild = document.CreateElement("add");
                newChild.SetAttribute("key", key);
                newChild.SetAttribute("value", value);
                node.AppendChild(newChild);
            }
            document.Save(HttpContext.Current.Server.MapPath("/XmlConfig/Config.xml"));
        }
    }
}
