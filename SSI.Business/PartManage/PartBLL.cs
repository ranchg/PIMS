using SSI.Entity.PartManage;
using SSI.Entity.SystemManage;
using SSI.Repository;
using SSI.Utilities;
using System.Collections.Generic;
using System.Data;

namespace SSI.Business.PartManage
{
    public class PartBLL : RepositoryFactory<T_Part>
    {
        public List<T_Part> GetGridList(GridParam gp)
        {
            string select = "SELECT * FROM", where = "WHERE 1=1";
            string from =
            @"SELECT T1.*
              FROM T_PART T1
             WHERE T1.F_DELETE_MARK = 0";
            if (!string.IsNullOrEmpty(gp.search))
            {
                where += string.Format(" AND (F_NAME LIKE '%{0}%' OR F_CODE LIKE '%{0}%' OR F_SPEC LIKE '%{0}%')", gp.search);
            }
            string sql = string.Format("{0} ({1}) {2}", select, from, where);
            return Repository().FindListPageBySql(sql, ref gp);
        }

        public List<T_Part> GetList()
        {
            string sql =
            @"SELECT T1.*
              FROM T_PART T1
             WHERE T1.F_DELETE_MARK = 0
             ORDER BY T1.F_CREATE_TIME DESC";
            return Repository().FindListBySql(sql);
        }

        public DataTable Export(string field, string search)
        {
            string select = "SELECT * FROM", where = "WHERE 1=1";
            string from =
            @"SELECT T1.*
              FROM T_PART T1
             WHERE T1.F_DELETE_MARK = 0";
            if (!string.IsNullOrEmpty(field))
            {
                select = ConditionBuilder.GetSelectSql(field.JsonToList<Column>());
            }
            if (!string.IsNullOrEmpty(search))
            {
                where += string.Format(" AND (F_NAME LIKE '%{0}%' OR F_CODE LIKE '%{0}%' OR F_SPEC LIKE '%{0}%')", search);
            }
            string sql = string.Format("{0} ({1}) {2}", select, from, where);
            return Repository().FindTableBySql(sql);
        }

        public DataTable GetPart()
        {
            string querySql = "SELECT T1.F_ID,T1.F_CODE || '00000000' AS F_CODE  FROM T_PART T1 WHERE T1.F_DELETE_MARK = 0";
            return Repository().FindTableBySql(querySql);
        }
    }
}
