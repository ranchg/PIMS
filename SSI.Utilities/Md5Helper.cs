using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace SSI.Utilities
{
    public class Md5Helper
    {
        public static string MD5(string str, int code)
        {
            string str2 = string.Empty;
            if (code == 0x10)
            {
                str2 = FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").Substring(8, 0x10);
            }
            if (code == 0x20)
            {
                str2 = FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
            }
            return str2;
        }
    }
}
