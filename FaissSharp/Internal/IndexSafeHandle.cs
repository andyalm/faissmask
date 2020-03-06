using System;

namespace FaissSharp.Internal
{
    internal class IndexSafeHandle : SafeHandleZeroIsInvalid
    {
        private static readonly NativeMethods _native = NativeMethods.Get();
        
        public static THandle Read<THandle>(string filename, Func<IntPtr,THandle> createHandle) where THandle : IndexSafeHandle
        {
            var pointer = _native.faiss_read_index(filename);
            var index = createHandle(pointer);
            FaissEnvironment.FaissNativeInit();

            return index;
        }
        public bool IsFree { get; internal set; } = false;

        protected IndexSafeHandle()
        {
        }

        protected IndexSafeHandle(IntPtr pointer) : base(pointer) {}
        
        public void Add(long count, float[] vectors)
        {
            _native.faiss_Index_add(this, count, vectors);
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