using System;
using System.Runtime.InteropServices;

namespace FaissMask.Internal
{
    internal static class NativeMethods
    {
        [DllImport("faiss_c", SetLastError = true)]
        public static extern int faiss_Index_add(IndexSafeHandle index, long n, float[] x);
        [DllImport("faiss_c", SetLastError = true)]
        public static extern int faiss_Index_add_with_ids(IndexSafeHandle index, long n, float[] x, long[] xids);
        [DllImport("faiss_c", SetLastError = true)]
        public static extern bool faiss_Index_is_trained(IndexSafeHandle index);
        [DllImport("faiss_c", SetLastError = true)]
        public static extern long faiss_Index_ntotal(IndexSafeHandle index);
        [DllImport("faiss_c", SetLastError = true)]
        public static extern int faiss_Index_free(IndexSafeHandle index);
        [DllImport("faiss_c", SetLastError = true)]
        public static extern int faiss_Index_search(IndexSafeHandle index, long n, float[] x, long k, float[] distances, long[] labels);
        [DllImport("faiss_c", SetLastError = true)]
        public static extern int faiss_IndexFlat_new(ref IndexFlatSafeHandle index);
        [DllImport("faiss_c", SetLastError = true)]
        public static extern int faiss_IndexFlat_new_with(ref IndexFlatSafeHandle index, long d, MetricType metric);
        [DllImport("faiss_c", SetLastError = true)]
        public static extern int faiss_IndexFlat_free(IndexFlatSafeHandle index);
        [DllImport("faiss_c", SetLastError = true)]
        public static extern int faiss_IndexFlatL2_new(ref IndexFlatL2SafeHandle index);
        [DllImport("faiss_c", SetLastError = true)]
        public static extern int faiss_IndexFlatL2_new_with(ref IndexFlatL2SafeHandle index, long d);
        [DllImport("faiss_c", SetLastError = true)]
        public static extern int faiss_IndexIDMap_new(ref IndexIDMapSafeHandle mapIndex, IndexFlatSafeHandle index);
        [DllImport("faiss_c", SetLastError = true)]
        public static extern int faiss_read_index_fname(string fname, int io_flags, ref IntPtr p_out);
    }
}