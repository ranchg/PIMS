using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace SSI.DataAccess
{
    public class Database : IDatabase, IDisposable
    {
        public Database(string connstring)
        {
            DbHelper helper = new DbHelper(connstring);
        }

        public DbTransaction BeginTrans()
        {
            if (!this.inTransaction)
            {
                this.dbConnection = DbFactory.CreateDbConnection(DbHelper.ConnectionString);
                if (this.dbConnection.State == ConnectionState.Closed)
                {
                    this.dbConnection.Open();
                }
                this.inTransaction = true;
                this.isOpenTrans = this.dbConnection.BeginTransaction();
            }
            return this.isOpenTrans;
        }

        public bool BulkInsert(DataTable datatable)
        {
            return false;
        }

        public void Close()
        {
            if (this.dbConnection != null)
            {
                this.dbConnection.Close();
                this.dbConnection.Dispose();
            }
            this.dbConnection = null;
            this.isOpenTrans = null;
        }

        public void Commit()
        {
            if (this.inTransaction)
            {
                this.inTransaction = false;
                this.isOpenTrans.Commit();
            }
        }

        public int Delete<T>(T entity)
        {
            StringBuilder builder = DatabaseCommon.DeleteSql<T>(entity);
            DbParameter[] parameter = DatabaseCommon.GetParameter<T>(entity);
            return DbHelper.ExecuteNonQuery(CommandType.Text, builder.ToString(), parameter);
        }

        public int Delete<T>(object propertyValue)
        {
            string name = typeof(T).Name;
            string pkName = DatabaseCommon.GetKeyField<T>().ToString();
            StringBuilder builder = DatabaseCommon.DeleteSql(name, pkName);
            IList<DbParameter> source = new List<DbParameter> {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + pkName, propertyValue)
            };
            return DbHelper.ExecuteNonQuery(CommandType.Text, builder.ToString(), source.ToArray<DbParameter>());
        }

        public int Delete<T>(object[] propertyValue)
        {
            int num3;
            string name = typeof(T).Name;
            string str2 = DatabaseCommon.GetKeyField<T>().ToString();
            StringBuilder builder = new StringBuilder("DELETE FROM " + name + " WHERE " + str2 + " IN (");
            try
            {
                IList<DbParameter> source = new List<DbParameter>();
                int index = 0;
                string str3 = DbHelper.DbParmChar + "ID" + index;
                for (int i = 0; i < (propertyValue.Length - 1); i++)
                {
                    object obj2 = propertyValue[i];
                    str3 = DbHelper.DbParmChar + "ID" + index;
                    builder.Append(str3).Append(",");
                    source.Add(DbFactory.CreateDbParameter(str3, obj2));
                    index++;
                }
                str3 = DbHelper.DbParmChar + "ID" + index;
                builder.Append(str3);
                source.Add(DbFactory.CreateDbParameter(str3, propertyValue[index]));
                builder.Append(")");
                num3 = DbHelper.ExecuteNonQuery(CommandType.Text, builder.ToString(), source.ToArray<DbParameter>());
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return num3;
        }

        public int Delete<T>(object propertyValue, DbTransaction isOpenTrans)
        {
            string name = typeof(T).Name;
            string pkName = DatabaseCommon.GetKeyField<T>().ToString();
            StringBuilder builder = DatabaseCommon.DeleteSql(name, pkName);
            IList<DbParameter> source = new List<DbParameter> {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + pkName, propertyValue)
            };
            return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, builder.ToString(), source.ToArray<DbParameter>());
        }

        public int Delete(string tableName, Hashtable ht)
        {
            StringBuilder builder = DatabaseCommon.DeleteSql(tableName, ht);
            DbParameter[] parameter = DatabaseCommon.GetParameter(ht);
            return DbHelper.ExecuteNonQuery(CommandType.Text, builder.ToString(), parameter);
        }

        public int Delete<T>(T entity, DbTransaction isOpenTrans)
        {
            StringBuilder builder = DatabaseCommon.DeleteSql<T>(entity);
            DbParameter[] parameter = DatabaseCommon.GetParameter<T>(entity);
            return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, builder.ToString(), parameter.ToArray<DbParameter>());
        }

        public int Delete<T>(object[] propertyValue, DbTransaction isOpenTrans)
        {
            int num3;
            string name = typeof(T).Name;
            string str2 = DatabaseCommon.GetKeyField<T>().ToString();
            StringBuilder builder = new StringBuilder("DELETE FROM " + name + " WHERE " + DbHelper.DbParmChar + str2 + " IN (");
            try
            {
                IList<DbParameter> source = new List<DbParameter>();
                int index = 0;
                string str3 = DbHelper.DbParmChar + "ID" + index;
                for (int i = 0; i < (propertyValue.Length - 1); i++)
                {
                    object obj2 = propertyValue[i];
                    str3 = DbHelper.DbParmChar + "ID" + index;
                    builder.Append(str3).Append(",");
                    source.Add(DbFactory.CreateDbParameter(str3, obj2));
                    index++;
                }
                str3 = DbHelper.DbParmChar + "ID" + index;
                builder.Append(str3);
                source.Add(DbFactory.CreateDbParameter(str3, propertyValue[index]));
                builder.Append(")");
                num3 = DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, builder.ToString(), source.ToArray<DbParameter>());
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return num3;
        }

        public int Delete<T>(string propertyName, string propertyValue)
        {
            StringBuilder builder = DatabaseCommon.DeleteSql(typeof(T).Name, propertyName);
            IList<DbParameter> source = new List<DbParameter> {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue)
            };
            return DbHelper.ExecuteNonQuery(CommandType.Text, builder.ToString(), source.ToArray<DbParameter>());
        }

        public int Delete<T>(string propertyName, object[] propertyValue)
        {
            int num3;
            string name = typeof(T).Name;
            string str2 = propertyName;
            StringBuilder builder = new StringBuilder("DELETE FROM " + name + " WHERE " + DbHelper.DbParmChar + str2 + " IN (");
            try
            {
                IList<DbParameter> source = new List<DbParameter>();
                int index = 0;
                string str3 = DbHelper.DbParmChar + "ID" + index;
                for (int i = 0; i < (propertyValue.Length - 1); i++)
                {
                    object obj2 = propertyValue[i];
                    str3 = DbHelper.DbParmChar + "ID" + index;
                    builder.Append(str3).Append(",");
                    source.Add(DbFactory.CreateDbParameter(str3, obj2));
                    index++;
                }
                str3 = DbHelper.DbParmChar + "ID" + index;
                builder.Append(str3);
                source.Add(DbFactory.CreateDbParameter(str3, propertyValue[index]));
                builder.Append(")");
                num3 = DbHelper.ExecuteNonQuery(CommandType.Text, builder.ToString(), source.ToArray<DbParameter>());
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return num3;
        }

        public int Delete(string tableName, Hashtable ht, DbTransaction isOpenTrans)
        {
            StringBuilder builder = DatabaseCommon.DeleteSql(tableName, ht);
            DbParameter[] parameter = DatabaseCommon.GetParameter(ht);
            return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, builder.ToString(), parameter);
        }

        public int Delete(string tableName, string propertyName, object[] propertyValue)
        {
            int num3;
            string str = propertyName;
            StringBuilder builder = new StringBuilder("DELETE FROM " + tableName + " WHERE " + DbHelper.DbParmChar + str + " IN (");
            try
            {
                IList<DbParameter> source = new List<DbParameter>();
                int index = 0;
                string str2 = DbHelper.DbParmChar + "ID" + index;
                for (int i = 0; i < (propertyValue.Length - 1); i++)
                {
                    object obj2 = propertyValue[i];
                    str2 = DbHelper.DbParmChar + "ID" + index;
                    builder.Append(str2).Append(",");
                    source.Add(DbFactory.CreateDbParameter(str2, obj2));
                    index++;
                }
                str2 = DbHelper.DbParmChar + "ID" + index;
                builder.Append(str2);
                source.Add(DbFactory.CreateDbParameter(str2, propertyValue[index]));
                builder.Append(")");
                num3 = DbHelper.ExecuteNonQuery(CommandType.Text, builder.ToString(), source.ToArray<DbParameter>());
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return num3;
        }

        public int Delete<T>(string propertyName, string propertyValue, DbTransaction isOpenTrans)
        {
            StringBuilder builder = DatabaseCommon.DeleteSql(typeof(T).Name, propertyName);
            IList<DbParameter> source = new List<DbParameter> {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue)
            };
            return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, builder.ToString(), source.ToArray<DbParameter>());
        }

        public int Delete<T>(string propertyName, object[] propertyValue, DbTransaction isOpenTrans)
        {
            int num3;
            string name = typeof(T).Name;
            string str2 = propertyName;
            StringBuilder builder = new StringBuilder("DELETE FROM " + name + " WHERE " + DbHelper.DbParmChar + str2 + " IN (");
            try
            {
                IList<DbParameter> source = new List<DbParameter>();
                int index = 0;
                string str3 = DbHelper.DbParmChar + "ID" + index;
                for (int i = 0; i < (propertyValue.Length - 1); i++)
                {
                    object obj2 = propertyValue[i];
                    str3 = DbHelper.DbParmChar + "ID" + index;
                    builder.Append(str3).Append(",");
                    source.Add(DbFactory.CreateDbParameter(str3, obj2));
                    index++;
                }
                str3 = DbHelper.DbParmChar + "ID" + index;
                builder.Append(str3);
                source.Add(DbFactory.CreateDbParameter(str3, propertyValue[index]));
                builder.Append(")");
                num3 = DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, builder.ToString(), source.ToArray<DbParameter>());
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return num3;
        }

        public int Delete(string tableName, string propertyName, string propertyValue)
        {
            StringBuilder builder = DatabaseCommon.DeleteSql(tableName, propertyName);
            IList<DbParameter> source = new List<DbParameter> {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue)
            };
            return DbHelper.ExecuteNonQuery(CommandType.Text, builder.ToString(), source.ToArray<DbParameter>());
        }

        public int Delete(string tableName, string propertyName, string propertyValue, DbTransaction isOpenTrans)
        {
            StringBuilder builder = DatabaseCommon.DeleteSql(tableName, propertyName);
            IList<DbParameter> source = new List<DbParameter> {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue)
            };
            return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, builder.ToString(), source.ToArray<DbParameter>());
        }

        public int Delete(string tableName, string propertyName, object[] propertyValue, DbTransaction isOpenTrans)
        {
            int num3;
            string str = propertyName;
            StringBuilder builder = new StringBuilder("DELETE FROM " + tableName + " WHERE " + DbHelper.DbParmChar + str + " IN (");
            try
            {
                IList<DbParameter> source = new List<DbParameter>();
                int index = 0;
                string str2 = DbHelper.DbParmChar + "ID" + index;
                for (int i = 0; i < (propertyValue.Length - 1); i++)
                {
                    object obj2 = propertyValue[i];
                    str2 = DbHelper.DbParmChar + "ID" + index;
                    builder.Append(str2).Append(",");
                    source.Add(DbFactory.CreateDbParameter(str2, obj2));
                    index++;
                }
                str2 = DbHelper.DbParmChar + "ID" + index;
                builder.Append(str2);
                source.Add(DbFactory.CreateDbParameter(str2, propertyValue[index]));
                builder.Append(")");
                num3 = DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, builder.ToString(), source.ToArray<DbParameter>());
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return num3;
        }

        public void Dispose()
        {
            if (this.dbConnection != null)
            {
                this.dbConnection.Dispose();
            }
            if (this.isOpenTrans != null)
            {
                this.isOpenTrans.Dispose();
            }
        }

        public int ExecuteByProc(string procName)
        {
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, procName);
        }

        public int ExecuteByProc(string procName, DbTransaction isOpenTrans)
        {
            return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.StoredProcedure, procName);
        }

        public int ExecuteByProc(string procName, DbParameter[] parameters)
        {
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, procName, parameters);
        }

        public int ExecuteByProc(string procName, DbParameter[] parameters, DbTransaction isOpenTrans)
        {
            return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.StoredProcedure, procName, parameters);
        }

        public int ExecuteBySql(StringBuilder strSql)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString());
        }

        public int ExecuteBySql(StringBuilder strSql, DbTransaction isOpenTrans)
        {
            return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString());
        }

        public int ExecuteBySql(StringBuilder strSql, DbParameter[] parameters)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
        }

        public int ExecuteBySql(StringBuilder strSql, DbParameter[] parameters, DbTransaction isOpenTrans)
        {
            return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameters);
        }

        public int FindCount<T>() where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectCountSql<T>();
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, builder.ToString()));
        }

        public int FindCount<T>(string WhereSql) where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectCountSql<T>();
            builder.Append(WhereSql);
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, builder.ToString()));
        }

        public int FindCount<T>(string propertyName, string propertyValue) where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectCountSql<T>();
            builder.Append(" AND " + propertyName + " = " + DbHelper.DbParmChar + propertyName);
            IList<DbParameter> source = new List<DbParameter> {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue)
            };
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, builder.ToString(), source.ToArray<DbParameter>()));
        }

        public int FindCount<T>(string WhereSql, DbParameter[] parameters) where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectCountSql<T>();
            builder.Append(WhereSql);
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, builder.ToString(), parameters));
        }

        public int FindCountBySql(string strSql)
        {
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, strSql));
        }

        public int FindCountBySql(string strSql, DbParameter[] parameters)
        {
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, strSql, parameters));
        }

        public DataSet FindDataSetByProc(string procName)
        {
            return DbHelper.GetDataSet(CommandType.StoredProcedure, procName);
        }

        public DataSet FindDataSetByProc(string procName, DbParameter[] parameters)
        {
            return DbHelper.GetDataSet(CommandType.StoredProcedure, procName, parameters);
        }

        public DataSet FindDataSetBySql(string strSql)
        {
            return DbHelper.GetDataSet(CommandType.Text, strSql);
        }

        public DataSet FindDataSetBySql(string strSql, DbParameter[] parameters)
        {
            return DbHelper.GetDataSet(CommandType.Text, strSql, parameters);
        }

        public T FindEntity<T>(object propertyValue) where T : new()
        {
            string str = DatabaseCommon.GetKeyField<T>().ToString();
            StringBuilder builder = DatabaseCommon.SelectSql<T>();
            builder.Append(" AND ").Append(str).Append("=").Append(DbHelper.DbParmChar + str);
            IList<DbParameter> source = new List<DbParameter> {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + str, propertyValue)
            };
            return DatabaseReader.ReaderToModel<T>(DbHelper.ExecuteReader(CommandType.Text, builder.ToString(), source.ToArray<DbParameter>()));
        }

        public T FindEntity<T>(string propertyName, object propertyValue) where T : new()
        {
            string str = propertyName;
            StringBuilder builder = DatabaseCommon.SelectSql<T>();
            builder.Append(" AND ").Append(str).Append("=").Append(DbHelper.DbParmChar + str);
            IList<DbParameter> source = new List<DbParameter> {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + str, propertyValue)
            };
            return DatabaseReader.ReaderToModel<T>(DbHelper.ExecuteReader(CommandType.Text, builder.ToString(), source.ToArray<DbParameter>()));
        }

        public T FindEntityBySql<T>(string strSql)
        {
            return DatabaseReader.ReaderToModel<T>(DbHelper.ExecuteReader(CommandType.Text, strSql.ToString()));
        }

        public T FindEntityBySql<T>(string strSql, DbParameter[] parameters)
        {
            return DatabaseReader.ReaderToModel<T>(DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameters));
        }

        public T FindEntityByWhere<T>(string WhereSql) where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectSql<T>(1);
            builder.Append(WhereSql);
            return DatabaseReader.ReaderToModel<T>(DbHelper.ExecuteReader(CommandType.Text, builder.ToString()));
        }

        public T FindEntityByWhere<T>(string WhereSql, DbParameter[] parameters) where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectSql<T>(1);
            builder.Append(WhereSql);
            return DatabaseReader.ReaderToModel<T>(DbHelper.ExecuteReader(CommandType.Text, builder.ToString(), parameters));
        }

        public Hashtable FindHashtable(string tableName, StringBuilder WhereSql)
        {
            StringBuilder builder = DatabaseCommon.SelectSql(tableName, 1);
            builder.Append(WhereSql);
            return DatabaseReader.ReaderToHashtable(DbHelper.ExecuteReader(CommandType.Text, builder.ToString()));
        }

        public Hashtable FindHashtable(string tableName, string propertyName, object propertyValue)
        {
            StringBuilder builder = DatabaseCommon.SelectSql(tableName, 1);
            builder.Append(" AND ").Append(propertyName).Append("=").Append(DbHelper.DbParmChar + propertyName);
            IList<DbParameter> source = new List<DbParameter> {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue)
            };
            return DatabaseReader.ReaderToHashtable(DbHelper.ExecuteReader(CommandType.Text, builder.ToString(), source.ToArray<DbParameter>()));
        }

        public Hashtable FindHashtable(string tableName, StringBuilder WhereSql, DbParameter[] parameters)
        {
            StringBuilder builder = DatabaseCommon.SelectSql(tableName, 1);
            builder.Append(WhereSql);
            return DatabaseReader.ReaderToHashtable(DbHelper.ExecuteReader(CommandType.Text, builder.ToString(), parameters));
        }

        public Hashtable FindHashtableBySql(string strSql)
        {
            return DatabaseReader.ReaderToHashtable(DbHelper.ExecuteReader(CommandType.Text, strSql.ToString()));
        }

        public Hashtable FindHashtableBySql(string strSql, DbParameter[] parameters)
        {
            return DatabaseReader.ReaderToHashtable(DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameters));
        }

        public List<T> FindList<T>() where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectSql<T>();
            return DatabaseReader.ReaderToList<T>(DbHelper.ExecuteReader(CommandType.Text, builder.ToString()));
        }

        public List<T> FindList<T>(string WhereSql) where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectSql<T>();
            builder.Append(WhereSql);
            return DatabaseReader.ReaderToList<T>(DbHelper.ExecuteReader(CommandType.Text, builder.ToString()));
        }

        public List<T> FindList<T>(string propertyName, string propertyValue) where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectSql<T>();
            builder.Append(" AND " + propertyName + " = " + DbHelper.DbParmChar + propertyName);
            IList<DbParameter> source = new List<DbParameter> {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue)
            };
            return DatabaseReader.ReaderToList<T>(DbHelper.ExecuteReader(CommandType.Text, builder.ToString(), source.ToArray<DbParameter>()));
        }

        public List<T> FindList<T>(string WhereSql, DbParameter[] parameters) where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectSql<T>();
            builder.Append(WhereSql);
            return DatabaseReader.ReaderToList<T>(DbHelper.ExecuteReader(CommandType.Text, builder.ToString(), parameters));
        }

        public List<T> FindListBySql<T>(string strSql)
        {
            return DatabaseReader.ReaderToList<T>(DbHelper.ExecuteReader(CommandType.Text, strSql.ToString()));
        }

        public List<T> FindListBySql<T>(string strSql, DbParameter[] parameters)
        {
            return DatabaseReader.ReaderToList<T>(DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameters));
        }

        public List<T> FindListPage<T>(string orderField, string orderType, int pageIndex, int pageSize, ref int recordCount, out string querySql) where T : new()
        {
            string strSql = DatabaseCommon.SelectSql<T>().ToString();
            querySql = string.IsNullOrEmpty(orderField) == true ? strSql : strSql + " order by " + orderField + " " + orderType;
            return SqlServerHelper.GetPageList<T>(strSql, orderField, orderType, pageIndex, pageSize, ref recordCount);
        }

        public List<T> FindListPage<T>(string WhereSql, string orderField, string orderType, int pageIndex, int pageSize, ref int recordCount) where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectSql<T>();
            builder.Append(WhereSql);
            return SqlServerHelper.GetPageList<T>(builder.ToString(), orderField, orderType, pageIndex, pageSize, ref recordCount);
        }

        public List<T> FindListPage<T>(string WhereSql, DbParameter[] parameters, string orderField, string orderType, int pageIndex, int pageSize, ref int recordCount, out string querySql) where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectSql<T>();
            builder.Append(WhereSql);
            string strSql = builder.ToString();
            querySql = string.IsNullOrEmpty(orderField) == true ? strSql : strSql + " order by " + orderField + " " + orderType;
            return SqlServerHelper.GetPageList<T>(strSql, parameters, orderField, orderType, pageIndex, pageSize, ref recordCount);
        }

        public List<T> FindListPageBySql<T>(string strSql, string orderField, string orderType, int pageIndex, int pageSize, ref int recordCount)
        {
            return SqlServerHelper.GetPageList<T>(strSql, orderField, orderType, pageIndex, pageSize, ref recordCount);
        }

        public List<T> FindListPageBySql<T>(string strSql, DbParameter[] parameters, string orderField, string orderType, int pageIndex, int pageSize, ref int recordCount)
        {
            return SqlServerHelper.GetPageList<T>(strSql, parameters, orderField, orderType, pageIndex, pageSize, ref recordCount);
        }

        public List<T> FindListTop<T>(int Top) where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectSql<T>(Top);
            return DatabaseReader.ReaderToList<T>(DbHelper.ExecuteReader(CommandType.Text, builder.ToString()));
        }

        public List<T> FindListTop<T>(int Top, string WhereSql) where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectSql<T>(Top);
            builder.Append(WhereSql);
            return DatabaseReader.ReaderToList<T>(DbHelper.ExecuteReader(CommandType.Text, builder.ToString()));
        }

        public List<T> FindListTop<T>(int Top, string propertyName, string propertyValue) where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectSql<T>(Top);
            builder.Append(" AND " + propertyName + " = " + DbHelper.DbParmChar + propertyName);
            IList<DbParameter> source = new List<DbParameter> {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue)
            };
            return DatabaseReader.ReaderToList<T>(DbHelper.ExecuteReader(CommandType.Text, builder.ToString(), source.ToArray<DbParameter>()));
        }

        public List<T> FindListTop<T>(int Top, string WhereSql, DbParameter[] parameters) where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectSql<T>(Top);
            builder.Append(WhereSql);
            return DatabaseReader.ReaderToList<T>(DbHelper.ExecuteReader(CommandType.Text, builder.ToString(), parameters));
        }

        public object FindMax<T>(string propertyName) where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectMaxSql<T>(propertyName);
            return DbHelper.ExecuteScalar(CommandType.Text, builder.ToString());
        }

        public object FindMax<T>(string propertyName, string WhereSql) where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectMaxSql<T>(propertyName);
            builder.Append(WhereSql);
            return DbHelper.ExecuteScalar(CommandType.Text, builder.ToString());
        }

        public object FindMax<T>(string propertyName, string WhereSql, DbParameter[] parameters) where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectMaxSql<T>(propertyName);
            builder.Append(WhereSql);
            return DbHelper.ExecuteScalar(CommandType.Text, builder.ToString(), parameters);
        }

        public object FindMaxBySql(string strSql)
        {
            return DbHelper.ExecuteScalar(CommandType.Text, strSql);
        }

        public object FindMaxBySql(string strSql, DbParameter[] parameters)
        {
            return DbHelper.ExecuteScalar(CommandType.Text, strSql, parameters);
        }

        public DataTable FindTable<T>() where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectSql<T>();
            return DatabaseReader.ReaderToDataTable(DbHelper.ExecuteReader(CommandType.Text, builder.ToString()));
        }

        public DataTable FindTable<T>(string WhereSql) where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectSql<T>();
            builder.Append(WhereSql);
            return DatabaseReader.ReaderToDataTable(DbHelper.ExecuteReader(CommandType.Text, builder.ToString()));
        }

        public DataTable FindTable<T>(string WhereSql, DbParameter[] parameters) where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectSql<T>();
            builder.Append(WhereSql);
            return DatabaseReader.ReaderToDataTable(DbHelper.ExecuteReader(CommandType.Text, builder.ToString(), parameters));
        }
        public DataTable FindTable<T>(List<string> queryField) where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectSql<T>(queryField);
            return DatabaseReader.ReaderToDataTable(DbHelper.ExecuteReader(CommandType.Text, builder.ToString()));
        }
        public DataTable FindTable<T>(List<string> queryField, string WhereSql, DbParameter[] parameters) where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectSql<T>(queryField);
            builder.Append(WhereSql);
            return DatabaseReader.ReaderToDataTable(DbHelper.ExecuteReader(CommandType.Text, builder.ToString(), parameters));
        }

        public DataTable FindTableByProc(string procName)
        {
            return DatabaseReader.ReaderToDataTable(DbHelper.ExecuteReader(CommandType.StoredProcedure, procName));
        }

        public DataTable FindTableByProc(string procName, DbParameter[] parameters)
        {
            return DatabaseReader.ReaderToDataTable(DbHelper.ExecuteReader(CommandType.StoredProcedure, procName, parameters));
        }

        public DataTable FindTableBySql(string strSql)
        {
            return DatabaseReader.ReaderToDataTable(DbHelper.ExecuteReader(CommandType.Text, strSql.ToString()));
        }

        public DataTable FindTableBySql(string strSql, DbParameter[] parameters)
        {
            return DatabaseReader.ReaderToDataTable(DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameters));
        }

        public DataTable FindTablePage<T>(string orderField, string orderType, int pageIndex, int pageSize, ref int recordCount) where T : new()
        {
            return SqlServerHelper.GetPageTable(DatabaseCommon.SelectSql<T>().ToString(), orderField, orderType, pageIndex, pageSize, ref recordCount);
        }

        public DataTable FindTablePage<T>(string WhereSql, string orderField, string orderType, int pageIndex, int pageSize, ref int recordCount) where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectSql<T>();
            builder.Append(WhereSql);
            return SqlServerHelper.GetPageTable(builder.ToString(), orderField, orderType, pageIndex, pageSize, ref recordCount);
        }

        public DataTable FindTablePage<T>(string WhereSql, DbParameter[] parameters, string orderField, string orderType, int pageIndex, int pageSize, ref int recordCount) where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectSql<T>();
            builder.Append(WhereSql);
            return SqlServerHelper.GetPageTable(builder.ToString(), parameters, orderField, orderType, pageIndex, pageSize, ref recordCount);
        }

        public DataTable FindTablePageBySql(string strSql, string orderField, string orderType, int pageIndex, int pageSize, ref int recordCount)
        {
            return SqlServerHelper.GetPageTable(strSql, orderField, orderType, pageIndex, pageSize, ref recordCount);
        }

        public DataTable FindTablePageBySql(string strSql, DbParameter[] parameters, string orderField, string orderType, int pageIndex, int pageSize, ref int recordCount)
        {
            return SqlServerHelper.GetPageTable(strSql, parameters, orderField, orderType, pageIndex, pageSize, ref recordCount);
        }

        public DataTable FindTableTop<T>(int Top) where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectSql<T>(Top);
            return DatabaseReader.ReaderToDataTable(DbHelper.ExecuteReader(CommandType.Text, builder.ToString()));
        }

        public DataTable FindTableTop<T>(int Top, string WhereSql) where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectSql<T>(Top);
            builder.Append(WhereSql);
            return DatabaseReader.ReaderToDataTable(DbHelper.ExecuteReader(CommandType.Text, builder.ToString()));
        }

        public DataTable FindTableTop<T>(int Top, string WhereSql, DbParameter[] parameters) where T : new()
        {
            StringBuilder builder = DatabaseCommon.SelectSql<T>(Top);
            builder.Append(WhereSql);
            return DatabaseReader.ReaderToDataTable(DbHelper.ExecuteReader(CommandType.Text, builder.ToString(), parameters));
        }

        public int Insert<T>(T entity)
        {
            object obj2 = 0;
            StringBuilder builder = DatabaseCommon.InsertSql<T>(entity);
            DbParameter[] parameter = DatabaseCommon.GetParameter<T>(entity);
            obj2 = DbHelper.ExecuteNonQuery(CommandType.Text, builder.ToString(), parameter);
            return Convert.ToInt32(obj2);
        }

        public int Insert<T>(List<T> entity)
        {
            object obj2 = 0;
            DbTransaction isOpenTrans = this.BeginTrans();
            try
            {
                foreach (T local in entity)
                {
                    this.Insert<T>(local, isOpenTrans);
                }
                this.Commit();
                obj2 = 1;
            }
            catch (Exception exception)
            {
                this.Rollback();
                this.Close();
                obj2 = -1;
                throw exception;
            }
            return Convert.ToInt32(obj2);
        }

        public int Insert(string tableName, Hashtable ht)
        {
            object obj2 = 0;
            StringBuilder builder = DatabaseCommon.InsertSql(tableName, ht);
            DbParameter[] parameter = DatabaseCommon.GetParameter(ht);
            obj2 = DbHelper.ExecuteNonQuery(CommandType.Text, builder.ToString(), parameter);
            return Convert.ToInt32(obj2);
        }

        public int Insert<T>(T entity, DbTransaction isOpenTrans)
        {
            object obj2 = 0;
            StringBuilder builder = DatabaseCommon.InsertSql<T>(entity);
            DbParameter[] parameter = DatabaseCommon.GetParameter<T>(entity);
            obj2 = DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, builder.ToString(), parameter);
            return Convert.ToInt32(obj2);
        }

        public int Insert<T>(List<T> entity, DbTransaction isOpenTrans)
        {
            object obj2 = 0;
            try
            {
                foreach (T local in entity)
                {
                    this.Insert<T>(local, isOpenTrans);
                }
                obj2 = 1;
            }
            catch (Exception exception)
            {
                obj2 = -1;
                throw exception;
            }
            return Convert.ToInt32(obj2);
        }

        public int Insert(string tableName, Hashtable ht, DbTransaction isOpenTrans)
        {
            object obj2 = 0;
            StringBuilder builder = DatabaseCommon.InsertSql(tableName, ht);
            DbParameter[] parameter = DatabaseCommon.GetParameter(ht);
            obj2 = DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, builder.ToString(), parameter);
            return Convert.ToInt32(obj2);
        }

        public void Rollback()
        {
            if (this.inTransaction)
            {
                this.inTransaction = false;
                this.isOpenTrans.Rollback();
            }
        }

        public int Update<T>(T entity)
        {
            object obj2 = 0;
            StringBuilder builder = DatabaseCommon.UpdateSql<T>(entity);
            DbParameter[] parameter = DatabaseCommon.GetParameter<T>(entity);
            obj2 = DbHelper.ExecuteNonQuery(CommandType.Text, builder.ToString(), parameter);
            if (Convert.ToInt32(obj2) < 1)
            {
                StringBuilder builderTmp = DatabaseCommon.InsertSql<T>(entity);
                obj2 = DbHelper.ExecuteNonQuery(CommandType.Text, builderTmp.ToString(), parameter);
            }
            return Convert.ToInt32(obj2);
        }

        public int Update<T>(List<T> entity)
        {
            object obj2 = 0;
            DbTransaction isOpenTrans = this.BeginTrans();
            try
            {
                foreach (T local in entity)
                {
                    this.Update<T>(local, isOpenTrans);
                }
                this.Commit();
                obj2 = 1;
            }
            catch (Exception exception)
            {
                this.Rollback();
                this.Close();
                obj2 = -1;
                throw exception;
            }
            return Convert.ToInt32(obj2);
        }

        public int Update<T>(T entity, DbTransaction isOpenTrans)
        {
            object obj2 = 0;
            StringBuilder builder = DatabaseCommon.UpdateSql<T>(entity);
            DbParameter[] parameter = DatabaseCommon.GetParameter<T>(entity);
            obj2 = DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, builder.ToString(), parameter);
            return Convert.ToInt32(obj2);
        }

        public int Update<T>(List<T> entity, DbTransaction isOpenTrans)
        {
            object obj2 = 0;
            try
            {
                foreach (T local in entity)
                {
                    this.Update<T>(local, isOpenTrans);
                }
                obj2 = 1;
            }
            catch (Exception exception)
            {
                obj2 = -1;
                throw exception;
            }
            return Convert.ToInt32(obj2);
        }

        public int Update<T>(string propertyName, string propertyValue)
        {
            object obj2 = 0;
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            builder2.Append("Update ");
            builder2.Append(typeof(T).Name);
            builder2.Append(" Set ");
            builder2.Append(propertyName);
            builder2.Append("=");
            builder2.Append(DbHelper.DbParmChar + propertyName);
            IList<DbParameter> source = new List<DbParameter> {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue)
            };
            obj2 = DbHelper.ExecuteNonQuery(CommandType.Text, builder.ToString(), source.ToArray<DbParameter>());
            return Convert.ToInt32(obj2);
        }

        public int Update(string tableName, Hashtable ht, string propertyName)
        {
            object obj2 = 0;
            StringBuilder builder = DatabaseCommon.UpdateSql(tableName, ht, propertyName);
            DbParameter[] parameter = DatabaseCommon.GetParameter(ht);
            obj2 = DbHelper.ExecuteNonQuery(CommandType.Text, builder.ToString(), parameter);
            return Convert.ToInt32(obj2);
        }

        public int Update<T>(string propertyName, string propertyValue, DbTransaction isOpenTrans)
        {
            object obj2 = 0;
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            builder2.Append("Update ");
            builder2.Append(typeof(T).Name);
            builder2.Append(" Set ");
            builder2.Append(propertyName);
            builder2.Append("=");
            builder2.Append(DbHelper.DbParmChar + propertyName);
            IList<DbParameter> source = new List<DbParameter> {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue)
            };
            obj2 = DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, builder.ToString(), source.ToArray<DbParameter>());
            return Convert.ToInt32(obj2);
        }

        public int Update(string tableName, Hashtable ht, string propertyName, DbTransaction isOpenTrans)
        {
            object obj2 = 0;
            StringBuilder builder = DatabaseCommon.UpdateSql(tableName, ht, propertyName);
            DbParameter[] parameter = DatabaseCommon.GetParameter(ht);
            obj2 = DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, builder.ToString(), parameter);
            return Convert.ToInt32(obj2);
        }

        public static string connString
        {
            get;
            set;
        }

        private DbConnection dbConnection { get; set; }

        public bool inTransaction { get; set; }

        private DbTransaction isOpenTrans { get; set; }
    }
}
