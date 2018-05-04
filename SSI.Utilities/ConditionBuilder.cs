using SSI.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Text;

namespace SSI.Utilities
{
    public class ConditionBuilder
    {

        public static string GetWhereSql(string modelName, IList conditions, out List<DbParameter> parameter, string orderField = "", string orderType = "")
        {
            string str = DbFactory.CreateDbParmCharacter();
            List<DbParameter> list = new List<DbParameter>();
            StringBuilder builder = new StringBuilder();
            if (conditions.Count > 0)
            {
                builder.Append(" AND");
            }
            int num = 0;
            foreach (Condition condition in conditions)
            {
                DateTime dateTime;
                DateTime time2;
                if (condition.ParamValue == null)
                {
                    continue;
                }
                string str2 = "";
                if ((conditions.Count - 1) == num)
                {
                    str2 = "";
                }
                else
                {
                    str2 = "AND";
                }
                string paramName = modelName == null ? condition.ParamName : modelName + "." + condition.ParamName;
                string str4 = condition.ParamName + num;
                switch (condition.Operation)
                {
                    case ConditionOperate.Equal:
                        builder.Append(" "+ paramName + " = " + str + str4 + " " + str2);
                        list.Add(DbFactory.CreateDbParameter(str + str4, condition.ParamValue));
                        break;

                    case ConditionOperate.NotEqual:
                        builder.Append(" "+ paramName + " <> " + str + str4 + " " + str2);
                        list.Add(DbFactory.CreateDbParameter(str + str4, condition.ParamValue));
                        break;

                    case ConditionOperate.Greater:
                        builder.Append(" "+ paramName + " > " + str + str4 + " " + str2);
                        list.Add(DbFactory.CreateDbParameter(str + str4, condition.ParamValue));
                        break;

                    case ConditionOperate.GreaterThan:
                        builder.Append(" "+ paramName + " >= " + str + str4 + " " + str2);
                        list.Add(DbFactory.CreateDbParameter(str + str4, condition.ParamValue));
                        break;

                    case ConditionOperate.Less:
                        builder.Append(" "+ paramName + " < " + str + str4 + " " + str2);
                        list.Add(DbFactory.CreateDbParameter(str + str4, condition.ParamValue));
                        break;

                    case ConditionOperate.LessThan:
                        builder.Append(" "+ paramName + " <= " + str + str4 + " " + str2);
                        list.Add(DbFactory.CreateDbParameter(str + str4, condition.ParamValue));
                        break;

                    case ConditionOperate.Null:
                        builder.Append(string.Format(" "+ "{0} is null ", paramName) + " " + str2);
                        break;

                    case ConditionOperate.NotNull:
                        builder.Append(string.Format(" "+ "{0} is not null ", paramName) + " " + str2);
                        break;

                    case ConditionOperate.Like:
                        builder.Append(" "+ paramName + " like " + str + str4 + " " + str2);
                        list.Add(DbFactory.CreateDbParameter(str + str4, "%" + condition.ParamValue + "%"));
                        break;

                    case ConditionOperate.NotLike:
                        builder.Append(" "+ paramName + " not like " + str + str4 + " " + str2);
                        list.Add(DbFactory.CreateDbParameter(str + str4, "%" + condition.ParamValue + "%"));
                        break;

                    case ConditionOperate.LeftLike:
                        builder.Append(" "+ paramName + " like " + str + str4 + " " + str2);
                        list.Add(DbFactory.CreateDbParameter(str + str4, "%" + condition.ParamValue));
                        break;

                    case ConditionOperate.RightLike:
                        builder.Append(" "+ paramName + " like " + str + str4 + " " + str2);
                        list.Add(DbFactory.CreateDbParameter(str + str4, condition.ParamValue + "%"));
                        break;

                    case ConditionOperate.Yesterday:
                        dateTime = CommonHelper.GetDateTime(DateTime.Now.AddDays(-1.0).ToString("yyyy-MM-dd") + " 00:00");
                        time2 = CommonHelper.GetDateTime(DateTime.Now.AddDays(-1.0).ToString("yyyy-MM-dd") + " 23:59");
                        builder.Append(string.Format(" "+ "{0} between " + str + "start{1}  AND " + str + "end_{2}", paramName, str4, str4) + " " + str2);
                        list.Add(DbFactory.CreateDbParameter(str + string.Format("start{0}", str4), dateTime));
                        list.Add(DbFactory.CreateDbParameter(str + string.Format("end_{0}", str4), time2));
                        break;

                    case ConditionOperate.Today:
                        dateTime = CommonHelper.GetDateTime(DateTimeHelper.GetToday() + " 00:00");
                        time2 = CommonHelper.GetDateTime(DateTimeHelper.GetToday() + " 23:59");
                        builder.Append(string.Format(" "+ "{0} between " + str + "start{1}  AND " + str + "end_{2}", paramName, str4, str4) + " " + str2);
                        list.Add(DbFactory.CreateDbParameter(str + string.Format("start{0}", str4), dateTime));
                        list.Add(DbFactory.CreateDbParameter(str + string.Format("end_{0}", str4), time2));
                        break;

                    case ConditionOperate.Tomorrow:
                        dateTime = CommonHelper.GetDateTime(DateTime.Now.AddDays(1.0).ToString("yyyy-MM-dd") + " 00:00");
                        time2 = CommonHelper.GetDateTime(DateTime.Now.AddDays(1.0).ToString("yyyy-MM-dd") + " 23:59");
                        builder.Append(string.Format(" "+ "{0} between " + str + "start{1}  AND " + str + "end_{2}", paramName, str4, str4) + " " + str2);
                        list.Add(DbFactory.CreateDbParameter(str + string.Format("start{0}", str4), dateTime));
                        list.Add(DbFactory.CreateDbParameter(str + string.Format("end_{0}", str4), time2));
                        break;

                    case ConditionOperate.LastWeek:
                        dateTime = CommonHelper.GetDateTime(DateTime.Now.AddDays((double)(Convert.ToInt32((int)(1 - Convert.ToInt32(DateTime.Now.DayOfWeek))) - 7)).ToString("yyyy-MM-dd") + " 00:00");
                        time2 = CommonHelper.GetDateTime(dateTime.AddDays(6.0).ToString("yyyy-MM-dd") + " 23:59");
                        builder.Append(string.Format(" "+ "{0} between " + str + "start{1}  AND " + str + "end_{2}", paramName, str4, str4) + " " + str2);
                        list.Add(DbFactory.CreateDbParameter(str + string.Format("start{0}", str4), dateTime));
                        list.Add(DbFactory.CreateDbParameter(str + string.Format("end_{0}", str4), time2));
                        break;

                    case ConditionOperate.ThisWeek:
                        dateTime = CommonHelper.GetDateTime(DateTime.Now.AddDays((double)(1 - Convert.ToInt32(DateTime.Now.DayOfWeek))).ToString("yyyy-MM-dd") + " 00:00");
                        time2 = CommonHelper.GetDateTime(dateTime.AddDays(6.0).ToString("yyyy-MM-dd") + " 23:59");
                        builder.Append(string.Format(" "+ "{0} between " + str + "start{1}  AND " + str + "end_{2}", paramName, str4, str4) + " " + str2);
                        list.Add(DbFactory.CreateDbParameter(str + string.Format("start{0}", str4), dateTime));
                        list.Add(DbFactory.CreateDbParameter(str + string.Format("end_{0}", str4), time2));
                        break;

                    case ConditionOperate.NextWeek:
                        dateTime = CommonHelper.GetDateTime(DateTime.Now.AddDays((double)(Convert.ToInt32((int)(1 - Convert.ToInt32(DateTime.Now.DayOfWeek))) + 7)).ToString("yyyy-MM-dd") + " 00:00");
                        time2 = CommonHelper.GetDateTime(dateTime.AddDays(6.0).ToString("yyyy-MM-dd") + " 23:59");
                        builder.Append(string.Format(" "+ "{0} between " + str + "start{1}  AND " + str + "end_{2}", paramName, str4, str4) + " " + str2);
                        list.Add(DbFactory.CreateDbParameter(str + string.Format("start{0}", str4), dateTime));
                        list.Add(DbFactory.CreateDbParameter(str + string.Format("end_{0}", str4), time2));
                        break;

                    case ConditionOperate.LastMonth:
                        dateTime = CommonHelper.GetDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-01") + " 00:00");
                        time2 = CommonHelper.GetDateTime(DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddDays(-1.0).ToString("yyyy-MM-dd") + " 23:59");
                        builder.Append(string.Format(" "+ "{0} between " + str + "start{1}  AND " + str + "end_{2}", paramName, str4, str4) + " " + str2);
                        list.Add(DbFactory.CreateDbParameter(str + string.Format("start{0}", str4), dateTime));
                        list.Add(DbFactory.CreateDbParameter(str + string.Format("end_{0}", str4), time2));
                        break;

                    case ConditionOperate.ThisMonth:
                        dateTime = CommonHelper.GetDateTime(DateTime.Now.ToString("yyyy-MM-01") + " 00:00");
                        time2 = CommonHelper.GetDateTime(DateTime.Now.AddMonths(1).AddDays(-1.0).ToString("yyyy-MM-dd") + " 23:59");
                        builder.Append(string.Format(" "+ "{0} between " + str + "start{1}  AND " + str + "end_{2}", paramName, str4, str4) + " " + str2);
                        list.Add(DbFactory.CreateDbParameter(str + string.Format("start{0}", str4), dateTime));
                        list.Add(DbFactory.CreateDbParameter(str + string.Format("end_{0}", str4), time2));
                        break;

                    case ConditionOperate.NextMonth:
                        dateTime = CommonHelper.GetDateTime(DateTime.Now.AddMonths(1).ToString("yyyy-MM-01") + " 00:00");
                        time2 = CommonHelper.GetDateTime(DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(2).AddDays(-1.0).ToString("yyyy-MM-dd") + " 23:59");
                        builder.Append(string.Format(" "+ "{0} between " + str + "start{1}  AND " + str + "end_{2}", paramName, str4, str4) + " " + str2);
                        list.Add(DbFactory.CreateDbParameter(str + string.Format("start{0}", str4), dateTime));
                        list.Add(DbFactory.CreateDbParameter(str + string.Format("end_{0}", str4), time2));
                        break;

                    case ConditionOperate.BeforeDay:
                        dateTime = CommonHelper.GetDateTime(DateTime.Now.AddDays(double.Parse("-" + condition.ParamValue.ToString())) + " 00:00");
                        time2 = CommonHelper.GetDateTime(DateTimeHelper.GetToday() + " 23:59");
                        builder.Append(string.Format(" "+ "{0} between " + str + "start{1}  AND " + str + "end_{2}", paramName, str4, str4) + " " + str2);
                        list.Add(DbFactory.CreateDbParameter(str + string.Format("start{0}", str4), dateTime));
                        list.Add(DbFactory.CreateDbParameter(str + string.Format("end_{0}", str4), time2));
                        break;

                    case ConditionOperate.AfterDay:
                        dateTime = CommonHelper.GetDateTime(DateTime.Now.AddDays(double.Parse(condition.ParamValue.ToString())) + " 00:00");
                        time2 = CommonHelper.GetDateTime(DateTimeHelper.GetToday() + " 23:59");
                        builder.Append(string.Format(" "+ "{0} between " + str + "start{1}  AND " + str + "end_{2}", paramName, str4, str4) + " " + str2);
                        list.Add(DbFactory.CreateDbParameter(str + string.Format("start{0}", str4), dateTime));
                        list.Add(DbFactory.CreateDbParameter(str + string.Format("end_{0}", str4), time2));
                        break;
                }
                num++;
            }
            if (!string.IsNullOrEmpty(orderField))
            {
                orderType = (orderType.ToLower() == "desc") ? "desc" : "asc";
                builder.Append(" Order By " + orderField + " " + orderType);
            }
            parameter = list;
            return builder.ToString();
        }

