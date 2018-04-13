using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SSI.Utilities
{
    public class DataCahceHelper<TEntity> where TEntity : new()
    {
        // 字典保存类属性需要校验的属性（内存常驻）
        private static Dictionary<string, Dictionary<string, CheckType>> EntityPropertyCheck = new Dictionary<string, Dictionary<string, CheckType>>();

        // 字典保存类属性和对应的中文描述（内存常驻）
        private static Dictionary<string, Dictionary<string, string>> EntityPropertyCN = new Dictionary<string, Dictionary<string, string>>();

        // 字典保存报表导出的查询主语句（内存常驻）
        private static Dictionary<string, string> GridQuerySql = new Dictionary<string, string>();
     
        #region 属性校验
        /// <summary>
        ///  返回一个类的属性和中文对应描述
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, CheckType> GetPropertyCheck()
        {
            Type type = typeof(TEntity);
            // 如果内存中已经存在
            if (EntityPropertyCheck.ContainsKey(type.Name))
            {
                return EntityPropertyCheck[type.Name];
            }
            else
            {
                TEntity entity = (TEntity)Activator.CreateInstance(type);
                Dictionary<string, CheckType> dic = GetPropertyWithCheck(entity);
                if (!EntityPropertyCheck.ContainsKey(type.Name) && dic != null)
                {
                    EntityPropertyCheck.Add(type.Name, dic);
                    return dic;
                }
                return null;
            }
        }

        /// <summary>
        ///  获取一个类的属性和中文对应描述
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static Dictionary<string, CheckType> GetPropertyWithCheck(TEntity entity)
        {
            Type type = typeof(TEntity);
            Dictionary<string, CheckType> dicResult = new Dictionary<string, CheckType>();
            foreach (PropertyInfo propInfo in type.GetProperties())
            {
                foreach (Attribute attribute2 in propInfo.GetCustomAttributes(true))
                {
                    PropertyCheckAttribute attribute = attribute2 as PropertyCheckAttribute;
                    if (attribute != null)
                    {
                        dicResult.Add(propInfo.Name, new CheckType(attribute.CheckType,attribute.UField));  // 属性名--->中文描述 (Excel导出使用)
                    }
                }
            }
            return dicResult;
        }

        #endregion

        #region 属性中文描述
        /// <summary>
        ///  返回一个类的属性和中文对应描述
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetPropertyCN()
        {
            Type type = typeof(TEntity);
            // 如果内存中已经存在
            if (EntityPropertyCN.ContainsKey(type.Name))
            {
                return EntityPropertyCN[type.Name];
            }
            else
            {
                TEntity entity = (TEntity)Activator.CreateInstance(type);
                Dictionary<string, string> dic = GetPropertyWithCN(entity);
                if (!EntityPropertyCN.ContainsKey(type.Name) && dic != null)
                {
                    EntityPropertyCN.Add(type.Name, dic);
                    return dic;
                }
                return null;
            }
        }

        /// <summary>
        ///  获取一个类的属性和中文对应描述
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static Dictionary<string, string> GetPropertyWithCN(TEntity entity)
        {
            Type type = typeof(TEntity);
            Dictionary<string, string> dicResult = new Dictionary<string, string>();
            foreach (PropertyInfo propInfo in type.GetProperties())
            {
                foreach (Attribute attribute2 in propInfo.GetCustomAttributes(true))
                {
                    PropertyCNAttribute attribute = attribute2 as PropertyCNAttribute;
                    if (attribute != null)
                    {
                        dicResult.Add(propInfo.Name.ToUpper(), attribute.Name);  // 属性名--->中文描述 (Excel导出使用)
                        dicResult.Add(attribute.Name, propInfo.Name);  // 中文描述--->属性名 (Excel导入使用)
                    }
                }
            }
            return dicResult;
        }
        #endregion

        #region 报表查询主语句
        /// <summary>
        ///  添加查询主语句到字典中
        /// </summary>
        /// <param name="sql"></param>
        public static void AddGridSql(string sql)
        {
            Type type = typeof(TEntity);
            if (!GridQuerySql.Keys.Contains(type.Name))
            {
                GridQuerySql.Add(type.Name, sql);
            }
        }
        /// <summary>
        ///  返回查询主语句
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetGridSql(string name)
        {
            Type type = typeof(TEntity);
            if (GridQuerySql.Keys.Contains(name))
            {
                return GridQuerySql[name];
            }
            return null;
        }
        #endregion

    }
}
