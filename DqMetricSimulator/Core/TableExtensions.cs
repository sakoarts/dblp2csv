using System;
using System.Linq;

namespace DqMetricSimulator.Core
{
    public static class TableExtensions
    {
        public static object GetValue(this ITable table, int idx, IRow row)
        {
            return table.Columns[idx][row.Rows[idx]];
        }
        public static string GetKeystring(this ITable table, IRow row)
        {
            var keystring = "";
            table.Metadata.GetKeyValuesForRow(row).ToList().ForEach( k => keystring+=String.Format("({0})", k.ToString()));
            return keystring;
        }
    }
}
