using System.Collections.Generic;
using System.Linq;
using FaissSharp.Internal;

namespace FaissSharp
{
    public class IndexIDMap : Index
    {
        public IndexIDMap(IndexFlat index) : base(IndexIDMapSafeHandle.New(index.Handle as IndexFlatSafeHandle))
        {
        }

        public void Add(IEnumerable<float[]> vectors, IEnumerable<long> ids)
        {
            var count = vectors.Count();
            Add(count, vectors.SelectMany(v => v).ToArray(), ids.ToArray());
        }
        private void Add(long count, float[] vectors, long[] ids)
        {
            var handle = Handle as IndexIDMapSafeHandle;
            handle.Add(count, vectors, ids);
        }
    }
}