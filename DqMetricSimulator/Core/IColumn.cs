
namespace DqMetricSimulator.Core
{
    public interface IColumn 
    {
        object this[int id] { get; set; }
        void Add(object o);
        int BinarySearch(object o);
    }
}