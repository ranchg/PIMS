using SSI.Entity.PartManage;
using SSI.Entity.SystemManage;
using SSI.Repository;
using SSI.Utilities;
using System.Collections.Generic;
using System.Data;

namespace SSI.Business.PartManage
{
    public class PartBLL
    {
        public List<T_Part> GetGridList(GridParam gp)
        {
            string select = "SELECT * FROM", where = "WHERE 1=1";
            string from =
            @"SELECT T1.*
              FROM V_PART T1";
            if (!string.IsNullOrEmpty(gp.search))
            {
                where += string.Format(" AND (F_NAME LIKE '%{0}%' OR F_CODE LIKE '%{0}%' OR F_SPEC LIKE '%{0}%')", gp.search);
            }
            string sql = string.Format("{0} ({1}) TT {2}", select, from, where);
            return new Repository<T_Part>().FindListPageBySql(sql, ref gp);
        }

        public List<T_Part> GetList()
        {
            string sql =
            @"SELECT T1.*
              FROM V_PART T1
             ORDER BY T1.F_CREATE_TIME DESC";
            return new Repository<T_Part>().FindListBySql(sql);
        }

        public T_Part GetForm(string F_Code)
        {
            string sql = string.Format(
            @"SELECT T1.*
              FROM V_PART T1
             WHERE T1.F_CODE = '{0}'", F_Code);
            return new Repository<T_Part>().FindEntityBySql(sql);
        }

        public DataTable Export(string field, string search)
        {
            string select = "SELECT * FROM", where = "WHERE 1=1";
            string from =
            @"SELECT T1.*
              FROM V_PART T1";
            if (!string.IsNullOrEmpty(field))
            {
                select = ConditionBuilder.GetSelectSql(field.JsonToList<Column>());
            }
            if (!string.IsNullOrEmpty(search))
            {
                where += string.Format(" AND (F_NAME LIKE '%{0}%' OR F_CODE LIKE '%{0}%' OR F_SPEC LIKE '%{0}%')", search);
            }
            string sql = string.Format("{0} ({1}) TT {2}", select, from, where);
            return new Repository<T_Part>().FindTableBySql(sql);
        }
    }
}
