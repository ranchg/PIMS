using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SSI.DataAccess
{
    public class DbHelper
    {
        public static LogHelper log = LogFactory.GetLogger(typeof(DbHelper));

        public DbHelper(string connstring)
        {
            string str = ConfigurationManager.AppSettings["ConStringDESEncrypt"];
            ConnectionString = ConfigurationManager.ConnectionStrings[connstring].ConnectionString;
            if (str == "true")
            {
                ConnectionString = DESEncrypt.Decrypt(ConnectionString);
            }
            this.DatabaseTypeEnumParse(ConfigurationManager.ConnectionStrings[connstring].ProviderName);
            DbParmChar = DbFactory.CreateDbParmCharacter();
        }

        public void DatabaseTypeEnumParse(string value)
        {
            try
            {
                string str = value;
                if (str != null)
                {
                    if (!(str == "System.Data.SqlClient"))
                    {
                        if (str == "System.Data.OracleClient")
                        {
                            goto Label_0053;
                        }
                        if (str == "MySql.Data.MySqlClient")
                        {
                            goto Label_005C;
                        }
                        if (str == "System.Data.OleDb")
                        {
                            goto Label_0065;
                        }
                        if (str == "System.Data.SQLite")
                        {
                            goto Label_006E;
                        }
                    }
                    else
                    {
                        DbType = DatabaseType.SqlServer;
                    }
                }
                return;
            Label_0053:
                DbType = DatabaseType.Oracle;
                return;
            Label_005C:
                DbType = DatabaseType.MySql;
                return;
            Label_0065:
                DbType = DatabaseType.Access;
                return;
            Label_006E:
                DbType = DatabaseType.SQLite;
            }
            catch
            {
                throw new Exception("数据库类型\"" + value + "\"错误，请检查！");
            }
        }

        public static int ExecuteNonQuery(CommandType cmdType, string cmdText)
        {
            int num2;
            try
            {
                DbCommand cmd = DbFactory.CreateDbCommand();
                using (DbConnection connection = DbFactory.CreateDbConnection(ConnectionString))
                {
                    PrepareCommand(cmd, connection, null, cmdType, cmdText, null);
                    int num = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    num2 = num;
                }
            }
            catch (Exception exception)
            {
                log.Error(exception.Message);
                throw;
            }
            return num2;
        }

        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            int num2;
            try
            {
                DbCommand cmd = DbFactory.CreateDbCommand();
                
                using (DbConnection connection = DbFactory.CreateDbConnection(ConnectionString))
                {
                    PrepareCommand(cmd, connection, null, cmdType, cmdText, parameters);

                    int num = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    num2 = num;
                }
            }
            catch (Exception exception)
            {
                log.Error(exception.Message);
                throw;
            }
            return num2;
        }

        public static int ExecuteNonQuery(DbConnection connection, CommandType cmdType, string cmdText)
        {
            int num2;
            try
            {
                DbCommand cmd = DbFactory.CreateDbCommand();
                PrepareCommand(cmd, connection, null, cmdType, cmdText, null);
                int num = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                num2 = num;
            }
            catch (Exception exception)
            {
                log.Error(exception.Message);
                throw;
            }
            return num2;
        }

        public static int ExecuteNonQuery(DbTransaction isOpenTrans, CommandType cmdType, string cmdText)
        {
            int num2;
            try
            {
                DbCommand cmd = DbFactory.CreateDbCommand();
                PrepareCommand(cmd, isOpenTrans.Connection, isOpenTrans, cmdType, cmdText, null);
                int num = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                num2 = num;
            }
            catch (Exception exception)
            {
                log.Error(exception.Message);
                throw;
            }
            return num2;
        }

        public static int ExecuteNonQuery(DbConnection connection, CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            int num2;
            try
            {
                DbCommand cmd = DbFactory.CreateDbCommand();
                PrepareCommand(cmd, connection, null, cmdType, cmdText, parameters);
                int num = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                num2 = num;
            }
            catch (Exception exception)
            {
                log.Error(exception.Message);
                throw;
            }
            return num2;
        }

        public static int ExecuteNonQuery(DbTransaction isOpenTrans, CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            int num2;
            try
            {
                int num = 0;
                DbCommand cmd = DbFactory.CreateDbCommand();
                if ((isOpenTrans == null) || (isOpenTrans.Connection == null))
                {
                    using (DbConnection connection = DbFactory.CreateDbConnection(ConnectionString))
                    {
                        PrepareCommand(cmd, connection, isOpenTrans, cmdType, cmdText, parameters);
                        num = cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    PrepareCommand(cmd, isOpenTrans.Connection, isOpenTrans, cmdType, cmdText, parameters);
                    num = cmd.ExecuteNonQuery();
                }
                cmd.Parameters.Clear();
                num2 = num;
            }
            catch (Exception exception)
            {
                log.Error(exception.Message);
                throw;
            }
            return num2;
        }

        public static IDataReader ExecuteReader(CommandType cmdType, string cmdText)
        {
            IDataReader reader2;
            DbCommand cmd = DbFactory.CreateDbCommand();
            DbConnection conn = DbFactory.CreateDbConnection(ConnectionString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, null);
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                reader2 = reader;
            }
            catch (Exception exception)
            {
                conn.Close();
                cmd.Dispose();
                log.Error(exception.Message);
                throw;
            }
            return reader2;
        }

        public static IDataReader ExecuteReader(CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            IDataReader reader2;
            DbCommand cmd = DbFactory.CreateDbCommand();
            DbConnection conn = DbFactory.CreateDbConnection(ConnectionString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, parameters);
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                reader2 = reader;
            }
            catch (Exception exception)
            {
                conn.Close();
                cmd.Dispose();
                log.Error(exception.Message);
                throw;
            }
            return reader2;
        }

        public static IDataReader ExecuteReader(DbTransaction isOpenTrans, CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            IDataReader reader2;
            DbCommand cmd = DbFactory.CreateDbCommand();
            DbConnection conn = DbFactory.CreateDbConnection(ConnectionString);
            try
            {
                PrepareCommand(cmd, conn, isOpenTrans, cmdType, cmdText, parameters);
                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                reader2 = reader;
            }
            catch (Exception exception)
            {
                conn.Close();
                cmd.Dispose();
                log.Error(exception.Message);
                throw;
            }
            return reader2;
        }

        public static object ExecuteScalar(CommandType cmdType, string cmdText)
        {
            object obj3;
            try
            {
                DbCommand cmd = DbFactory.CreateDbCommand();
                using (DbConnection connection = DbFactory.CreateDbConnection(ConnectionString))
                {
                    PrepareCommand(cmd, connection, null, cmdType, cmdText, null);
                    object obj2 = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    obj3 = obj2;
                }
            }
            catch (Exception exception)
            {
                log.Error(exception.Message);
                throw;
            }
            return obj3;
        }

        public static object ExecuteScalar(CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            object obj3;
            try
            {
                DbCommand cmd = DbFactory.CreateDbCommand();
                using (DbConnection connection = DbFactory.CreateDbConnection(ConnectionString))
                {
                    PrepareCommand(cmd, connection, null, cmdType, cmdText, parameters);
                    object obj2 = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    obj3 = obj2;
                }
            }
            catch (Exception exception)
            {
                log.Error(exception.Message);
                throw;
            }
            return obj3;
        }

        public static object ExecuteScalar(DbConnection connection, CommandType cmdType, string cmdText)
        {
            object obj3;
            try
            {
                DbCommand cmd = DbFactory.CreateDbCommand();
                PrepareCommand(cmd, connection, null, cmdType, cmdText, null);
                object obj2 = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                obj3 = obj2;
            }
            catch (Exception exception)
            {
                log.Error(exception.Message);
                throw;
            }
            return obj3;
        }

        public static object ExecuteScalar(DbConnection connection, CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            object obj3;
            try
            {
                DbCommand cmd = DbFactory.CreateDbCommand();
                PrepareCommand(cmd, connection, null, cmdType, cmdText, parameters);
                object obj2 = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                obj3 = obj2;
            }
            catch (Exception exception)
            {
                log.Error(exception.Message);
                throw;
            }
            return obj3;
        }

        public static object ExecuteScalar(DbConnection conn, DbTransaction isOpenTrans, CommandType cmdType, string cmdText)
        {
            object obj3;
            try
            {
                DbCommand cmd = DbFactory.CreateDbCommand();
                PrepareCommand(cmd, conn, isOpenTrans, cmdType, cmdText, null);
                object obj2 = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                obj3 = obj2;
            }
            catch (Exception exception)
            {
                log.Error(exception.Message);
                throw;
            }
            return obj3;
        }

        public static object ExecuteScalar(DbTransaction isOpenTrans, CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            object obj3;
            try
            {
                DbCommand cmd = DbFactory.CreateDbCommand();
                PrepareCommand(cmd, isOpenTrans.Connection, isOpenTrans, cmdType, cmdText, parameters);
                object obj2 = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                obj3 = obj2;
            }
            catch (Exception exception)
            {
                log.Error(exception.Message);
                throw;
            }
            return obj3;
        }

        public static DataSet GetDataSet(CommandType cmdType, string cmdText)
        {
            DataSet set2;
            DataSet dataSet = new DataSet();
            DbCommand cmd = DbFactory.CreateDbCommand();
            DbConnection conn = DbFactory.CreateDbConnection(ConnectionString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, null);
                DbFactory.CreateDataAdapter(cmd).Fill(dataSet);
                set2 = dataSet;
            }
            catch (Exception exception)
            {
                conn.Close();
                cmd.Dispose();
                log.Error(exception.Message);
                throw;
            }
            return set2;
        }

        public static DataSet GetDataSet(CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            DataSet set2;
            DataSet dataSet = new DataSet();
            DbCommand cmd = DbFactory.CreateDbCommand();
            DbConnection conn = DbFactory.CreateDbConnection(ConnectionString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, parameters);
                DbFactory.CreateDataAdapter(cmd).Fill(dataSet);
                set2 = dataSet;
            }
            catch (Exception exception)
            {
                conn.Close();
                cmd.Dispose();
                log.Error(exception.Message);
                throw;
            }
            return set2;
        }

        private static void PrepareCommand(DbCommand cmd, DbConnection conn, DbTransaction isOpenTrans, CommandType cmdType, string cmdText, DbParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (isOpenTrans != null)
            {
                cmd.Transaction = isOpenTrans;
            }
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                cmd.Parameters.AddRange(cmdParms);
            }
        }

        public static string ConnectionString
        {
            get;
            set;
        }

        public static string DbParmChar
        {
            get;
            set;
        }

        public static DatabaseType DbType
        {
            get;
            set;
        }
    }
}
