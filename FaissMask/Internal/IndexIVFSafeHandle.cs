using System;

namespace FaissMask.Internal
{
    internal class IndexIVFSafeHandle : IndexSafeHandle
    {
        public IndexIVFSafeHandle() {}
        public IndexIVFSafeHandle(IntPtr pointer) : base(pointer) {}
        public void MakeDirectMap()
        {
            NativeMethods.faiss_IndexIVF_make_direct_map(this, 1);
        }

    }
}