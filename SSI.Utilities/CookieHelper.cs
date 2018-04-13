using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SSI.Utilities
{
    public class CookieHelper
    {
        public static void DelCookie(string CookiesName)
        {
            HttpCookie cookie = new HttpCookie(CookiesName.Trim())
            {
                Expires = DateTime.Now.AddYears(-5)
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static string GetCookie(string strName)
        {
            if ((HttpContext.Current.Request.Cookies != null) && (HttpContext.Current.Request.Cookies[strName] != null))
            {
                return HttpContext.Current.Request.Cookies[strName].Value.ToString();
            }
            return "";
        }

        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = strValue;
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = strValue;
            cookie.Expires = DateTime.Now.AddMinutes((double)expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }
    }
}
