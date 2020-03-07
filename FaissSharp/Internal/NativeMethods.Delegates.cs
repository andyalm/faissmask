using System;

namespace FaissSharp.Internal
{
    internal partial class NativeMethods
    {
        #region Native Methods

        public readonly Delegates.faiss_Index_add_delegate faiss_Index_add;
        public readonly Delegates.faiss_Index_add_with_ids_delegate faiss_Index_add_with_ids;
        public readonly Delegates.faiss_Index_is_trained_delegate faiss_Index_is_trained;
        public readonly Delegates.faiss_Index_ntotal_delegate faiss_Index_ntotal;
        public readonly Delegates.faiss_Index_free_delegate faiss_Index_free;
        public readonly Delegates.faiss_Index_search_delegate faiss_Index_search;
        public readonly Delegates.faiss_IndexFlat_new_delegate faiss_IndexFlat_new;
        public readonly Delegates.faiss_IndexFlat_new_with_delegate faiss_IndexFlat_new_with;
        public readonly Delegates.faiss_IndexFlat_free_delegate faiss_IndexFlat_free;
        public readonly Delegates.faiss_IndexFlatL2_new_delegate faiss_IndexFlatL2_new;
        public readonly Delegates.faiss_IndexFlatL2_new_with_delegate faiss_IndexFlatL2_new_with;
        public readonly Delegates.faiss_IndexIDMap_new_delegate faiss_IndexIDMap_new;
        public readonly Delegates.faiss_read_index_fname_delegate faiss_read_index_fname;

        #endregion

        public NativeMethods(UnmanagedLibrary library)
        {
            this.faiss_Index_add = GetMethodDelegate<Delegates.faiss_Index_add_delegate>(library);
            this.faiss_Index_add_with_ids = GetMethodDelegate<Delegates.faiss_Index_add_with_ids_delegate>(library);
            this.faiss_Index_ntotal = GetMethodDelegate<Delegates.faiss_Index_ntotal_delegate>(library);
            this.faiss_Index_is_trained = GetMethodDelegate<Delegates.faiss_Index_is_trained_delegate>(library);
            this.faiss_Index_free = GetMethodDelegate<Delegates.faiss_Index_free_delegate>(library);
            this.faiss_Index_search = GetMethodDelegate<Delegates.faiss_Index_search_delegate>(library);
            this.faiss_IndexFlat_new = GetMethodDelegate<Delegates.faiss_IndexFlat_new_delegate>(library);
            this.faiss_IndexFlat_new_with = GetMethodDelegate<Delegates.faiss_IndexFlat_new_with_delegate>(library);
            this.faiss_IndexFlat_free = GetMethodDelegate<Delegates.faiss_IndexFlat_free_delegate>(library);
            this.faiss_IndexFlatL2_new = GetMethodDelegate<Delegates.faiss_IndexFlatL2_new_delegate>(library);
            this.faiss_IndexFlatL2_new_with = GetMethodDelegate<Delegates.faiss_IndexFlatL2_new_with_delegate>(library);
            this.faiss_IndexIDMap_new = GetMethodDelegate<Delegates.faiss_IndexIDMap_new_delegate>(library);
            this.faiss_read_index_fname = GetMethodDelegate<Delegates.faiss_read_index_fname_delegate>(library);
        }
        public class Delegates
        {
            public delegate int faiss_Index_add_delegate(IndexSafeHandle index, long n, float[] x);
            public delegate int faiss_Index_add_with_ids_delegate(IndexSafeHandle index, long n, float[] x, long[] xids);
            public delegate bool faiss_Index_is_trained_delegate(IndexSafeHandle index);
            public delegate long faiss_Index_ntotal_delegate(IndexSafeHandle index);
            public delegate int faiss_Index_free_delegate(IndexSafeHandle index);
            public delegate int faiss_Index_search_delegate(IndexSafeHandle index, long n, float[] x, long k, float[] distances, long[] labels);
            public delegate int faiss_IndexFlat_new_delegate(ref IndexFlatSafeHandle index);
            public delegate int faiss_IndexFlat_new_with_delegate(ref IndexFlatSafeHandle index, long d, MetricType metric);
            public delegate int faiss_IndexFlat_free_delegate(IndexFlatSafeHandle index);
            public delegate int faiss_IndexFlatL2_new_delegate(ref IndexFlatL2SafeHandle index);
            public delegate int faiss_IndexFlatL2_new_with_delegate(ref IndexFlatL2SafeHandle index, long d);
            public delegate int faiss_IndexIDMap_new_delegate(ref IndexIDMapSafeHandle mapIndex, IndexFlatSafeHandle index);
            public delegate int faiss_read_index_fname_delegate(string fname, int io_flags, ref IntPtr p_out);
        }
    }
}