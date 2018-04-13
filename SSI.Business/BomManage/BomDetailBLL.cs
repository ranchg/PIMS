using SSI.Entity.BomManage;
using SSI.Repository;
using SSI.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace SSI.Business.BomManage
{
    public class BomDetailBLL : RepositoryFactory<T_Bom_Detail>
    {
        public DataTable GetGridList(GridParam gp)
        {
            bool flag = false;
            string whereBom="WHERE 1=1";
            string bom = "T_BOM";
            string whereTotal = "WHERE 1=1";
            string querySql = "";
            if (!string.IsNullOrEmpty(gp.query))
            {
                IList conditions=gp.query.JsonToList<Condition>();
                foreach (Condition con in conditions)
                {
                    if (con.ParamType=="1")
                    {
                        flag = true;
                        switch (con.Operation)
                        {
                            case ConditionOperate.Like:
                                whereBom += " and " + con.ParamName + " like" + "'%" + con.ParamValue + "%' ";
                                break;
                            case ConditionOperate.Equal:
                                whereBom += " and " + con.ParamName + " = '" + con.ParamValue + "' ";
                                break;
                            case ConditionOperate.AfterDay:
                                whereBom += " and " + con.ParamName + " >= to_date('" + con.ParamValue + "','yyyy-mm-dd hh24:mi:ss') ";
                                break;
                            case ConditionOperate.BeforeDay:
                                whereBom += " and " + con.ParamName + " <= to_date('" + con.ParamValue + "','yyyy-mm-dd hh24:mi:ss') ";
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (con.Operation)
                        {
                            case ConditionOperate.Like:
                                whereTotal += " and " + con.ParamName + " like" + "'%" + con.ParamValue + "%' ";
                                break;
                            case ConditionOperate.Equal:
                                whereTotal += " and " + con.ParamName + " = '" + con.ParamValue + "' ";
                                break;
                            case ConditionOperate.AfterDay:
                                whereTotal += " and " + con.ParamName + " >= to_date('" + con.ParamValue + "','yyyy-mm-dd hh24:mi:ss') ";
                                break;
                            case ConditionOperate.BeforeDay:
                                whereTotal += " and " + con.ParamName + " <= to_date('" + con.ParamValue + "','yyyy-mm-dd hh24:mi:ss') ";
                                break;
                            default:
                                break;
                        }
                    }
                }
                if (flag)
                {
                    bom= string.Format(@"(SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY F_DATE DESC) RN,
                                                    T1.*
                                                    FROM T_BOM T1 {0}) WHERE RN=1) ", whereBom);
                }
                querySql=string.Format(@"SELECT * FROM (SELECT T1.F_ID,T1.F_NUM,
                                                    T2.F_NAME AS F_BOM_NAME,T2.F_CODE AS F_BOM_CODE,T2.F_VERSION AS F_BOM_VERSION,T2.F_DATE AS F_BOM_DATE,
                                                    T3.F_NAME AS F_PART_NAME,T3.F_CODE AS F_PART_CODE,T3.F_SPEC AS F_PART_SPEC
                                                    FROM T_BOM_DETAIL T1 
                                                    INNER JOIN {0} T2
                                                    ON T1.F_BOM_ID=T2.F_ID
                                                    INNER JOIN T_PART T3
                                                    ON T1.F_PART_ID=T3.F_ID
                                                    WHERE T2.F_DELETE_MARK=0 AND T3.F_DELETE_MARK=0) {1}", bom, whereTotal);
            }
            return Repository().FindTablePageBySql(querySql, ref gp);
        }


        public DataTable ExportExcel(string field, string query)
        {
            string select = "SELECT * FROM";
            if (!string.IsNullOrEmpty(field))
            {
                select = ConditionBuilder.GetSelectSql(field.JsonToList<Column>());
            }



            bool flag = false;
            string whereBom = "WHERE 1=1";
            string bom = "T_BOM";
            string whereTotal = "WHERE 1=1";
            string querySql = "";
            if (!string.IsNullOrEmpty(query))
            {
                IList conditions = query.JsonToList<Condition>();
                foreach (Condition con in conditions)
                {
                    if (con.ParamType == "1")
                    {
                        flag = true;
                        switch (con.Operation)
                        {
                            case ConditionOperate.Like:
                                whereBom += " and " + con.ParamName + " like" + "'%" + con.ParamValue + "%' ";
                                break;
                            case ConditionOperate.Equal:
                                whereBom += " and " + con.ParamName + " = '" + con.ParamValue + "' ";
                                break;
                            case ConditionOperate.AfterDay:
                                whereBom += " and " + con.ParamName + " >= to_date('" + con.ParamValue + "','yyyy-mm-dd hh24:mi:ss') ";
                                break;
                            case ConditionOperate.BeforeDay:
                                whereBom += " and " + con.ParamName + " <= to_date('" + con.ParamValue + "','yyyy-mm-dd hh24:mi:ss') ";
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (con.Operation)
                        {
                            case ConditionOperate.Like:
                                whereTotal += " and " + con.ParamName + " like" + "'%" + con.ParamValue + "%' ";
                                break;
                            case ConditionOperate.Equal:
                                whereTotal += " and " + con.ParamName + " = '" + con.ParamValue + "' ";
                                break;
                            case ConditionOperate.AfterDay:
                                whereTotal += " and " + con.ParamName + " >= to_date('" + con.ParamValue + "','yyyy-mm-dd hh24:mi:ss') ";
                                break;
                            case ConditionOperate.BeforeDay:
                                whereTotal += " and " + con.ParamName + " <= to_date('" + con.ParamValue + "','yyyy-mm-dd hh24:mi:ss') ";
                                break;
                            default:
                                break;
                        }
                    }
                }
                if (flag)
                {
                    bom = string.Format(@"(SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY F_DATE DESC) RN,
                                                    T1.*
                                                    FROM T_BOM T1 {0}) WHERE RN=1) ", whereBom);
                }
                querySql = string.Format(@"SELECT * FROM (SELECT T1.F_ID,T1.F_NUM,
                                                    T2.F_NAME AS F_BOM_NAME,T2.F_CODE AS F_BOM_CODE,T2.F_VERSION AS F_BOM_VERSION,T2.F_DATE AS F_BOM_DATE,
                                                    T3.F_NAME AS F_PART_NAME,T3.F_CODE AS F_PART_CODE,T3.F_SPEC AS F_PART_SPEC
                                                    FROM T_BOM_DETAIL T1 
                                                    INNER JOIN {0} T2
                                                    ON T1.F_BOM_ID=T2.F_ID
                                                    INNER JOIN T_PART T3
                                                    ON T1.F_PART_ID=T3.F_ID
                                                    WHERE T2.F_DELETE_MARK=0 AND T3.F_DELETE_MARK=0) {1}", bom, whereTotal);
            }
            string orderby = "ORDER BY F_PART_CODE DESC";
            //return Repository().FindTablePageBySql(querySql, ref gp);
            string sql = string.Format("{0} ({1}) ", select, querySql);
            return Repository().FindTableBySql(sql);
        }


        public DataTable getForm(int F_Id)
        {
            string querySql = string.Format(@"SELECT T1.F_ID,T1.F_NUM AS F_Part_Num,T2.F_NAME AS F_Bom_Name,T3.F_NAME AS F_Part_Name from T_BOM_DETAIL T1 
                                INNER JOIN T_BOM T2 ON
                                T1.F_BOM_ID=T2.F_ID
                                INNER JOIN T_PART T3 ON
                                T1.F_PART_ID=T3.F_ID WHERE T1.F_ID={0}", F_Id);
            return Repository().FindTableBySql(querySql);
        }

        public int SubmitForm(string F_Id, string F_Num)
        {
            StringBuilder execSql = new StringBuilder(string.Format(@"UPDATE T_BOM_DETAIL SET F_NUM={0} WHERE F_ID={1}", F_Num, F_Id));
            return Repository().ExecuteBySql(execSql);
        }

        public DataTable GetDetailById(int f_id, GridParam gp)
        {
            string bom = string.Format(@"(SELECT * FROM T_BOM T1 WHERE T1.F_ID={0})", f_id);
            string where = "WHERE 1=1";
            string querySql = "";
            if (!string.IsNullOrEmpty(gp.query))
            {
                IList conditions = gp.query.JsonToList<Condition>();
                foreach (Condition con in conditions)
                {
                    switch (con.Operation)
                    {
                        case ConditionOperate.Like:
                            where += " and " + con.ParamName + " like" + "'%" + con.ParamValue + "%' ";
                            break;
                        case ConditionOperate.Equal:
                            where += " and " + con.ParamName + " = '" + con.ParamValue + "' ";
                            break;
                        case ConditionOperate.AfterDay:
                            where += " and " + con.ParamName + " >= to_date('" + con.ParamValue + "','yyyy-mm-dd hh24:mi:ss') ";
                            break;
                        case ConditionOperate.BeforeDay:
                            where += " and " + con.ParamName + " <= to_date('" + con.ParamValue + "','yyyy-mm-dd hh24:mi:ss') ";
                            break;
                        default:
                            break;
                    }
                }
                querySql = string.Format(@"SELECT * FROM (SELECT T1.F_ID,T1.F_NUM,
                                                    T2.F_NAME AS F_BOM_NAME,T2.F_CODE AS F_BOM_CODE,T2.F_VERSION AS F_BOM_VERSION,T2.F_DATE AS F_BOM_DATE,
                                                    T3.F_NAME AS F_PART_NAME,T3.F_CODE AS F_PART_CODE,T3.F_SPEC AS F_PART_SPEC
                                                    FROM T_BOM_DETAIL T1 
                                                    INNER JOIN {0} T2
                                                    ON T1.F_BOM_ID=T2.F_ID
                                                    INNER JOIN T_PART T3
                                                    ON T1.F_PART_ID=T3.F_ID
                                                    WHERE T1.F_ENABLE_MARK=1 AND T2.F_DELETE_MARK=0 AND T3.F_DELETE_MARK=0) {1}", bom, where);
            }
            return Repository().FindTablePageBySql(querySql, ref gp);
        }

        public int Modify(int f_id, DbTransaction isOpenTrans)
        {
            StringBuilder execSql = new StringBuilder(string.Format("UPDATE T_BOM_DETAIL SET F_ENABLE_MARK=0 WHERE F_BOM_ID={0}", f_id));
            return Repository().ExecuteBySql(execSql);
        }
        
    }
}
