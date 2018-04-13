using Newtonsoft.Json;
using SSI.DataAccess;
using SSI.Utilities;
using System;
using System.Web;

namespace SSI.Entity.Manage
{
    public class ManageProvider : IManageProvider
    {
        private string LoginProvider = ConfigHelper.AppSettings("LoginProvider") ?? "Session";
        private string LoginUserKey = "LoginUserKey";

        public virtual void AddCurrent(ManageUser user)
        {
            try
            {
                if (LoginProvider == "Session")
                {
                    SessionHelper.Add(LoginUserKey, DESEncrypt.Encrypt(JsonConvert.SerializeObject(user)));
                }
                else if (LoginProvider == "Cookie")
                {
                    CookieHelper.WriteCookie(LoginUserKey, DESEncrypt.Encrypt(JsonConvert.SerializeObject(user)), 0x5a0);
                }
                else
                {
                    throw new Exception("LoginProvider配置错误");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual ManageUser Current()
        {
            ManageUser user = null;
            try
            {
                if (LoginProvider == "Session")
                {
                    user = JsonConvert.DeserializeObject<ManageUser>(DESEncrypt.Decrypt(SessionHelper.Get(LoginUserKey).ToString()));
                }
                else if (LoginProvider == "Cookie")
                {
                    user = JsonConvert.DeserializeObject<ManageUser>(DESEncrypt.Decrypt(CookieHelper.GetCookie(LoginUserKey)));
                }
                else
                {
                    throw new Exception("LoginProvider配置错误");
                }
            }
            catch
            {
                user = null;
            }
            return user;
        }

        public virtual void EmptyCurrent()
        {
            if (LoginProvider == "Session")
            {
                SessionHelper.Remove(LoginUserKey.Trim());
            }
            else if (LoginProvider == "Cookie")
            {
                HttpCookie cookie = new HttpCookie(LoginUserKey.Trim())
                {
                    Expires = DateTime.Now.AddYears(-5)
                };
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            else
            {
                throw new Exception("LoginProvider配置错误");
            }
        }

        /// <summary>
        ///  Session会话是否存在
        /// </summary>
        /// <returns></returns>
        public virtual bool IsOverdue()
        {
            object session = null;
            if (LoginProvider == "Session")
            {
                session = SessionHelper.Get(LoginUserKey);
            }
            else if (LoginProvider == "Cookie")
            {
                session = CookieHelper.GetCookie(LoginUserKey);
            }
            else
            {
                throw new Exception("LoginProvider配置错误");
            }
            return session == null;
        }

        public static IManageProvider Provider
        {
            get
            {
                return new ManageProvider();
            }
        }
    }
}
