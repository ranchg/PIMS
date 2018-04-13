using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SSI.DataAccess
{
    public class LogFactory
    {
        static LogFactory()
        {
            FileInfo configFile = new FileInfo(HttpContext.Current.Server.MapPath("/Configs/log4net.config"));
            XmlConfigurator.Configure(configFile);
        }

        public static LogHelper GetLogger(string str)
        {
            return new LogHelper(LogManager.GetLogger(str));
        }

        public static LogHelper GetLogger(Type type)
        {
            return new LogHelper(LogManager.GetLogger(type));
        }
    }
}
