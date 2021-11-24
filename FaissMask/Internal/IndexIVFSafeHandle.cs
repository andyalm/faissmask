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
        public long NumProbes
        {
            get => NativeMethods.faiss_IndexIVF_nprobe(this);
            set => NativeMethods.faiss_IndexIVF_set_nprobe(this, value);
        }
    }
}