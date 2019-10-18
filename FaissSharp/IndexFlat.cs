using FaissSharp.Internal;

namespace FaissSharp
{
    public class IndexFlat : Index
    {
        internal IndexFlat(IndexFlatSafeHandle handle) : base(handle) { }
        public IndexFlat() : this(0, MetricType.MetricL2)
        {
        }
        public IndexFlat(long dimension) : this(dimension, MetricType.MetricL2)
        {
        }
        public IndexFlat(long dimension, MetricType metric) : base(IndexFlatSafeHandle.New(dimension, metric))
        {
        }
    }
}