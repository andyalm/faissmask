using System;

namespace FaissMask.Internal
{
    internal class IndexPreTransformSafeHandle : IndexSafeHandle
    {
        public IndexPreTransformSafeHandle() {}
        public IndexPreTransformSafeHandle(IntPtr pointer) : base(pointer) {}

        public static IndexPreTransformSafeHandle New()
        {
            var index = new IndexPreTransformSafeHandle();
            NativeMethods.faiss_IndexPreTransform_new(ref index);
            return index;
        }

        protected override bool ReleaseHandle()
        {
            if (!IsFree)
                Free();
            return true;
        }
    }
}