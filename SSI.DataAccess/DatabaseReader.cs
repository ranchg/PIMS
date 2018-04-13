using SSI.DataAccess.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SSI.DataAccess
{
    public class DatabaseReader
    {
        public static object HackType(object value, Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }
                NullableConverter converter = new NullableConverter(conversionType);
                conversionType = converter.UnderlyingType;
            }
            return Convert.ChangeType(value, conversionType);
        }

        public static bool IsNullOrDBNull(object obj)
        {
            return ((obj is DBNull) || string.IsNullOrEmpty(obj.ToString()));
        }

        public static DataTable ReaderToDataTable(IDataReader reader)
        {
            DataTable table = new DataTable("Table");
            int fieldCount = reader.FieldCount;
            for (int i = 0; i < fieldCount; i++)
            {
                table.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
            }
            table.BeginLoadData();
            object[] values = new object[fieldCount];
            while (reader.Read())
            {
                reader.GetValues(values);
                table.LoadDataRow(values, true);
            }
            reader.Close();
            table.EndLoadData();
            return table;
        }

        public static Hashtable ReaderToHashtable(IDataReader dr)
        {
            Hashtable hashtable = new Hashtable();
            while (dr.Read())
            {
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    string str = dr.GetName(i);
                    hashtable[str] = dr[str];
                }
            }
            return hashtable;
        }

        public static List<T> ReaderToList<T>(IDataReader dr)
        {
            using (dr)
            {
                List<string> list = new List<string>(dr.FieldCount);
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    list.Add(dr.GetName(i).ToLower());
                }
                List<T> list2 = new List<T>();
                while (dr.Read())
                {
                    T local = Activator.CreateInstance<T>();
                    foreach (PropertyInfo info in local.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
                    {
                        if (list.Contains(info.Name.ToLower()) && !IsNullOrDBNull(dr[info.Name]))
                        {
                            info.SetValue(local, HackType(dr[info.Name], info.PropertyType), null);
                        }
                    }
                    list2.Add(local);
                }
                return list2;
            }
        }

        public static T ReaderToModel<T>(IDataReader dr)
        {
            T local = Activator.CreateInstance<T>();
            while (dr.Read())
            {
                foreach (PropertyInfo info in local.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
                {
                    if (info.GetCustomAttributes(typeof(PropertyIgnoreAttribute), false).Length > 0) continue;
                    if (!IsNullOrDBNull(dr[info.Name]))
                    {
                        info.SetValue(local, HackType(dr[info.Name], info.PropertyType), null);
                    }
                }
            }
            return local;
        }

        public static T ReaderToModelWithForignKey<T>(IDataReader dr)
        {
            T local = Activator.CreateInstance<T>();
            while (dr.Read())
            {
                foreach (PropertyInfo info in local.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
                {
                    if (!IsNullOrDBNull(dr[info.Name]))
                    {
                        info.SetValue(local, HackType(dr[info.Name], info.PropertyType), null);
                    }
                }
            }
            return local;
        }
    }
}
