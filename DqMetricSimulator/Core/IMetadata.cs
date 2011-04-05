using System.Collections.Generic;

namespace DqMetricSimulator.Core
{
    public interface IMetadata
    {
        IList<int> KeyCols { get; }
        IEnumerable<object> GetKeyValuesForRow(IRow row);
    }
}