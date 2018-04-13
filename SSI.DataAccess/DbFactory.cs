using MySql.Data.MySqlClient;
//using Oracle.DataAccess.Client;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSI.DataAccess
{
    public class DbFactory
    {
        public static IDbDataAdapter CreateDataAdapter()
        {
            switch (DbHelper.DbType)
            {
                case DatabaseType.Oracle:
                    return new OracleDataAdapter();

                case DatabaseType.SqlServer:
                    return new SqlDataAdapter();

                case DatabaseType.Access:
                    return new OleDbDataAdapter();

                case DatabaseType.MySql:
                    return new MySqlDataAdapter();

                //case DatabaseType.SQLite:
                //    return new SQLiteDataAdapter();
            }
            throw new Exception("数据库类型目前不支持！");
        }

        public static IDbDataAdapter CreateDataAdapter(DbCommand cmd)
        {
            switch (DbHelper.DbType)
            {
                case DatabaseType.Oracle:
                    return new OracleDataAdapter((OracleCommand)cmd);

                case DatabaseType.SqlServer:
                    return new SqlDataAdapter((SqlCommand)cmd);

                case DatabaseType.Access:
                    return new OleDbDataAdapter((OleDbCommand)cmd);

                case DatabaseType.MySql:
                    return new MySqlDataAdapter((MySqlCommand)cmd);

                //case DatabaseType.SQLite:
                //    return new SQLiteDataAdapter((SQLiteCommand)cmd);
            }
            throw new Exception("数据库类型目前不支持！");
        }

        public static DbCommand CreateDbCommand()
        {
            switch (DbHelper.DbType)
            {
                case DatabaseType.Oracle:
                    OracleCommand cmd = new OracleCommand();
                    cmd.BindByName = true;
                    return cmd;

                case DatabaseType.SqlServer:
                    return new SqlCommand();

                case DatabaseType.Access:
                    return new OleDbCommand();

                case DatabaseType.MySql:
                    return new MySqlCommand();

                //case DatabaseType.SQLite:
                //    return new SQLiteCommand();
            }
            throw new Exception("数据库类型目前不支持！");
        }

        public static DbConnection CreateDbConnection(string connectionString)
        {
            switch (DbHelper.DbType)
            {
                case DatabaseType.Oracle:
                    return new OracleConnection(connectionString);

                case DatabaseType.SqlServer:
                    return new SqlConnection(connectionString);

                case DatabaseType.Access:
                    return new OleDbConnection(connectionString);

                case DatabaseType.MySql:
                    return new MySqlConnection(connectionString);

                //case DatabaseType.SQLite:
                //    return new SQLiteConnection(connectionString);
            }
            throw new Exception("数据库类型目前不支持！");
        }

        public static DbParameter CreateDbOutParameter(string paramName, int size)
        {
            DbParameter parameter = CreateDbParameter();
            parameter.Direction = ParameterDirection.Output;
            parameter.ParameterName = paramName;
            parameter.Size = size;
            return parameter;
        }

        public static DbParameter CreateDbParameter()
        {
            switch (DbHelper.DbType)
            {
                case DatabaseType.Oracle:
                    return new OracleParameter();

                case DatabaseType.SqlServer:
                    return new SqlParameter();

                case DatabaseType.Access:
                    return new OleDbParameter();

                case DatabaseType.MySql:
                    return new MySqlParameter();

                //case DatabaseType.SQLite:
                //    return new SQLiteParameter();
            }
            throw new Exception("数据库类型目前不支持！");
        }

        public static DbParameter CreateDbParameter(string paramName, object value)
        {
            DbParameter parameter = CreateDbParameter();
            parameter.ParameterName = paramName;
            parameter.Value = value;
            try{
                DateTime dtime = Convert.ToDateTime(value);
                parameter.Value = dtime;
                parameter.DbType = DbType.DateTime;
            }
            catch{
                return parameter;
            }
            return parameter;
        }

        public static DbParameter CreateDbParameter(string paramName, object value, DbType dbType)
        {
            DbParameter parameter = CreateDbParameter();
            parameter.DbType = dbType;
            parameter.ParameterName = paramName;
            parameter.Value = value;
            return parameter;
        }

        public static DbParameter CreateDbParameter(string paramName, object value, int size)
        {
            DbParameter parameter = CreateDbParameter();
            parameter.ParameterName = paramName;
            parameter.Value = value;
            parameter.Size = size;
            return parameter;
        }

        public static DbParameter CreateDbParameter(string paramName, object value, DbType dbType, int size)
        {
            DbParameter parameter = CreateDbParameter();
            parameter.DbType = dbType;
            parameter.ParameterName = paramName;
            parameter.Value = value;
            parameter.Size = size;
            return parameter;
        }

        public static DbParameter[] CreateDbParameters(int size)
        {
            int index = 0;
            DbParameter[] parameterArray = null;
            switch (DbHelper.DbType)
            {
                case DatabaseType.Oracle:
                    parameterArray = new OracleParameter[size];
                    while (index < size)
                    {
                        parameterArray[index] = new OracleParameter();
                        index++;
                    }
                    return parameterArray;

                case DatabaseType.SqlServer:
                    parameterArray = new SqlParameter[size];
                    while (index < size)
                    {
                        parameterArray[index] = new SqlParameter();
                        index++;
                    }
                    return parameterArray;

                case DatabaseType.Access:
                    parameterArray = new OleDbParameter[size];
                    while (index < size)
                    {
                        parameterArray[index] = new OleDbParameter();
                        index++;
                    }
                    return parameterArray;

                case DatabaseType.MySql:
                    parameterArray = new MySqlParameter[size];
                    while (index < size)
                    {
                        parameterArray[index] = new MySqlParameter();
                        index++;
                    }
                    return parameterArray;

                //case DatabaseType.SQLite:
                //    parameterArray = new SQLiteParameter[size];
                //    while (index < size)
                //    {
                //        parameterArray[index] = new SQLiteParameter();
                //        index++;
                //    }
                //    return parameterArray;
            }
            throw new Exception("数据库类型目前不支持！");
        }

        public static string CreateDbParmCharacter()
        {
            switch (DbHelper.DbType)
            {
                case DatabaseType.Oracle:
                    return ":";

                case DatabaseType.SqlServer:
                    return "@";

                case DatabaseType.Access:
                    return "@";

                case DatabaseType.MySql:
                    return "?";

                case DatabaseType.SQLite:
                    return "@";
            }
            throw new Exception("数据库类型目前不支持！");
        }
    }
}
