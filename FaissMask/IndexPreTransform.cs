using System;
using FaissMask.Internal;

namespace FaissMask
{
    public class IndexPreTransform : Index
    {
        internal IndexPreTransform(IndexPreTransformSafeHandle handle) : base(handle)
        {
        }

        public static IndexPreTransform Read(string filename)
        {
            var handle = IndexSafeHandle.Read(filename, ptr => new IndexPreTransformSafeHandle(ptr));
            return new IndexPreTransform(handle);
        }

        IndexPreTransformSafeHandle IndexPreTransformSafeHandle => Handle as IndexPreTransformSafeHandle;
    }
}