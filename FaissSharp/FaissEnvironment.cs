
using FaissSharp.Internal;

namespace FaissSharp
{
    public class FaissEnvironment
    {
        private static readonly AtomicCounter _nativeInitCounter = new AtomicCounter();

        private static bool IsNativeShutdownAllowed => true;
        internal static void FaissNativeInit()
        {
            if (!IsNativeShutdownAllowed && _nativeInitCounter.Count > 0)
            {
                return;
            }
            // Llmar metodo init de faiss
            _nativeInitCounter.Increment();
        }
        internal static void FaissNativeShutdown()
        {
            if (IsNativeShutdownAllowed)
            {
                // Llamar a metodo shutdown de faiss
            }
        }
    }
}