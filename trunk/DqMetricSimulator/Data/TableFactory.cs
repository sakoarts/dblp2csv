using System;
using System.Collections.Generic;
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
    }
}
