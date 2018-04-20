using SSI.Entity.Manage;
using SSI.Entity.SystemManage;
using SSI.Repository;
using SSI.Utilities;
using System;
using System.Collections.Generic;

namespace SSI.Business.SystemManage
{
    public class UserBLL : RepositoryFactory<T_User>
    {
        //获取表格列表 By 阮创 2017/11/30
        public List<T_User> GetGridList(GridParam gp)
        {
            string select = "SELECT * FROM", where = "WHERE 1=1";
            string from =
            @"SELECT T1.*
              FROM T_USER T1
             WHERE T1.F_DELETE_MARK = 0
               AND T1.F_SYSTEM_MARK = 0";
            if (!string.IsNullOrEmpty(gp.search))
            {
                where += string.Format(" AND (F_ACCOUNT LIKE '%{0}%' OR F_REAL_NAME LIKE '%{0}%')", gp.search);
            }
            string sql = string.Format("{0} ({1}) TT {2}", select, from, where);
            return Repository().FindListPageBySql(sql, ref gp);
        }

        //获取列表 By 阮创 2017/11/30
        public List<T_User> GetList()
        {
            string sql =
            @"SELECT T1.*
              FROM T_USER T1
             WHERE T1.F_DELETE_MARK = 0
               AND T1.F_SYSTEM_MARK = 0
             ORDER BY T1.F_CREATE_TIME DESC";
            return Repository().FindListBySql(sql);
        }

        //根据用户账号获取实体 By 阮创 2017/11/30
        public T_User GetEntityByAccount(string account)
        {
            string sql = string.Format(
            @"SELECT T1.*
              FROM T_USER T1
             WHERE T1.F_DELETE_MARK = 0
               AND UPPER(T1.F_ACCOUNT) = UPPER('{0}')", account);
            return Repository().FindEntityBySql(sql);
        }

        //检查登录 By 阮创 2017/11/30
        public T_User CheckLogin(string account, string password)
        {
            T_User t_User = GetEntityByAccount(account);
            if (t_User != null && t_User.F_Id != 0)
            {
                if (t_User.F_Enable_Mark == 1)
                {
                    if (t_User.F_Password == Md5Helper.MD5(password, 0x20).ToUpper())
                    {
                        return t_User;
                    }
                    else
                    {
                        throw new Exception("帐号或密码错误");
                    }
                }
                else
                {
                    throw new Exception("帐号被禁用");
                }
            }
            else
            {
                throw new Exception("帐号不存在");
            }
        }

        public void AddProvider(T_User t_User)
        {
            ManageUser manageUser = new ManageUser();
            manageUser.User = GetForm(t_User.F_Id);
            if (t_User.F_System_Mark == 1)
            {
                manageUser.Orgs = new OrgBLL().GetTreeListByUserId(0);
                manageUser.Roles = new RoleBLL().GetListByUserId(0);
                manageUser.Menus = new MenuBLL().GetList();
                manageUser.Actions = new ActionBLL().GetList();
            }
            else
            {
                manageUser.Orgs = new OrgBLL().GetTreeListByUserId(t_User.F_Id);
                manageUser.Roles = new RoleBLL().GetListByUserId(t_User.F_Id);
                manageUser.Menus = new MenuBLL().GetTreeByUserId(t_User.F_Id);
                manageUser.Actions = new ActionBLL().GetListByUserId(t_User.F_Id);
            }
            ManageProvider.Provider.AddCurrent(manageUser);
        }
    }
}
