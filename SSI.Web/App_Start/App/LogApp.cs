using SSI.Business.SystemManage;
using SSI.Entity.Manage;
using SSI.Entity.SystemManage;
using SSI.Utilities;
using System.ComponentModel;

namespace SSI.Web
{
    //日志 By 阮创 2017/11/30
    public static class LogApp
    {
        static UserLogBLL userLogBusiness = new UserLogBLL();

        //日志结果 By 阮创 2017/11/30
        public enum Result
        {
            [Description("成功")]
            Success = 1,
            [Description("失败")]
            Fail = 2,
            [Description("异常")]
            Exception = 3
        }
        //写日志 By 阮创 2017/11/30
        public static void Write(Result result)
        {
            T_User_Log t_User_Log = new T_User_Log();
            t_User_Log.F_User_Id = ManageProvider.Provider.Current().User.F_Id;
            t_User_Log.F_Account = ManageProvider.Provider.Current().User.F_Account;
            t_User_Log.F_IPAddress = URIHelper.GetUserIP();
            t_User_Log.F_Menu = AuthApp.CurrentMenu;
            t_User_Log.F_Action = AuthApp.CurrentAction;
            t_User_Log.F_Result_Mark = (int)result;
            t_User_Log.F_Enable_Mark = 1;
            t_User_Log.F_Delete_Mark = 0;
            userLogBusiness.Write(t_User_Log);
        }
    }
}