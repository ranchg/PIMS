using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SSI.Utilities
{
    public class CollectionHelper
    {
        private CollectionHelper()
        {
        }

        public static DataTable ConvertTo<T>(List<T> list)
        {
            DataTable table = CreateTable<T>();
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (T item in list)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item);
                }

                table.Rows.Add(row);
            }

            return table;
        }

        public static List<T> ConvertTo<T>(List<DataRow> rows)
        {
            List<T> list = null;

            if (rows != null)
            {
                list = new List<T>();

                foreach (DataRow row in rows)
                {
                    bool isAllNull = true;
                    T item = CreateItem<T>(row, ref isAllNull);
                    if (isAllNull) continue;
                    list.Add(item);
                }
            }
            return list;
        }

        public static List<T> ConvertTo<T>(DataTable table)
        {
            if (table == null)
            {
                return null;
            }

            List<DataRow> rows = new List<DataRow>();

            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }

            return ConvertTo<T>(rows);
        }

        public static T CreateItem<T>(DataRow row, ref bool isAllNull)
        {
            T obj = default(T);
            isAllNull = true;
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();
                foreach (DataColumn column in row.Table.Columns)
                {
                    PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName);
                    try
                    {
                        object value = default(T);
                        if(row[column.ColumnName] != DBNull.Value)
                            value = row[column.ColumnName];
                        prop.SetValue(obj, HackType(value,prop.PropertyType), null);
                        object propVal = prop.GetValue(obj, null);
                        if (propVal != null && !CommonHelper.IsEmpty(propVal.ToString())) isAllNull = false;
                    }
                    catch
                    {
                        // You can log something here  
                        throw;
                    }
                }
            }
            return obj;
        }

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

        public static DataTable CreateTable<T>()
        {
            Type entityType = typeof(T);
            DataTable table = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, prop.PropertyType);
            }

            return table;
        }
    }
}
