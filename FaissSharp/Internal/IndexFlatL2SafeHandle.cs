namespace FaissSharp.Internal
{
    internal class IndexFlatL2SafeHandle : IndexFlatSafeHandle
    {
        private static readonly NativeMethods _native = NativeMethods.Get();
        public new static IndexFlatL2SafeHandle New()
        {
            return New(dimension: 0);
        }
        public new static IndexFlatL2SafeHandle New(long dimension)
        {
            var index = new IndexFlatL2SafeHandle();
            var result = _native.faiss_IndexFlatL2_new_with(ref index, dimension);
            return index;
        }
        protected override bool ReleaseHandle()
        {
            return base.ReleaseHandle();
        }
    }
}