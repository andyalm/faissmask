using System;

namespace FaissMask.Internal
{
    public abstract class SafeHandleZeroIsInvalid : System.Runtime.InteropServices.SafeHandle
    {
        public override bool IsInvalid => handle == IntPtr.Zero;
        public SafeHandleZeroIsInvalid() : base(IntPtr.Zero, true) { }

        protected SafeHandleZeroIsInvalid(IntPtr pointer) : base(pointer, true) {}
    }
}