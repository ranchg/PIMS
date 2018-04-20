using SSI.Entity.SystemManage;
using SSI.Repository;
using SSI.Utilities;
using System.Collections.Generic;
using System.Data;

namespace SSI.Business.SystemManage
{
    public class RoleBLL : RepositoryFactory<T_Role>
    {
        //获取表格列表 By 阮创 2017/11/30
        public DataTable GetGridList(GridParam gp)
        {
            string select = "SELECT * FROM", where = "WHERE 1=1";
            string from =
            @"SELECT T1.*, T2.F_NAME F_ORG_NAME
              FROM T_ROLE T1
              LEFT JOIN T_ORG T2
                ON T2.F_ID = T1.F_ORG_ID
             WHERE T1.F_DELETE_MARK = 0";
            if (!string.IsNullOrEmpty(gp.search))
            {
                where += string.Format(" AND (F_ORG_NAME LIKE '%{0}%' OR F_NAME LIKE '%{0}%')", gp.search);
            }
            string sql = string.Format("{0} ({1}) TT {2}", select, from, where);
            return Repository().FindTablePageBySql(sql, ref gp);
        }

        //获取列表 By 阮创 2017/11/30
        public List<T_Role> GetList()
        {
            string sql =
            @"SELECT T1.*
              FROM T_ROLE T1
             WHERE T1.F_DELETE_MARK = 0
             ORDER BY T1.F_CREATE_TIME DESC";
            return Repository().FindListBySql(sql);
        }

        //根据用户ID获取列表 By 阮创 2017/11/30
        public List<T_Role> GetListByUserId(int userId)
        {
            string sql = string.Format(
            @"SELECT T1.*
              FROM T_ROLE T1
              JOIN T_USER_ROLE T2
                ON T2.F_ROLE_ID = T1.F_ID
             WHERE T1.F_DELETE_MARK = 0
               AND T2.F_DELETE_MARK = 0
               AND T2.F_USER_ID = {0}
             ORDER BY T1.F_CREATE_TIME DESC", userId);
            return Repository().FindListBySql(sql);
        }
    }
}
