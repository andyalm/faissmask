using System;
using FaissMask.Internal;

namespace FaissMask
{
    public class IndexIVF : Index
    {
        internal IndexIVF(IndexIVFSafeHandle handle) : base(handle)
        {
        }

        public static IndexIVF Read(string filename)
        {
            var handle = IndexSafeHandle.Read(filename, ptr => new IndexIVFSafeHandle(ptr));
            return new IndexIVF(handle);
        }

        IndexIVFSafeHandle IndexIvfSafeHandle => Handle as IndexIVFSafeHandle;

        public void MakeDirectMap()
        {
            IndexIvfSafeHandle.MakeDirectMap();
        }

        public long NumProbes
        {
            get => IndexIvfSafeHandle.NumProbes;
            set => IndexIvfSafeHandle.NumProbes = value;
        }
    }
}