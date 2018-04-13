using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace SSI.DataAccess
{
    public class MySqlHelper
    {
        public static List<T> GetPageList<T>(string sql, string orderField, string orderType, int pageIndex, int pageSize, ref int count)
        {
            return GetPageList<T>(sql, null, orderField, orderType, pageIndex, pageSize, ref count);
        }

        public static List<T> GetPageList<T>(string sql, DbParameter[] param, string orderField, string orderType, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder builder = new StringBuilder();
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            int num = (pageIndex - 1) * pageSize;
            int num2 = pageIndex * pageSize;
            string str = "";
            if (!string.IsNullOrEmpty(orderField))
            {
                str = "Order By " + orderField + " " + orderType;
            }
            builder.Append(string.Concat(new object[] { " select  * From (", sql + str, ") As T  LIMIT " + num + "," + num2 }));
            count = Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, "Select Count(1) From (" + sql + ") As t", param));
            return DatabaseReader.ReaderToList<T>(DbHelper.ExecuteReader(CommandType.Text, builder.ToString(), param));
        }

        public static DataTable GetPageTable(string sql, string orderField, string orderType, int pageIndex, int pageSize, ref int count)
        {
            return GetPageTable(sql, null, orderField, orderType, pageIndex, pageSize, ref count);
        }

        public static DataTable GetPageTable(string sql, DbParameter[] param, string orderField, string orderType, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder builder = new StringBuilder();
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            int num = (pageIndex - 1) * pageSize;
            int num2 = pageSize;
            string str = "";
            if (!string.IsNullOrEmpty(orderField))
            {
                str = " Order By " + orderField + " " + orderType;
            }
            else
            {
                str = " order by (select 0)";
            }
            builder.Append(string.Concat(new object[] { " select  * From (", sql + str, ") As T  LIMIT " + num + "," + num2 }));
            count = Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, "Select Count(1) From (" + sql + ") As t", param));
            return DatabaseReader.ReaderToDataTable(DbHelper.ExecuteReader(CommandType.Text, builder.ToString(), param));
        }
    }
}
