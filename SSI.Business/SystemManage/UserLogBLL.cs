using SSI.Entity.SystemManage;
using SSI.Repository;
using SSI.Utilities;
using System;
using System.Collections.Generic;
using System.Data;

namespace SSI.Business.SystemManage
{
    public class UserLogBLL : RepositoryFactory<T_User_Log>
    {
        //获取表格列表 By 阮创 2017/11/30
        public List<T_User_Log> GetGridList(GridParam gp)
        {
            string select = "SELECT * FROM", where = "WHERE 1=1";
            string from = "SELECT T1.* FROM T_USER_LOG T1 WHERE T1.F_DELETE_MARK = 0";
            if (!string.IsNullOrEmpty(gp.query))
            {
                where += ConditionBuilder.GetWhereSql2(gp.query.JsonToList<Condition>());
            }
            string sql = string.Format("{0} ({1}) TT {2}", select, from, where);
            return Repository().FindListPageBySql(sql, ref gp);
        }

        //导出数据 By 阮创 2017/11/30
        public DataTable Export(string field, string query)
        {
            string select = "SELECT * FROM", where = "WHERE 1=1";
            string from =
            @"SELECT T1.F_ACCOUNT,
                   T1.F_IPADDRESS,
                   T1.F_MENU,
                   T1.F_ACTION,
                   (CASE T1.F_RESULT_MARK
                     WHEN 1 THEN
                      '成功'
                     WHEN 2 THEN
                      '失败'
                     WHEN 3 THEN
                      '异常'
                     ELSE
                      '类型错误'
                   END) AS F_RESULT_MARK,
                   (CASE T1.F_ENABLE_MARK
                     WHEN 1 THEN
                      '有效'
                     WHEN 0 THEN
                      '无效'
                     ELSE
                      '类型错误'
                   END) AS F_ENABLE_MARK,
                   T1.F_CREATE_TIME
              FROM T_USER_LOG T1
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

        //写日志 By 阮创 2017/11/30
        public int Write(T_User_Log t_User_Log)
        {
            return Repository().Insert(t_User_Log.Create());
        }

        //删日志 By 阮创 2017/11/30
        public int Remove(int type)
        {
            string sql = "UPDATE T_USER_LOG T1 SET T1.F_DELETE_MARK = 1";
            switch (type)
            {
                case 7:
                    sql += " WHERE T1.F_CREATE_TIME < SYSDATE-7";
                    break;
                case 1:
                    sql += " WHERE T1.F_CREATE_TIME < ADD_MONTHS(SYSDATE,-1)";
                    break;
                case 3:
                    sql += " WHERE T1.F_CREATE_TIME < ADD_MONTHS(SYSDATE,-3)";
                    break;
                case 0:
                    break;
                default:
                    throw new Exception("参数错误");
            }
            return Repository().ExecuteBySql(new System.Text.StringBuilder(sql));
        }
    }
}
