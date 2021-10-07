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

        public void MakeDirectMap()
        {
            var handle = Handle as IndexIVFSafeHandle;
            handle.MakeDirectMap();
        }
    }
}