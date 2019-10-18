using System;

namespace FaissSharp.Internal
{
    internal class IndexSafeHandle : SafeHandleZeroIsInvalid
    {
        private static readonly NativeMethods _native = NativeMethods.Get();
        public bool IsFree { get; internal set; } = false;

        protected IndexSafeHandle()
        {
        }
        public void Add(long count, float[] vectors)
        {
            _native.faiss_Index_add(this, count, vectors);
        }
        public void Add(long count, float[] vectors, long[] ids)
        {
            _native.faiss_Index_add_with_ids(this, count, vectors, ids);
        }
        public bool IsTrained()
        {
            return _native.faiss_Index_is_trained(this);
        }
        public long NTotal()
        {
            long total = _native.faiss_Index_ntotal(this);
            return total;
        }
        public virtual void Free()
        {
            _native.faiss_Index_free(this);
            IsFree = true;
        }
        public void Search(long count, float[] vectors, long k, float[] distances, long[] labels)
        {
            _native.faiss_Index_search(this, count, vectors, k, distances, labels);
        }

        protected override bool ReleaseHandle()
        {
            if (!IsFree)
                Free();
            return true;
        }
    }
}