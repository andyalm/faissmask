using System;

namespace FaissMask.Internal
{
    internal class IndexIDMapSafeHandle : IndexSafeHandle
    {
        public static IndexIDMapSafeHandle New(IndexFlatSafeHandle index)
        {
            var idIndex = new IndexIDMapSafeHandle();
            NativeMethods.faiss_IndexIDMap_new(ref idIndex, index);
            return idIndex;
        }
        
        public IndexIDMapSafeHandle() {}
        public IndexIDMapSafeHandle(IntPtr pointer) : base(pointer) {}
        public void Add(long count, float[] vectors, long[] ids)
        {
            NativeMethods.faiss_Index_add_with_ids(this, count, vectors, ids);
        }
        public IndexSafeHandle SubIndex => NativeMethods.faiss_IndexIDMap_sub_index(this);
    }
}