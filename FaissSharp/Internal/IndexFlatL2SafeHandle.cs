namespace FaissSharp.Internal
{
    internal class IndexFlatL2SafeHandle : IndexFlatSafeHandle
    {
        public new static IndexFlatL2SafeHandle New()
        {
            return New(dimension: 0);
        }
        public new static IndexFlatL2SafeHandle New(long dimension)
        {
            var index = new IndexFlatL2SafeHandle();
            var result = NativeMethods.faiss_IndexFlatL2_new_with(ref index, dimension);
            return index;
        }
        protected override bool ReleaseHandle()
        {
            return base.ReleaseHandle();
        }
    }
}