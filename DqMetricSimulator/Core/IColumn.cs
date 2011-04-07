
using System.Collections.Generic;

namespace DqMetricSimulator.Core
{
    public interface IColumn 
    {
        new object this[int id] { get; set; }
        void Add(object o);
    }
}