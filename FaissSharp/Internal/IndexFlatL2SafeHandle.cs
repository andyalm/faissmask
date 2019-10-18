namespace FaissSharp.Internal
{
    internal class IndexFlatL2SafeHandle : IndexFlatSafeHandle
    {
        private static readonly NativeMethods _native = NativeMethods.Get();
        public static IndexFlatL2SafeHandle NewIndexFlatL2()
        {
            return NewIndexFlatL2(dimension: 0);
        }
        public static IndexFlatL2SafeHandle NewIndexFlatL2(long dimension)
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