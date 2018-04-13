using SSI.DataAccess.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SSI.DataAccess
{
    public class DatabaseCommon
    {
        public static StringBuilder DeleteSql<T>(T entity)
        {
            Type type = entity.GetType();
            PropertyInfo[] properties = type.GetProperties();
            StringBuilder builder = new StringBuilder("Delete From " + type.Name + " Where 1=1");
            foreach (PropertyInfo info in properties)
            {
                if (info.GetCustomAttributes(typeof(PropertyIgnoreAttribute), false).Length > 0) continue;
                if (info.GetValue(entity, null) != null)
                {
                    builder.Append(" AND " + info.Name + " = " + DbHelper.DbParmChar + info.Name);
                }
            }
            return builder;
        }

        public static StringBuilder DeleteSql(string tableName, Hashtable ht)
        {
            StringBuilder builder = new StringBuilder("Delete From " + tableName + " Where 1=1");
            foreach (string str in ht.Keys)
            {
                builder.Append(" AND " + str + " = " + DbHelper.DbParmChar + str);
            }
            return builder;
        }

        public static StringBuilder DeleteSql(string tableName, string pkName)
        {
            return new StringBuilder("Delete From " + tableName + " Where " + pkName + " = " + DbHelper.DbParmChar + pkName);
        }

        public static string GetClassName<T>()
        {
            Type type = typeof(T);
            IEnumerable<DisplayNameAttribute> source = type.GetCustomAttributes(true).OfType<DisplayNameAttribute>();
            DisplayNameAttribute[] attributeArray = (source as DisplayNameAttribute[]) ?? source.ToArray<DisplayNameAttribute>();
            if (attributeArray.Any<DisplayNameAttribute>())
            {
                return attributeArray.ToList<DisplayNameAttribute>()[0].DisplayName;
            }
            return type.Name;
        }

        public static string GetFieldText(PropertyInfo pi)
        {
            object[] customAttributes = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true);
            if (customAttributes.Any<object>())
            {
                DisplayNameAttribute attribute = customAttributes[0] as DisplayNameAttribute;
                return attribute.DisplayName;
            }
            return pi.Name;
        }

        public static object GetKeyField<T>()
        {
            Type type = typeof(T);
            string str = "";
            string name = type.Name;
            foreach (Attribute attribute2 in type.GetCustomAttributes(true))
            {
                PrimaryKeyAttribute attribute = attribute2 as PrimaryKeyAttribute;
                if (attribute != null)
                {
                    str = attribute.Name;
                }
            }
            return str;
        }

        public static string GetKeyField(string className)
        {
            Type type = Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "SSI.Entity.dll")).GetType(className);
            string str = "";
            string name = type.Name;
            foreach (Attribute attribute2 in type.GetCustomAttributes(true))
            {
                PrimaryKeyAttribute attribute = attribute2 as PrimaryKeyAttribute;
                if (attribute != null)
                {
                    str = attribute.Name;
                }
            }
            return str;
        }

        public static object GetKeyFieldValue<T>(T entity)
        {
            Type type = typeof(T);
            string name = "";
            string str2 = type.Name;
            foreach (Attribute attribute2 in type.GetCustomAttributes(true))
            {
                PrimaryKeyAttribute attribute = attribute2 as PrimaryKeyAttribute;
                if (attribute != null)
                {
                    name = attribute.Name;
                }
            }
            return type.GetProperty(name).GetValue(entity, null).ToString();
        }

        public static DbParameter[] GetParameter(Hashtable ht)
        {
            IList<DbParameter> source = new List<DbParameter>();
            DbType ansiString = DbType.AnsiString;
            foreach (string str in ht.Keys)
            {
                if (ht[str] is DateTime)
                {
                    ansiString = DbType.DateTime;
                }
                else
                {
                    ansiString = DbType.String;
                }
                source.Add(DbFactory.CreateDbParameter(DbHelper.DbParmChar + str, ht[str], ansiString));
            }
            return source.ToArray<DbParameter>();
        }

        public static DbParameter[] GetParameter<T>(T entity)
        {
            IList<DbParameter> source = new List<DbParameter>();
            DbType ansiString = DbType.AnsiString;
            PropertyInfo[] properties = entity.GetType().GetProperties();
            foreach (PropertyInfo info in properties)
            {
                if (info.GetCustomAttributes(typeof(PropertyIgnoreAttribute), false).Length > 0) continue;
                if (info.GetValue(entity, null) == null)
                {
                    continue;
                }
                object objVal = info.GetValue(entity, null);
                string str = info.PropertyType.ToString();
                if (str == null)
                {
                    goto Label_0095;
                }
                if (!(str == "System.Nullable`1[System.Int32]"))
                {
                    if (str == "System.Nullable`1[System.Decimal]")
                    {
                        goto Label_008D;
                    }
                    if (str == "System.Nullable`1[System.DateTime]")
                    {
                        goto Label_0091;
                    }
                    goto Label_0095;
                }
                ansiString = DbType.Int32;
                goto Label_009A;
                Label_008D:
                ansiString = DbType.Decimal;
                goto Label_009A;
                Label_0091:
                ansiString = DbType.DateTime;
                goto Label_009A;
                Label_0095:
                ansiString = DbType.String;
                Label_009A:
                source.Add(DbFactory.CreateDbParameter(DbHelper.DbParmChar + info.Name, objVal.ToString() == "&nbsp;" ? "" : objVal, ansiString));
            }
            return source.ToArray<DbParameter>();
        }

        public static PropertyInfo[] GetProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        public static StringBuilder InsertSql<T>(T entity)
        {
            Type type = entity.GetType();
            StringBuilder builder = new StringBuilder();
            builder.Append(" Insert Into ");
            builder.Append(type.Name);
            builder.Append("(");
            StringBuilder builder2 = new StringBuilder();
            StringBuilder builder3 = new StringBuilder();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo info in properties)
            {
                if (info.GetCustomAttributes(typeof(PropertyIgnoreAttribute), false).Length > 0) continue;
                if (info.GetValue(entity, null) != null)
                {
                    builder3.Append("," + info.Name);
                    builder2.Append("," + DbHelper.DbParmChar + info.Name);
                }
            }
            builder.Append(builder3.ToString().Substring(1, builder3.ToString().Length - 1) + ") Values (");
            builder.Append(builder2.ToString().Substring(1, builder2.ToString().Length - 1) + ")");
            return builder;
        }

        public static StringBuilder InsertSql(string tableName, Hashtable ht)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" Insert Into ");
            builder.Append(tableName);
            builder.Append("(");
            StringBuilder builder2 = new StringBuilder();
            StringBuilder builder3 = new StringBuilder();
            foreach (string str in ht.Keys)
            {
                if (ht[str] != null)
                {
                    builder3.Append("," + str);
                    builder2.Append("," + DbHelper.DbParmChar + str);
                }
            }
            builder.Append(builder3.ToString().Substring(1, builder3.ToString().Length - 1) + ") Values (");
            builder.Append(builder2.ToString().Substring(1, builder2.ToString().Length - 1) + ")");
            return builder;
        }

        public static StringBuilder SelectCountSql<T>() where T : new()
        {
            string name = typeof(T).Name;
            return new StringBuilder("SELECT Count(1) FROM " + name + " WHERE 1=1 ");
        }

        public static StringBuilder SelectCountSql<T>(Dictionary<string, string[]> dicStr) where T : new()
        {
            string name = typeof(T).Name;
            StringBuilder builderPK = new StringBuilder();
            foreach (string str in dicStr.Keys)
            {
                string[] strS = dicStr[str];
                builderPK.Append(" join " + strS[0] + " on " + name + "." + str + " = " + strS[0] + "." + str);
            }

            return new StringBuilder("SELECT Count(1) FROM " + name + builderPK.ToString() + " WHERE 1=1 ");
        }

        public static StringBuilder SelectMaxSql<T>(string propertyName) where T : new()
        {
            string name = typeof(T).Name;
            return new StringBuilder("SELECT MAX(" + propertyName + ") FROM " + name + "  WHERE 1=1 ");
        }

        public static StringBuilder SelectSql<T>() where T : new()
        {
            string name = typeof(T).Name;
            PropertyInfo[] properties = GetProperties(new T().GetType());
            StringBuilder builder = new StringBuilder();
            foreach (PropertyInfo info in properties)
            {
                if (info.GetCustomAttributes(typeof(PropertyIgnoreAttribute), false).Length > 0) continue;
                builder.Append(info.Name + ",");
            }
            if (builder.Length > 0)
            {
                builder.Remove(builder.ToString().Length - 1, 1);
            }
            string format = "SELECT {0} FROM {1} WHERE 1=1 ";
            return new StringBuilder(string.Format(format, builder.ToString(), name + " "));
        }

        public static StringBuilder SelectSql<T>(List<string> queryField) where T : new()
        {
            string name = typeof(T).Name;
            StringBuilder builder = new StringBuilder();
            foreach (string str in queryField)
            {
                builder.Append(name + "." + str + ",");
            }
            if (builder.Length > 0)
            {
                builder.Remove(builder.ToString().Length - 1, 1);
            }
            string format = "SELECT {0} FROM {1}  ";
            return new StringBuilder(string.Format(format, builder.ToString(), name));
        }

        public static StringBuilder SelectSql<T>(Dictionary<string, string[]> dicStr) where T : new()
        {
            string name = typeof(T).Name;
            PropertyInfo[] properties = GetProperties(new T().GetType());
            StringBuilder builder = new StringBuilder();
            foreach (PropertyInfo info in properties)
            {
                if (dicStr.Keys.Contains(info.Name)) continue;
                if (info.GetCustomAttributes(typeof(PropertyIgnoreAttribute), false).Length > 0) continue;
                builder.Append(name + "." + info.Name + ",");
            }
            foreach (string str in dicStr.Keys)
            {
                string[] strS = dicStr[str];
                builder.Append(strS[0] + "." + strS[1] + " as " + str + ",");
            }
            if (builder.Length > 0)
            {
                builder.Remove(builder.ToString().Length - 1, 1);
            }
            string format = "SELECT {0} FROM {1}  ";
            return new StringBuilder(string.Format(format, builder.ToString(), name));
        }

        public static StringBuilder SelectSql<T>(int Top) where T : new()
        {
            string name = typeof(T).Name;
            PropertyInfo[] properties = GetProperties(new T().GetType());
            StringBuilder builder = new StringBuilder();
            foreach (PropertyInfo info in properties)
            {
                if (info.GetCustomAttributes(typeof(PropertyIgnoreAttribute), false).Length > 0) continue;
                builder.Append(info.Name + ",");
            }
            if (builder.Length > 0)
            {
                builder.Remove(builder.ToString().Length - 1, 1);
            }
            string format = "SELECT top {0} {1} FROM {2} WHERE 1=1 ";
            return new StringBuilder(string.Format(format, Top, builder.ToString(), name + " "));
        }

        public static StringBuilder SelectSql(string tableName)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT * FROM " + tableName + " WHERE 1=1 ");
            return builder;
        }

        public static StringBuilder SelectSql(string tableName, int Top)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Concat(new object[] { "SELECT top ", Top, " * FROM ", tableName, " WHERE 1=1 " }));
            return builder;
        }

        public static StringBuilder UpdateSql<T>(T entity)
        {
            string str = GetKeyField<T>().ToString();
            Type type = entity.GetType();
            PropertyInfo[] properties = type.GetProperties();
            StringBuilder builder = new StringBuilder();
            builder.Append("Update ");
            builder.Append(type.Name);
            builder.Append(" Set ");
            bool flag = true;
            foreach (PropertyInfo info in properties)
            {
                if (info.GetCustomAttributes(typeof(PropertyIgnoreAttribute), false).Length > 0) continue;
                if ((info.GetValue(entity, null) != null) && (str != info.Name))
                {
                    if (flag)
                    {
                        flag = false;
                        builder.Append(info.Name);
                        builder.Append("=");
                        builder.Append(DbHelper.DbParmChar + info.Name);
                    }
                    else
                    {
                        builder.Append("," + info.Name);
                        builder.Append("=");
                        builder.Append(DbHelper.DbParmChar + info.Name);
                    }
                }
            }
            builder.Append(" Where ").Append(str).Append("=").Append(DbHelper.DbParmChar + str);
            return builder;
        }

        public static StringBuilder UpdateSql<T>(T entity, string pkName)
        {
            Type type = entity.GetType();
            PropertyInfo[] properties = type.GetProperties();
            StringBuilder builder = new StringBuilder();
            builder.Append(" Update ");
            builder.Append(type.Name);
            builder.Append(" Set ");
            bool flag = true;
            foreach (PropertyInfo info in properties)
            {
                if (info.GetCustomAttributes(typeof(PropertyIgnoreAttribute), false).Length > 0) continue;
                if ((info.GetValue(entity, null) != null) && (GetKeyField<T>().ToString() != info.Name))
                {
                    if (flag)
                    {
                        flag = false;
                        builder.Append(info.Name);
                        builder.Append("=");
                        builder.Append(DbHelper.DbParmChar + info.Name);
                    }
                    else
                    {
                        builder.Append("," + info.Name);
                        builder.Append("=");
                        builder.Append(DbHelper.DbParmChar + info.Name);
                    }
                }
            }
            builder.Append(" Where ").Append(pkName).Append("=").Append(DbHelper.DbParmChar + pkName);
            return builder;
        }

        public static StringBuilder UpdateSql(string tableName, Hashtable ht, string pkName)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" Update ");
            builder.Append(tableName);
            builder.Append(" Set ");
            bool flag = true;
            foreach (string str in ht.Keys)
            {
                if ((ht[str] != null) && (pkName != str))
                {
                    if (flag)
                    {
                        flag = false;
                        builder.Append(str);
                        builder.Append("=");
                        builder.Append(DbHelper.DbParmChar + str);
                    }
                    else
                    {
                        builder.Append("," + str);
                        builder.Append("=");
                        builder.Append(DbHelper.DbParmChar + str);
                    }
                }
            }
            builder.Append(" Where ").Append(pkName).Append("=").Append(DbHelper.DbParmChar + pkName);
            return builder;
        }
    }
}
