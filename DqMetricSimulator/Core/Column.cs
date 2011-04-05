using System;
using System.Collections.Generic;

namespace DqMetricSimulator.Core
{
    public class Column<T> : IColumn
    {
        private readonly List<T> _data = new List<T>();
        
        public T Get(int id)
        {
            return _data[id];
        }
        public object this[int id]
        {
            get { return _data[id]; }
            set { _data[id] = (T)value; }
        }
        public IList<T> Data
        {
            get { return _data; }
        }
    }
}