        public static string GetWhereSql2(IList conditions, bool IsAddWhere = false, string orderField = "", string orderType = "")
        {
            StringBuilder querySql = new StringBuilder();
            if (IsAddWhere)
            {
                querySql.Append(" where 1=1 ");
            }
            foreach (Condition con in conditions)
            {
                switch (con.Operation)
                {
                    case ConditionOperate.Like:
                        querySql.AppendFormat(" and " + con.ParamName + " like" + "'%" + con.ParamValue + "%' ");
                        break;
                    case ConditionOperate.Equal:
                        querySql.AppendFormat(" and " + con.ParamName + " = '" + con.ParamValue + "' ");
                        break;
                    case ConditionOperate.AfterDay:
                        querySql.AppendFormat(" and " + con.ParamName + " >= '" + con.ParamValue + "' ");
                        break;
                    case ConditionOperate.BeforeDay:
                        querySql.AppendFormat(" and " + con.ParamName + " <= '" + con.ParamValue + "' ");
                        break;
                    default:
                        break;
                }
            }
            if (!string.IsNullOrEmpty(orderField))
            {
                orderType = (orderType.ToLower() == "desc") ? "desc" : "asc";
                querySql.Append(" Order By " + orderField + " " + orderType);
            }
            return querySql.ToString();
        }

        public static string GetSelectSql(List<Column> columns)
        {
            StringBuilder querySql = new StringBuilder();
            querySql.Append(" select ");
            querySql.Append(string.Join<Column>(",", columns.ToArray()));
            querySql.Append(" from ");
            return querySql.ToString();
        }
    }
}
