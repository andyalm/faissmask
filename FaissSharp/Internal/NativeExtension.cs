using System;
using System.IO;
using System.Reflection;

namespace FaissSharp.Internal
{
    internal sealed class NativeExtension
    {
        private static volatile NativeExtension _instance;

        private readonly NativeMethods _nativeMethods;
        private static readonly object _staticLock = new object();

        private NativeExtension()
        {
            _nativeMethods = LoadNativeMethods();
            /*
            NativeLogRedirector.Redirect(this.nativeMethods);

            // Initialize
            NativeCallbackDispatcher.Init(this.nativeMethods);

            DefaultSslRootsOverride.Override(this.nativeMethods);

            Logger.Debug("gRPC native library loaded successfully.");
            */
        }
        public static NativeExtension Get()
        {
            if (_instance == null)
            {
                lock (_staticLock)
                {
                    if (_instance == null)
                    {
                        _instance = new NativeExtension();
                    }
                }
            }
            return _instance;
        }
        public NativeMethods NativeMethods
        {
            get => _nativeMethods;
        }

        private static NativeMethods LoadNativeMethods()
        {
            return new NativeMethods(LoadUnmanagedLibrary());
        }

        private static UnmanagedLibrary LoadUnmanagedLibrary()
        {
            var assemblyDirectory = Path.GetDirectoryName(GetAssemblyPath());
            var classicPath = Path.Combine(assemblyDirectory, GetNativeLibraryFilename());
            string runtimesDirectory = $"runtimes/{GetPlatformString()}/native";
            var netCorePublishedAppStylePath = Path.Combine(assemblyDirectory, runtimesDirectory, GetNativeLibraryFilename());
            var netCoreAppStylePath = Path.Combine(assemblyDirectory, "../..", runtimesDirectory, GetNativeLibraryFilename());
            string[] paths = new[] { classicPath, netCorePublishedAppStylePath, netCoreAppStylePath };
            return new UnmanagedLibrary(paths);
        }


        private static string GetAssemblyPath()
        {
            var assembly = typeof(NativeExtension).GetTypeInfo().Assembly;
            return assembly.Location;
        }

        private static string GetNativeLibraryFilename()
        {
            if (PlatformApis.IsLinux)
            {
                return "libfaiss_c.so";
            }
            else if(PlatformApis.IsMacOSX)
            {
                return "libfaiss_c.dylib";
            }
            throw new InvalidOperationException("Unsupported Platform");
        }
        private static object GetPlatformString()
        {
            if (PlatformApis.IsLinux)
            {
                return "linux";
            }
            else if(PlatformApis.IsMacOSX)
            {
                return "osx-x64";
            }
            throw new InvalidOperationException("Unsupported Platform");
        }
    }
}