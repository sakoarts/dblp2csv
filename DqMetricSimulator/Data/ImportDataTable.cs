using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Autofac;
using DqMetricSimulator.Core;

namespace DqMetricSimulator.Data
{
    /// <summary>
    /// This class provides tools to import data from Ado.net DataTable
    /// </summary>
    public static class ImportDataTable
    {
        /// <summary>
        /// Import data table. Performance is not a big consideration.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static ITable ImportAdoNetDataTable(DataTable dataTable)
        {
            //1.Create the ITable based on the dataTable schema
            //2.Load each column in an IColumn
            //3.Rebuild the table, lookup every column value
            var table = ExtractSchema(dataTable);
        }

        private static ITable ExtractSchema(DataTable dataTable)
        {
            //Get keys
            var keys = dataTable.PrimaryKey.Select(k => dataTable.Columns.IndexOf(k.ColumnName)).ToList();
            var table = TableFactory.CreateTable(dataTable.Columns.Count, keys);
            table.Columns.Select( (c, i) =>
                                      {
                                          if (dataTable.Columns[i].DataType == typeof (Int32))
                                              c = new Column<Int32>();
                                          if (dataTable.Columns[i].DataType == typeof (DateTime))
                                              c = new Column<DateTime>();
                                          if (dataTable.Columns[i].DataType == typeof (Double))
                                              c = new Column<Double>();
                                          if (dataTable.Columns[i].DataType == typeof (String))
                                              c = new Column<String>();
                                          return c;
                                      }
                                      ).ToList();
            return table;
        }

        
    }
}
