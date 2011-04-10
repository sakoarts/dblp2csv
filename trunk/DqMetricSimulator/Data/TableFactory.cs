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
            var cols = new List<IColumn>(columns);
            for(var i= 0; i<columns; i++)cols.Add(new Column<string>());
            keys.ToList().ForEach(k => meta.KeyCols.Add(k));
            return new Table
                (
                    meta, new List<IRow>(), cols
                );
        }

        public static IColumn CreateColumn(DataColumn dc, IEnumerable<object> data)
        {
            if (new[]{typeof(Int64), typeof(Int32), typeof(Int16), typeof(UInt16), typeof(UInt32), typeof(UInt64), typeof(int)}.Contains(dc.DataType))
                return new Column<Int64>( data.Cast<Int64>() );
            if (dc.DataType == typeof (DateTime))
                return new Column<DateTime>(data.Cast<DateTime>());
            if (dc.DataType == typeof (Double))
                return new Column<Double>(data.Cast<Double>());
            return new Column<String>(data.Cast<String>());
        }
    }
}
