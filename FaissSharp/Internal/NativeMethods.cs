using System;

namespace FaissSharp.Internal
{
    internal partial class NativeMethods
    {
        public static NativeMethods Get()
        {
            return NativeExtension.Get().NativeMethods;
        }
        private static T GetMethodDelegate<T>(UnmanagedLibrary library) where T : class
        {
            var methodName = RemoveStringSuffix(typeof(T).Name, "_delegate");
            return library.GetNativeMethodDelegate<T>(methodName);
        }

        private static string RemoveStringSuffix(string str, string toRemove)
        {
            if (!str.EndsWith(toRemove))
            {
                return str;
            }
            return str.Substring(0, str.Length - toRemove.Length);
        }
    }
}