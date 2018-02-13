using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ares.Cimes.IntelliService.Info;

namespace Ares.Cimes.IntelliService.Lite.Core
{
    public static class ReflectionHelper
    {
        public static string GetColumnName(PropertyInfo property)
        {
            string columnName = property.Name;

            object[] propertyAttrs = property.GetCustomAttributes(false);
            for (int i = 0; i < propertyAttrs.Length; i++)
            {
                object attribute = propertyAttrs[i];

                if (attribute is ColumnAttribute)
                {
                    ColumnAttribute columnAttr = attribute as ColumnAttribute;
                    if (!string.IsNullOrEmpty(columnAttr.Name))
                    {
                        columnName = columnAttr.Name;
                        break;
                    }
                }
            }

            return columnName;
        }

        public static List<T> ToDataList<T>(this ICollection infoList) //where U : InfoBase, new()
        {
            List<T> result = new List<T>();
            foreach (InfoBase info in infoList)
            {
                result.Add(info.GetInternalDataRow().ToData<T>());
            }
            return result;
        }

        public static T ToData<T>(this InfoBase info)
        {
            if (info == null)
                return default(T);

            var clone = info.GetInternalDataRow().Table.Clone();
            clone.Rows.Add(info.GetInternalDataRow().ItemArray);
            return clone.ToDataList<T>().FirstOrDefault();
        }

        public static T ToData<T>(this DataRow row)
        {
            var columnNames = row.Table.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();

            var properties = typeof(T).GetProperties();


            var objT = Activator.CreateInstance<T>();

            foreach (var property in properties)
            {
                var columnName = GetColumnName(property);
                if ((columnNames.Contains(columnName) || columnNames.Contains(columnName.ToUpper())) && row[columnName] != DBNull.Value)
                {
                    if (property.PropertyType == typeof(decimal))
                    {
                        property.SetValue(objT, Convert.ToDecimal(row[columnName]));
                    }
                    else if (property.PropertyType == typeof(bool))
                    {
                        property.SetValue(objT, row[columnName].ToBool());
                    }
                    else
                    {
                        property.SetValue(objT, row[columnName]);
                    }
                }
            }

            return objT;

        }


        public static List<T> ToDataList<T>(this DataTable table)
        {
            if (table == null || table.Rows.Count == 0)
                return new List<T>();

            var columnNames = table.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();

            var properties = typeof(T).GetProperties();

            return table.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();

                foreach (var property in properties)
                {
                    var columnName = GetColumnName(property);
                    if ((columnNames.Contains(columnName) || columnNames.Contains(columnName.ToUpper())) && row[columnName] != DBNull.Value)
                    {
                        if (property.PropertyType == typeof(decimal))
                        {
                            property.SetValue(objT, Convert.ToDecimal(row[columnName]));
                        }
                        else if (property.PropertyType == typeof(bool))
                        {
                            property.SetValue(objT, row[columnName].ToBool());
                        }
                        else
                        {
                            if (row[columnName] is DBNull )
                                property.SetValue(objT, string.Empty);
                            else
                                property.SetValue(objT, row[columnName]);
                        }
                    }
                }

                return objT;
            }).ToList();

        }

        public static List<Dictionary<string, string>> ToDictList(this DataTable table)
        {
            return table.AsEnumerable().Select(
                // ...then iterate through the columns...
                    row => table.Columns.Cast<DataColumn>().ToDictionary(
                        // ...and find the key value pairs for the dictionary
                    column => column.ColumnName,    // Key
                    column => (column.DataType == typeof(decimal) || column.DataType == typeof(double) ? row[column].ToCimesDecimal(0).Format() : row[column] as string) // Value
                    )).ToList();
        }
    }
}
