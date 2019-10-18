using System.Threading;

namespace FaissSharp.Internal
{
    internal class AtomicCounter
    {
        private long _counter = 0;
        public AtomicCounter(long initialCount = 0)
        {
            _counter = initialCount;
        }
        public long Increment()
        {
            return Interlocked.Increment(ref _counter);
        }
        public long Count => Interlocked.Read(ref _counter);
    }
}