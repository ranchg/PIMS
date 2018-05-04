using SSI.Entity.EquipmentManage;
using SSI.Entity.SystemManage;
using SSI.Repository;
using SSI.Utilities;
using System.Collections.Generic;
using System.Data;

namespace SSI.Business.SystemManage
{
    public class MaintenanceBLL : RepositoryFactory<T_Maintenance>
    {
        //获取表格列表 By 阮创 2017/11/30
        public DataTable GetGridList(GridParam gp, string F_Equipment_Id = null)
        {
            string select = "SELECT * FROM", where = "WHERE 1=1";
            string from = @"
                SELECT
	                T1.*, T2.F_CODE F_EQUIPMENT_CODE
                FROM
	                T_MAINTENANCE T1
                LEFT JOIN T_EQUIPMENT T2 ON T2.F_ID = T1.F_EQUIPMENT_ID
                WHERE
	                T1.F_DELETE_MARK = 0";
            if (!string.IsNullOrEmpty(F_Equipment_Id))
            {
                from += string.Format(" AND T1.F_EQUIPMENT_ID = '{0}'", F_Equipment_Id);
            }
            if (!string.IsNullOrEmpty(gp.query))
            {
                where += ConditionBuilder.GetWhereSql2(gp.query.JsonToList<Condition>());
            }
            string sql = string.Format("{0} ({1}) TT {2}", select, from, where);
            return Repository().FindTablePageBySql(sql, ref gp);
        }

        //获取列表 By 阮创 2017/11/30
        public List<T_Maintenance> GetList()
        {
            string sql =
            @"SELECT T1.*
              FROM T_MAINTENANCE T1
             WHERE T1.F_DELETE_MARK = 0
             ORDER BY T1.F_CREATE_TIME DESC";
            return Repository().FindListBySql(sql);
        }

        public DataTable Export(string field, string query)
        {
            string select = "SELECT * FROM", where = "WHERE 1=1";
            string from =
            @"SELECT T1.*, T2.F_NAME F_EQUIPMENT_CODE
              FROM T_MAINTENANCE T1
              LEFT JOIN T_EQUIPMENT T2
                ON T2.F_ID = T1.F_EQUIPMENT_ID
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
