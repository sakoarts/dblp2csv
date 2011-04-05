using System;
using System.Collections.Generic;
using System.Linq;

namespace DqMetricSimulator.Core
{
    public class Metadata : IMetadata
    {
        private readonly ITable _table;
        readonly List<int> _keyCols = new List<int>();

        public IList<int> KeyCols
        {
            get { return _keyCols; }
        }

        public Metadata(ITable table)
        {
            _table = table;
        }

        public IEnumerable<object> GetKeyValuesForRow(IRow row)
        {
            return _keyCols.Select(keyCol => _table.Columns[keyCol][row.Rows[keyCol]]);
        }
    }
}
