using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SSI.Utilities
{
    public class SessionHelper
    {
        public static void Add(string strSessionName, object objValue)
        {
            HttpContext.Current.Session[strSessionName] = objValue;
        }

        public static object Get(string strSessionName)
        {
            return HttpContext.Current.Session[strSessionName];
        }

        public static void Remove(string strSessionName)
        {
            HttpContext.Current.Session.Remove(strSessionName);
        }

        public static void Set(string strSessionName, object objValue, int iExpires, int iYear)
        {
            HttpContext.Current.Session[strSessionName] = objValue;
            if (iExpires > 0)
            {
                HttpContext.Current.Session.Timeout = iExpires;
            }
            else if (iYear > 0)
            {
                HttpContext.Current.Session.Timeout = 0x80520 * iYear;
            }
        }

        public static void SaveCookie(string name, string value,int day=1)
        {

            HttpCookie cookie = new HttpCookie(name);
            cookie.Name = name;
            cookie.Value = value;
            cookie.Expires = DateTime.Now.AddDays(day);   // 不加这句，就读不到，不知何故？  
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        public static string ReadCookie(string name)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie != null)
            {
                return cookie.Value;
            }
            return "";  
        }
    }
}
