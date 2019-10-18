using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace FaissSharp.Internal
{
    internal class UnmanagedLibrary
    {
        private readonly string _libraryPath;
        private readonly IntPtr _handle;
        private readonly ILogger _logger;
        // flags for dlopen
        const int RTLD_LAZY = 1;
        const int RTLD_GLOBAL = 8;

        public UnmanagedLibrary(string[] libraryPathAlternatives)
        {
            _libraryPath = FirstValidLibraryPath(libraryPathAlternatives);
            _logger?.LogDebug($"Intentando cargar libreria nativa {_libraryPath}");
            _handle = PlatformSpecificLoadLibrary(_libraryPath, out string loadLibraryErrorDetail);
            if (_handle == IntPtr.Zero)
            {
                throw new IOException($"Error loading native library \"{_libraryPath}\". {loadLibraryErrorDetail}");
            }
        }
        public T GetNativeMethodDelegate<T>(string methodName) where T : class
        {
            var ptr = LoadSymbol(methodName);
            if (ptr == IntPtr.Zero)
            {
                throw new MissingMethodException($"The native method \"{methodName}\" does not exist.");
            }
            return Marshal.GetDelegateForFunctionPointer<T>(ptr);
        }

        private IntPtr LoadSymbol(string symbolName)
        {
            if (PlatformApis.IsLinux)
            {
                if (PlatformApis.IsNetCore)
                {
                    return CoreCLR.dlsym(_handle, symbolName);
                }
            }
            throw new InvalidOperationException("Unsupported Platform.");
        }

        private static string FirstValidLibraryPath(string[] libraryPathAlternatives)
        {
            if (libraryPathAlternatives.Length == 0)
            {
                throw new ArgumentException("libraryPathAlternatives can't be empty.");
            }
            foreach (var path in libraryPathAlternatives)
            {
                if (File.Exists(path))
                {
                    return path;
                }
            }
            throw new FileNotFoundException($"Error loading native library. Can't be found in any possible location: {string.Join(',', libraryPathAlternatives)}");
        }

        private static IntPtr PlatformSpecificLoadLibrary(string libraryPath, out string errorMsg)
        {
            if (PlatformApis.IsLinux)
            {
                if (PlatformApis.IsNetCore)
                {
                    return LoadLibraryPosix(CoreCLR.dlopen, CoreCLR.dlerror, libraryPath, out errorMsg);
                }
            }
            throw new InvalidOperationException("Unsupported Platform.");
        }

        private static IntPtr LoadLibraryPosix(Func<string, int, IntPtr> dlopenFunc, Func<IntPtr> dlerrorFunc, string libraryPath, out string errorMsg)
        {
            errorMsg = null;
            IntPtr ret = dlopenFunc(libraryPath, RTLD_GLOBAL + RTLD_LAZY);
            if (ret == IntPtr.Zero)
            {
                errorMsg = Marshal.PtrToStringAnsi(dlerrorFunc());
            }
            return ret;
        }

        private static class CoreCLR
        {
            [DllImport("libcoreclr.so")]
            internal static extern IntPtr dlopen(string filename, int flags);
            [DllImport("libcoreclr.so")]
            internal static extern IntPtr dlerror();
            [DllImport("libcoreclr.so")]
            internal static extern IntPtr dlsym(IntPtr handle, string symbol);
        }


    }
}