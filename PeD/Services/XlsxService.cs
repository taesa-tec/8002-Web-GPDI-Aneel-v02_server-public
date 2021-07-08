using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using ClosedXML.Excel;
using PeD.Core.Attributes;

namespace PeD.Services
{
    public class XlsxService
    {
        protected PropertyInfo[] ColumnsFromType(Type t)
        {
            var properties = t.GetProperties().Where(p => p.GetCustomAttribute<XlxsColumnAttribute>() != null)
                .ToArray();
            return properties
                .OrderBy(p => p.GetCustomAttribute<XlxsColumnAttribute>()?.Order ?? properties.Count())
                .ThenBy(p => p.GetCustomAttribute<XlxsColumnAttribute>()?.Name ?? p.Name).ToArray();
        }

        public DataTable DataTableFrom(Type t)
        {
            return DataTableFrom(t, out var c);
        }

        public DataTable DataTableFrom(Type t, out PropertyInfo[] cols)
        {
            var table = new DataTable();
            cols = ColumnsFromType(t);

            foreach (var col in cols)
            {
                var attribute = col.GetCustomAttribute<XlxsColumnAttribute>();
                if (attribute is null)
                    continue;
                var nameCol = attribute?.Name ?? col.Name;
                if (attribute.Type != null)
                {
                    table.Columns.Add(nameCol, attribute.Type);
                }
                else
                {
                    table.Columns.Add(nameCol);
                }
            }

            return table;
        }

        public DataTable DataTableFrom<T>(IEnumerable<T> collection)
        {
            var table = DataTableFrom(typeof(T), out var cols);

            foreach (var item in collection)
            {
                var row = cols.Select(col => col.GetValue(item)).ToArray();
                table.Rows.Add(row);
            }

            return table;
        }

        public XLWorkbook WorkbookFromList<T>(IEnumerable<T> collection, string name)
        {
            return WorkbookFromList(new XLWorkbook(), collection, name);
        }

        public XLWorkbook WorkbookFromList<T>(XLWorkbook workbook, IEnumerable<T> collection, string name, int x = 1,
            int y = 1)
        {
            return WorksheetFromList(workbook.AddWorksheet(name), collection, x, y).Workbook;
        }

        public IXLWorksheet WorksheetFromList<T>(IXLWorksheet worksheet, IEnumerable<T> collection, int x = 1,
            int y = 1)
        {
            var table = DataTableFrom(collection);
            worksheet.Cell(x, y).InsertTable(table);
            return worksheet;
        }
    }
}