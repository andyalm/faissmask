using System;
using System.IO;

namespace FaissMask.Internal
{
    public class VectorTransformSafeHandle : SafeHandleZeroIsInvalid
    {
        private VectorTransformSafeHandle(IntPtr pointer) : base(pointer)
        {
        }        
        
        public static VectorTransformSafeHandle Read(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException(nameof(filename));
            }

            var fullPathFilename = Path.GetFullPath(filename);
            if (!File.Exists(fullPathFilename))
            {
                throw new FileNotFoundException($"The file {filename} does not exist", fullPathFilename);
            }

            var pointer = IntPtr.Zero;
            var returnCode = NativeMethods.faiss_read_VectorTransform_fname(fullPathFilename, ref pointer);
            if (returnCode != 0 || pointer == IntPtr.Zero)
            {
                var lastError = NativeMethods.faiss_get_last_error();

                if (string.IsNullOrEmpty(lastError))
                {
                    throw new IOException(
                        $"An unknown error occurred trying to read the vector transform '{fullPathFilename}' (return code {returnCode})");
                }

                throw new IOException(
                    $"An error occurred trying to read the vector transform '{fullPathFilename}': {lastError} (return code {returnCode})");
            }

            var safeHandle = new VectorTransformSafeHandle(pointer);

            return safeHandle;
        }
        
        public void Apply(long count, float[] vectors, float[] transformedVectors)
        {
            NativeMethods.faiss_VectorTransform_apply_noalloc(this, count, vectors, transformedVectors);
        }
        
        public int DimensionIn => NativeMethods.faiss_VectorTransform_d_in(this);
        
        public int DimensionOut => NativeMethods.faiss_VectorTransform_d_out(this);
        
        protected override bool ReleaseHandle()
        {
            if (!IsFree)
                Free();
            return true;
        }
        
        private bool IsFree { get; set; }
        
        protected internal void Free()
        {
            if (IsInvalid) return;
            NativeMethods.faiss_VectorTransform_free(this);
            IsFree = true;
        }        
        
    }
}


