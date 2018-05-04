using SSI.Entity.PartManage;
using SSI.Entity.SystemManage;
using SSI.Repository;
using SSI.Utilities;
using System.Collections.Generic;
using System.Data;

namespace SSI.Business.PartManage
{
    public class PartBuyBLL : RepositoryFactory<T_Part_Buy>
    {
        public DataTable GetGridList(GridParam gp)
        {
            string select = "SELECT * FROM", where = "WHERE 1=1";
            string from =
            @"SELECT T1.*,
                   T2.F_NAME F_PART_NAME,
                   T2.F_SPEC F_PART_SPEC,
                   T2.F_UNIT F_PART_UNIT
              FROM T_PART_BUY T1
              LEFT JOIN V_PART T2
                ON T2.F_CODE = T1.F_PART_CODE
             WHERE T1.F_DELETE_MARK = 0";
            if (!string.IsNullOrEmpty(gp.query))
            {
                where += ConditionBuilder.GetWhereSql2(gp.query.JsonToList<Condition>());
            }
            string sql = string.Format("{0} ({1}) TT {2}", select, from, where);
            return Repository().FindTablePageBySql(sql, ref gp);
        }

        public DataTable Export(string field, string query)
        {
            string select = "SELECT * FROM", where = "WHERE 1=1";
            string from =
            @"SELECT T1.*,
                   T2.F_NAME F_PART_NAME,
                   T2.F_SPEC F_PART_SPEC,
                   T2.F_UNIT F_PART_UNIT
              FROM T_PART_BUY T1
              LEFT JOIN V_PART T2
                ON T2.F_CODE = T1.F_PART_CODE
             WHERE T1.F_DELETE_MARK = 0";
            if (!string.IsNullOrEmpty(field))
            {
                select = ConditionBuilder.GetSelectSql(field.JsonToList<Column>());
            }
            if (!string.IsNullOrEmpty(query))
            {
                where += ConditionBuilder.GetWhereSql2(query.JsonToList<Condition>());
            }
            string sql = string.Format("{0} ({1}) TT {2}", select, from, where);
            return Repository().FindTableBySql(sql);
        }
    }
}
