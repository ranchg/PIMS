using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using LeaRun.Entity;

namespace LeaRun.Repository
{
    public class RepositoryMY<T>:IRepository<T> where T : new ()
    {

        MySqlConnection mySql = new MySqlConnection("Server=127.0.0.1;Database=learunframework_base;Uid=root;Pwd=123456;");
        public System.Data.Common.DbTransaction BeginTrans()
        {
            return mySql.BeginTransaction();
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public bool BulkInsert(System.Data.DataTable datatable)
        {
            throw new NotImplementedException();
        }

        public int ExecuteBySql(StringBuilder strSql)
        {
            throw new NotImplementedException();
        }

        public int ExecuteBySql(StringBuilder strSql, System.Data.Common.DbTransaction isOpenTrans)
        {
            throw new NotImplementedException();
        }

        public int ExecuteBySql(StringBuilder strSql, System.Data.Common.DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public int ExecuteBySql(StringBuilder strSql, System.Data.Common.DbParameter[] parameters, System.Data.Common.DbTransaction isOpenTrans)
        {
            throw new NotImplementedException();
        }

        public int ExecuteByProc(string procName)
        {
            throw new NotImplementedException();
        }

        public int ExecuteByProc(string procName, System.Data.Common.DbTransaction isOpenTrans)
        {
            throw new NotImplementedException();
        }

        public int ExecuteByProc(string procName, System.Data.Common.DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public int ExecuteByProc(string procName, System.Data.Common.DbParameter[] parameters, System.Data.Common.DbTransaction isOpenTrans)
        {
            throw new NotImplementedException();
        }

        public int Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public int Insert(T entity, System.Data.Common.DbTransaction isOpenTrans)
        {
            throw new NotImplementedException();
        }

        public int Insert(List<T> entity)
        {
            throw new NotImplementedException();
        }

        public int Insert(List<T> entity, System.Data.Common.DbTransaction isOpenTrans)
        {
            throw new NotImplementedException();
        }

        public int Update(T entity)
        {
            throw new NotImplementedException();
        }

        public int Update(T entity, System.Data.Common.DbTransaction isOpenTrans)
        {
            throw new NotImplementedException();
        }

        public int Update(string propertyName, string propertyValue)
        {
            throw new NotImplementedException();
        }

        public int Update(string propertyName, string propertyValue, System.Data.Common.DbTransaction isOpenTrans)
        {
            throw new NotImplementedException();
        }

        public int Update(List<T> entity)
        {
            throw new NotImplementedException();
        }

        public int Update(List<T> entity, System.Data.Common.DbTransaction isOpenTrans)
        {
            throw new NotImplementedException();
        }

        public int Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(T entity, System.Data.Common.DbTransaction isOpenTrans)
        {
            throw new NotImplementedException();
        }

        public int Delete(object propertyValue)
        {
            throw new NotImplementedException();
        }

        public int Delete(object propertyValue, System.Data.Common.DbTransaction isOpenTrans)
        {
            throw new NotImplementedException();
        }

        public int Delete(string propertyName, string propertyValue)
        {
            throw new NotImplementedException();
        }

        public int Delete(string propertyName, string propertyValue, System.Data.Common.DbTransaction isOpenTrans)
        {
            throw new NotImplementedException();
        }

        public int Delete(string tableName, System.Collections.Hashtable ht)
        {
            throw new NotImplementedException();
        }

        public int Delete(string tableName, System.Collections.Hashtable ht, System.Data.Common.DbTransaction isOpenTrans)
        {
            throw new NotImplementedException();
        }

        public int Delete(object[] propertyValue)
        {
            throw new NotImplementedException();
        }

        public int Delete(object[] propertyValue, System.Data.Common.DbTransaction isOpenTrans)
        {
            throw new NotImplementedException();
        }

        public int Delete(string propertyName, object[] propertyValue)
        {
            throw new NotImplementedException();
        }

        public int Delete(string propertyName, object[] propertyValue, System.Data.Common.DbTransaction isOpenTrans)
        {
            throw new NotImplementedException();
        }

        public List<T> FindListTop(int Top)
        {
            throw new NotImplementedException();
        }

        public List<T> FindListTop(int Top, string propertyName, string propertyValue)
        {
            throw new NotImplementedException();
        }

        public List<T> FindListTop(int Top, string WhereSql)
        {
            throw new NotImplementedException();
        }

        public List<T> FindListTop(int Top, string WhereSql, System.Data.Common.DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public List<T> FindList()
        {
            throw new NotImplementedException();
        }

        public List<T> FindList(string propertyName, string propertyValue)
        {
            throw new NotImplementedException();
        }

        public List<T> FindList(string WhereSql)
        {
            return null;
        }

        public List<T> FindList(string WhereSql, System.Data.Common.DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public List<T> FindListBySql(string strSql)
        {
            throw new NotImplementedException();
        }

        public List<T> FindListBySql(string strSql, System.Data.Common.DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public List<T> FindListPage(ref Utilities.JqGridParam jqgridparam)
        {
            throw new NotImplementedException();
        }

        public List<T> FindListPage(string WhereSql, ref Utilities.JqGridParam jqgridparam)
        {
            throw new NotImplementedException();
        }

        public List<T> FindListPage(string WhereSql, System.Data.Common.DbParameter[] parameters, ref Utilities.JqGridParam jqgridparam)
        {
            throw new NotImplementedException();
        }

        public List<T> FindListPageBySql(string strSql, ref Utilities.JqGridParam jqgridparam)
        {
            throw new NotImplementedException();
        }

        public List<T> FindListPageBySql(string strSql, System.Data.Common.DbParameter[] parameters, ref Utilities.JqGridParam jqgridparam)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable FindTableTop(int Top)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable FindTableTop(int Top, string WhereSql)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable FindTableTop(int Top, string WhereSql, System.Data.Common.DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable FindTable()
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable FindTable(string WhereSql)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable FindTable(string WhereSql, System.Data.Common.DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable FindTableBySql(string strSql)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable FindTableBySql(string strSql, System.Data.Common.DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable FindTablePage(ref Utilities.JqGridParam jqgridparam)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable FindTablePage(string WhereSql, ref Utilities.JqGridParam jqgridparam)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable FindTablePage(string WhereSql, System.Data.Common.DbParameter[] parameters, ref Utilities.JqGridParam jqgridparam)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable FindTablePageBySql(string strSql, ref Utilities.JqGridParam jqgridparam)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable FindTablePageBySql(string strSql, System.Data.Common.DbParameter[] parameters, ref Utilities.JqGridParam jqgridparam)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable FindTableByProc(string procName)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable FindTableByProc(string procName, System.Data.Common.DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataSet FindDataSetBySql(string strSql)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataSet FindDataSetBySql(string strSql, System.Data.Common.DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataSet FindDataSetByProc(string procName)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataSet FindDataSetByProc(string procName, System.Data.Common.DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public T FindEntity(object propertyValue)
        {
            throw new NotImplementedException();
        }

        public T FindEntity(string propertyName, object propertyValue)
        {
            throw new NotImplementedException();
        }

        public T FindEntityByWhere(string WhereSql)
        {
            throw new NotImplementedException();
        }

        public T FindEntityByWhere(string WhereSql, System.Data.Common.DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public T FindEntityBySql(string strSql)
        {
            throw new NotImplementedException();
        }

        public T FindEntityBySql(string strSql, System.Data.Common.DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public int FindCount()
        {
            throw new NotImplementedException();
        }

        public int FindCount(string propertyName, string propertyValue)
        {
            throw new NotImplementedException();
        }

        public int FindCount(string WhereSql)
        {
            throw new NotImplementedException();
        }

        public int FindCount(string WhereSql, System.Data.Common.DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public int FindCountBySql(string strSql)
        {
            throw new NotImplementedException();
        }

        public int FindCountBySql(string strSql, System.Data.Common.DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public object FindMax(string propertyName)
        {
            throw new NotImplementedException();
        }

        public object FindMax(string propertyName, string WhereSql)
        {
            throw new NotImplementedException();
        }

        public object FindMax(string propertyName, string WhereSql, System.Data.Common.DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public object FindMaxBySql(string strSql)
        {
            throw new NotImplementedException();
        }

        public object FindMaxBySql(string strSql, System.Data.Common.DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
