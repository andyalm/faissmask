using System;
using System.IO;

namespace FaissMask.Internal
{
    internal class IndexSafeHandle : SafeHandleZeroIsInvalid
    {
       
        public static THandle Read<THandle>(string filename, Func<IntPtr,THandle> createHandle) where THandle : IndexSafeHandle
        {
            if(string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException(nameof(filename));
            }

            filename = Path.GetFullPath(filename);
            if(!File.Exists(filename))
            {
                throw new FileNotFoundException($"The file {filename} does not exist", filename);
            }

            var pointer = IntPtr.Zero;
            var returnCode = NativeMethods.faiss_read_index_fname(filename, 0, ref pointer);
            if(returnCode != 0 || pointer == IntPtr.Zero)
            {
                var lastError = NativeMethods.faiss_get_last_error();

                if (string.IsNullOrEmpty(lastError))
                {
                    throw new IOException($"An unknown error occurred trying to read the index '{filename}' (return code {returnCode})");
                }
                else
                {
                    throw new IOException($"An error occurred trying to read the index '{filename}': {lastError} (return code {returnCode})");
                }
            }
            var index = createHandle(pointer);

            return index;
        }
        public bool IsFree { get; internal set; } = false;

        protected IndexSafeHandle()
        {
        }

        protected IndexSafeHandle(IntPtr pointer) : base(pointer) {}
        
        public void Add(long count, float[] vectors)
        {
            NativeMethods.faiss_Index_add(this, count, vectors);
        }
        public bool IsTrained()
        {
            return NativeMethods.faiss_Index_is_trained(this);
        }
        public long NTotal()
        {
            long total = NativeMethods.faiss_Index_ntotal(this);
            return total;
        }
        public virtual void Free()
        {
            if (!IsInvalid)
            {
                NativeMethods.faiss_Index_free(this);
                IsFree = true;
            }
        }
        public void Search(long count, float[] vectors, long k, float[] distances, long[] labels)
        {
            NativeMethods.faiss_Index_search(this, count, vectors, k, distances, labels);
        }

        protected override bool ReleaseHandle()
        {
            if (!IsFree)
                Free();
            return true;
        }
    }
}