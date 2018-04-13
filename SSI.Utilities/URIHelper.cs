using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;

namespace SSI.Utilities
{
   public class URIHelper
    {
        static System.Text.Encoding encoding = System.Text.Encoding.UTF8;

        #region URL的64位编码
        public static string Base64Encrypt(string sourthUrl)
        {
            string eurl = HttpUtility.UrlEncode(sourthUrl);
            eurl = Convert.ToBase64String(encoding.GetBytes(eurl));
            return eurl;
        }
        #endregion

        #region URL的64位解码
        public static string Base64Decrypt(string eStr)
        {
            if (!IsBase64(eStr))
            {
                return eStr;
            }
            byte[] buffer = Convert.FromBase64String(eStr);
            string sourthUrl = encoding.GetString(buffer);
            sourthUrl = HttpUtility.UrlDecode(sourthUrl);
            return sourthUrl;
        }
        /// <summary>
        /// 是否是Base64字符串
        /// </summary>
        /// <param name="eStr"></param>
        /// <returns></returns>
        public static bool IsBase64(string eStr)
        {
            if ((eStr.Length % 4) != 0)
            {
                return false;
            }
            if (!Regex.IsMatch(eStr, "^[A-Z0-9/+=]*$", RegexOptions.IgnoreCase))
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 取得ip地址归属地
        public static string GetLocation(string ip=null)
        {
            try
            {
                string db = HttpContext.Current.Request.MapPath("~/Resource/ipDataBase.dat");
                IPScanner ips = new IPScanner();
                return ips.IPLocation(db, string.IsNullOrEmpty(ip)?GetUserIP():ip.Trim());
            }
            catch
            {
                return "";
            }
        }
        #endregion

        /// <summary>
        /// 浏览器和操作系统信息
        /// </summary>
        /// <returns></returns>
        public static string getClientBrowserOSInfo()
        {
            HttpContext cont = HttpContext.Current;
            string bs = cont.Request.Browser.Browser + cont.Request.Browser.Version;//cont.Request.Browser.Type.ToString();//浏览器信息
            //string os = cont.Request.UserAgent.ToString();
            string os = Environment.OSVersion.VersionString;//操作系统信息
            return bs + ";" + os;
        }

       public static string GetFilePath(string file)
        {
            return HttpContext.Current.Request.MapPath(file);
        }
        /// <summary>
        /// 添加URL参数
        /// </summary>
        public static string AddParam(string url, string paramName, string value)
        {
            Uri uri = new Uri(url);
            if (string.IsNullOrEmpty(uri.Query))
            {
                string eval = HttpContext.Current.Server.UrlEncode(value);
                return String.Concat(url, "?" + paramName + "=" + eval);
            }
            else
            {
                string eval = HttpContext.Current.Server.UrlEncode(value);
                return String.Concat(url, "&" + paramName + "=" + eval);
            }
        }
        /// <summary>
        /// 更新URL参数
        /// </summary>
        public static string UpdateParam(string url, string paramName, string value)
        {
            string keyWord = paramName + "=";
            int index = url.IndexOf(keyWord) + keyWord.Length;
            int index1 = url.IndexOf("&", index);
            if (index1 == -1)
            {
                url = url.Remove(index, url.Length - index);
                url = string.Concat(url, value);
                return url;
            }
            url = url.Remove(index, index1 - index);
            url = url.Insert(index, value);
            return url;
        }

        #region 分析URL所属的域
        public static void GetDomain(string fromUrl, out string domain, out string subDomain)
        {
            domain = "";
            subDomain = "";
            try
            {
                if (fromUrl.IndexOf("的名片") > -1)
                {
                    subDomain = fromUrl;
                    domain = "名片";
                    return;
                }

                UriBuilder builder = new UriBuilder(fromUrl);
                fromUrl = builder.ToString();

                Uri u = new Uri(fromUrl);

                if (u.IsWellFormedOriginalString())
                {
                    if (u.IsFile)
                    {
                        subDomain = domain = "客户端本地文件路径";

                    }
                    else
                    {
                        string Authority = u.Authority;
                        string[] ss = u.Authority.Split('.');
                        if (ss.Length == 2)
                        {
                            Authority = "www." + Authority;
                        }
                        int index = Authority.IndexOf('.', 0);
                        domain = Authority.Substring(index + 1, Authority.Length - index - 1).Replace("comhttp", "com");
                        subDomain = Authority.Replace("comhttp", "com");
                        if (ss.Length < 2)
                        {
                            domain = "不明路径";
                            subDomain = "不明路径";
                        }
                    }
                }
                else
                {
                    if (u.IsFile)
                    {
                        subDomain = domain = "客户端本地文件路径";
                    }
                    else
                    {
                        subDomain = domain = "不明路径";
                    }
                }
            }
            catch
            {
                subDomain = domain = "不明路径";
            }
        }

        /// <summary>
        /// 分析 url 字符串中的参数信息
        /// </summary>
        /// <param name="url">输入的 URL</param>
        /// <param name="baseUrl">输出 URL 的基础部分</param>
        /// <param name="nvc">输出分析后得到的 (参数名,参数值) 的集合</param>
        public static void ParseUrl(string url, out string baseUrl, out NameValueCollection nvc)
        {
            if (url == null)
                throw new ArgumentNullException("url");

            nvc = new NameValueCollection();
            baseUrl = "";

            if (url == "")
                return;

            int questionMarkIndex = url.IndexOf('?');

            if (questionMarkIndex == -1)
            {
                baseUrl = url;
                return;
            }
            baseUrl = url.Substring(0, questionMarkIndex);
            if (questionMarkIndex == url.Length - 1)
                return;
            string ps = url.Substring(questionMarkIndex + 1);

            // 开始分析参数对    
            Regex re = new Regex(@"(^|&)?(\w+)=([^&]+)(&|$)?", RegexOptions.Compiled);
            MatchCollection mc = re.Matches(ps);

            foreach (Match m in mc)
            {
                nvc.Add(m.Result("$2").ToLower(), m.Result("$3"));
            }
        }

        #endregion

        public static void SaveCheckbox(System.Web.UI.Page page, string namelist, string namelist2)
        {
            string js = ""; int i;
            string[] name = namelist.Split(',');
            string[] name2 = namelist2.Split(',');
            js += "\n\n<script>\r\n";
            js += "function ld()\n{\r\n";
            for (i = 0; i < name.Length; i++)
            {
                js += "var check=document.getElementsByName('" + name2[i] + "').item(0).value.split(',');\r\n";
                js += "for(i=0;i<document.getElementsByName('" + name[i] + "').length;i++)\r\n";
                js += "for(j=0;j<check.length;j++)\r\n";
                js += "if(check[j]==";
                js += "document.getElementsByName('" + name[i] + "')[i].value)";
                js += "document.getElementsByName('" + name[i] + "')[i].checked=true;\r\n";
            }
            js += "}\r\n";
            js += "window.onload=ld;\r\n";//将那个客户端脚本设置为window.onload发生时的事件。当Page_Load()执行完毕才触发window.onload事件 
            js += "</" + "script>\r\n\n";
            page.ClientScript.RegisterStartupScript(page.GetType(), "getbox", js);
        }

        public static string GetUrlFileName()
        {
            string p = System.IO.Path.GetFileName(HttpContext.Current.Request.FilePath);
            return p.Trim();
        }
        public static string GetUrl()
        {
            return HttpContext.Current.Request.FilePath;
        }

        public static string GetUserIP()
        {
            return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
        }


        #region "防刷新检测"
        /// <summary>
        /// 防刷新检测
        /// </summary>
        /// <param name="Second">访问间隔秒</param>
        /// <param name="UserSession"></param>
        public static bool CheckRefurbish(int Second, HttpSessionState UserSession)
        {

            bool i = true;
            if (UserSession["RefTime"] != null)
            {
                DateTime d1 = Convert.ToDateTime(UserSession["RefTime"]);
                DateTime d2 = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"));
                TimeSpan d3 = d2.Subtract(d1);
                if (d3.Seconds < Second)
                {
                    i = false;
                }
                else
                {
                    UserSession["RefTime"] = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
                }
            }
            else
            {
                UserSession["RefTime"] = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            }

            return i;
        }
        #endregion

        #region "返回请求地址"
        public static string GetReturnUrl()
        {
            string p;
            p = HttpContext.Current.Request.ServerVariables["PATH_INFO"] + "&" + HttpContext.Current.Request.ServerVariables["QUERY_STRING"];
            return p;
        }
        #endregion

        public static string GetHome()
        {
            //return HttpContext.Current.Request.ApplicationPath;
            return "/CommModule/Default.aspx";
        }
    }
}
