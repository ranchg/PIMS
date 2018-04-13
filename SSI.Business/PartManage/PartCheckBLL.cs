using SSI.Entity.PartManage;
using SSI.Entity.SystemManage;
using SSI.Repository;
using SSI.Utilities;
using System;
using System.Collections.Generic;
using System.Data;

namespace SSI.Business.PartManage
{
    public class PartCheckBLL : RepositoryFactory<T_Part_Check>
    {
        public DataTable GetGridList(GridParam gp)
        {
            string select = "SELECT * FROM", where = "WHERE 1=1";
            string from =
            @"SELECT T1.*,
                   T2.F_TIME F_CHECK_TIME,
                   T3.F_NAME F_PART_NAME,
                   T3.F_CODE F_PART_CODE,
                   T3.F_SPEC F_PART_SPEC,
                   T3.F_UNIT F_PART_UNIT
              FROM T_PART_CHECK T1
              LEFT JOIN T_PART_CHECK_TIME T2
                ON T2.F_ID = T1.F_TIME_ID
              LEFT JOIN T_PART T3
                ON T3.F_ID = T1.F_PART_ID
             WHERE T1.F_DELETE_MARK = 0";
            if (!string.IsNullOrEmpty(gp.query))
            {
                where += ConditionBuilder.GetWhereSql2(gp.query.JsonToList<Condition>());
            }
            string sql = string.Format("{0} ({1}) {2}", select, from, where);
            return Repository().FindTablePageBySql(sql, ref gp);
        }

        public void Check(T_Part_Check_Time t_Part_Check_Time, DataTable dt_Part_Check)
        {
            var isOpenTrans = Repository().BeginTrans();
            try
            {
                new PartCheckTimeBLL().SubmitForm(t_Part_Check_Time, isOpenTrans);
                List<T_Part> t_Parts = new PartBLL().GetList();
                List<T_Part_Check> t_Part_Checks = new List<T_Part_Check>();
                foreach (DataRow dr in dt_Part_Check.Rows)
                {
                    T_Part_Check t_Part_Check = new T_Part_Check();
                    t_Part_Check.F_Time_Id = t_Part_Check_Time.F_Id;
                    t_Part_Check.F_Part_Id = t_Parts.Find(e => e.F_Code == dr["编码"].ToString()).F_Id;
                    t_Part_Check.F_Quantity = int.Parse(dr["数量"].ToString());
                    t_Part_Checks.Add(t_Part_Check);
                }
                InsertFormBatch(t_Part_Checks, isOpenTrans);
                Repository().Commit();
            }
            catch (Exception ex)
            {
                Repository().Rollback();
                throw new Exception("操作失败：请检查盘点清单");
            }
        }

        public DataTable Export(string field, string query)
        {
            string select = "SELECT * FROM", where = "WHERE 1=1";
            string from =
            @"SELECT T1.*,
                   T2.F_TIME F_CHECK_TIME,
                   T3.F_NAME F_PART_NAME,
                   T3.F_CODE F_PART_CODE,
                   T3.F_SPEC F_PART_SPEC,
                   T3.F_UNIT F_PART_UNIT
              FROM T_PART_CHECK T1
              LEFT JOIN T_PART_CHECK_TIME T2
                ON T2.F_ID = T1.F_TIME_ID
              LEFT JOIN T_PART T3
                ON T3.F_ID = T1.F_PART_ID
             WHERE T1.F_DELETE_MARK = 0";
            if (!string.IsNullOrEmpty(field))
            {
                select = ConditionBuilder.GetSelectSql(field.JsonToList<Column>());
            }
            if (!string.IsNullOrEmpty(query))
            {
                where += ConditionBuilder.GetWhereSql2(query.JsonToList<Condition>());
            }
            string sql = string.Format("{0} ({1}) {2}", select, from, where);
            return Repository().FindTableBySql(sql);
        }
    }
}
