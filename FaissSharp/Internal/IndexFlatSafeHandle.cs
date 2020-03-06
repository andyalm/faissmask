using System;

namespace FaissSharp.Internal
{
    internal class IndexFlatSafeHandle : IndexSafeHandle
    {
        private static readonly NativeMethods _native = NativeMethods.Get();

        public static IndexFlatSafeHandle New()
        {
            return New(d: 0, metric: MetricType.MetricL2);
        }
        public static IndexFlatSafeHandle New(long d)
        {
            return New(d, metric: MetricType.MetricL2);
        }
        public static IndexFlatSafeHandle New(long d, MetricType metric = MetricType.MetricL2)
        {
            var index = new IndexFlatSafeHandle();
            FaissEnvironment.FaissNativeInit();
            _native.faiss_IndexFlat_new_with(ref index, d, metric);
            return index;
        }
        
        public IndexFlatSafeHandle() {}
        public IndexFlatSafeHandle(IntPtr pointer) : base(pointer) {}
        
        public override void Free()
        {
            _native.faiss_IndexFlat_free(this);
            IsFree = true;
        }

        protected override bool ReleaseHandle()
        {
            if (!IsFree)
                Free();
            return true;
        }
    }
}