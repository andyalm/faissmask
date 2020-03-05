using System;

namespace FaissSharp.Internal
{
    internal class IndexIDMapSafeHandle : IndexSafeHandle
    {
        private static readonly NativeMethods _native = NativeMethods.Get();
        public static IndexIDMapSafeHandle New(IndexFlatSafeHandle index)
        {
            var idIndex = new IndexIDMapSafeHandle();
            _native.faiss_IndexIDMap_new(ref idIndex, index);
            return idIndex;
        }
        
        public IndexIDMapSafeHandle() {}
        public IndexIDMapSafeHandle(IntPtr pointer) : base(pointer) {}
        public void Add(long count, float[] vectors, long[] ids)
        {
            _native.faiss_Index_add_with_ids(this, count, vectors, ids);
        }

    }
}