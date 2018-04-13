using SSI.Entity.SystemManage;
using SSI.Repository;
using SSI.Utilities;
using System.Collections.Generic;
using System.Data;

namespace SSI.Business.SystemManage
{
    public class ActionBLL : RepositoryFactory<T_Action>
    {
        //获取表格列表 By 阮创 2017/11/30
        public DataTable GetGridList(GridParam gp)
        {
            string select = "SELECT * FROM", where = "WHERE 1=1";
            string from =
            @"SELECT T1.*, T2.F_NAME F_MENU_NAME
              FROM T_ACTION T1
              LEFT JOIN T_MENU T2
                ON T2.F_ID = T1.F_MENU_ID
             WHERE T1.F_DELETE_MARK = 0";
            if (!string.IsNullOrEmpty(gp.search))
            {
                where += string.Format(" AND (F_MENU_NAME LIKE '%{0}%' OR F_NAME LIKE '%{0}%' OR F_CODE LIKE '%{0}%' OR F_TARGET LIKE '%{0}%')", gp.search);
            }
            string sql = string.Format("{0} ({1}) {2}", select, from, where);
            return Repository().FindTablePageBySql(sql, ref gp);
        }
        //获取列表 By 阮创 2017/11/30
        public List<T_Action> GetList()
        {
            string sql =
            @"SELECT T1.*
              FROM T_ACTION T1
             WHERE T1.F_DELETE_MARK = 0
             ORDER BY T1.F_CREATE_TIME DESC";
            return Repository().FindListBySql(sql);
        }

        //根据用户ID获取列表 By 阮创 2017/11/30
        public List<T_Action> GetListByUserId(int userId)
        {
            string sql = string.Format(
            @"SELECT T1.*
              FROM T_ACTION T1
              JOIN T_ROLE_ACTION T2
                ON T2.F_ACTION_ID = T1.F_ID
              JOIN T_USER_ROLE T3
                ON T3.F_ROLE_ID = T2.F_ROLE_ID
             WHERE T1.F_DELETE_MARK = 0
               AND T2.F_DELETE_MARK = 0
               AND T3.F_DELETE_MARK = 0
               AND T3.F_USER_ID = {0}
             ORDER BY T1.F_CREATE_TIME DESC", userId);
            return Repository().FindListBySql(sql);
        }

        //根据角色ID获取列表 By 阮创 2017/11/30
        public List<T_Action> GetListByRoleId(int roleId)
        {
            string sql = string.Format(
            @"SELECT T1.*
              FROM T_ACTION T1
              JOIN T_ROLE_ACTION T2
                ON T2.F_ACTION_ID = T1.F_ID
             WHERE T1.F_DELETE_MARK = 0
               AND T2.F_DELETE_MARK = 0
               AND T2.F_ROLE_ID = {0}
             ORDER BY T1.F_CREATE_TIME DESC", roleId);
            return Repository().FindListBySql(sql);
        }

        //校验操作 By 阮创 2017/11/30
        public bool Validate(int userId,string menu,string action)
        {
            string sql = string.Format(
            @"SELECT COUNT(1)
              FROM T_MENU T1
              JOIN T_ACTION T2
                ON T2.F_MENU_ID = T1.F_ID
              JOIN T_ROLE_ACTION T3
                ON T3.F_ACTION_ID = T2.F_ID
              JOIN T_USER_ROLE T4
                ON T4.F_ROLE_ID = T3.F_ROLE_ID
             WHERE T1.F_DELETE_MARK = 0
               AND T2.F_DELETE_MARK = 0
               AND T3.F_DELETE_MARK = 0
               AND T4.F_DELETE_MARK = 0
               AND T4.F_USER_ID = {0}
               AND T1.F_TARGET = '{1}'
               AND T2.F_TARGET = '{2}'
             ORDER BY T1.F_PARENT_ID ASC, T1.F_SORT ASC", userId, menu, action);
            return Repository().FindCountBySql(sql) > 0;
        }
    }
}
