namespace FaissSharp.Internal
{
    internal class IndexFlatSafeHandle : IndexSafeHandle
    {
        private static readonly NativeMethods _native = NativeMethods.Get();

        protected static IndexFlatSafeHandle NewIndexFlat()
        {
            return NewIndexFlat(d: 0, metric: MetricType.MetricL2);
        }
        protected static IndexFlatSafeHandle NewIndexFlat(long d, MetricType metric)
        {
            var index = new IndexFlatSafeHandle();
            FaissEnvironment.FaissNativeInit();
            _native.faiss_IndexFlat_new_with(ref index, d, metric);
            return index;
        }
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