using System.Collections.Generic;
using System.Linq;
using FaissMask.Internal;

namespace FaissMask
{
    public class IndexIDMap : Index
    {
        public static IndexIDMap Read(string filename)
        {
            var handle = IndexSafeHandle.Read(filename, ptr => new IndexIDMapSafeHandle(ptr));

            return new IndexIDMap(handle);
        }

        internal IndexIDMap(IndexIDMapSafeHandle handle) : base(handle)
        {
        }

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