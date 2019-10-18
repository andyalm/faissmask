using System;
using System.Runtime.InteropServices;

namespace FaissSharp.Internal
{
    internal static class PlatformApis
    {
        public static bool IsLinux { get; private set; }
        public static bool IsMacOSX { get; private set; }
        public static bool IsWindows { get; private set; }
        public static bool IsMono { get; private set; }
        public static bool IsNetCore { get; private set; }
        public static bool Is64Bit { get; private set; }
        static PlatformApis()
        {
            IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            IsMacOSX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            IsNetCore = RuntimeInformation.FrameworkDescription.StartsWith(".NET Core");
            IsMono = Type.GetType("Mono.Runtime") != null;
            Is64Bit = IntPtr.Size == 8;
        }
    }
}