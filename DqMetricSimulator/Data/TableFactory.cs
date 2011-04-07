using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DqMetricSimulator.Core;

namespace DqMetricSimulator.Data
{
    public static class TableFactory
    {
        public static ITable CreateTable(int columns, IEnumerable<int> keys)
        {
            var meta = new Metadata();
            keys.ToList().ForEach(k => meta.KeyCols.Add(k));
            return new Table
                (
                    meta, new List<IRow>(), new List<IColumn>(columns)
                );
        }

        public static IColumn CreateColumn(DataColumn dc, IEnumerable<object> data)
        {
            if (dc.DataType == typeof (Int32))
                return new Column<Int32>( data.Cast<Int32>() );
            if (dc.DataType == typeof (DateTime))
                return new Column<DateTime>(data.Cast<DateTime>());
            if (dc.DataType == typeof (Double))
                return new Column<Double>(data.Cast<Double>());
            return new Column<String>(data.Cast<String>());
        }
    }
}
