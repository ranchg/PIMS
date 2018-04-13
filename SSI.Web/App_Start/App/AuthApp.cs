using SSI.Entity.Manage;
using System.Web;

namespace SSI.Web
{
    //验证 By 阮创 2017/11/30
    public static class AuthApp
    {
        //当前菜单 By 阮创 2017/11/30
        public static string CurrentMenu
        {
            get
            {
                if (HttpContext.Current.Request.RequestContext.RouteData.DataTokens.ContainsKey("area"))
                {
                    return string.Format("/{0}/{1}/", HttpContext.Current.Request.RequestContext.RouteData.DataTokens["area"], HttpContext.Current.Request.RequestContext.RouteData.Values["controller"]);
                }
                else
                {
                    return string.Format("/{0}/", HttpContext.Current.Request.RequestContext.RouteData.Values["controller"]);
                }
            }
        }
        //当前操作 By 阮创 2017/11/30
        public static string CurrentAction
        {
            get
            {
                return HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
            }
        }
        //进行验证 By 阮创 2017/11/30
        public static bool Auth()
        {
            return (ManageProvider.Provider.Current().Menus.FindAll(t => t.F_Target == CurrentMenu).Count > 0 && ManageProvider.Provider.Current().Actions.FindAll(t => t.F_Target == CurrentAction).Count > 0);
        }
    }
}