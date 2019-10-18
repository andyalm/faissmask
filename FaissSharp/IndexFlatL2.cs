using System;
using FaissSharp.Internal;

namespace FaissSharp
{
    public class IndexFlatL2 : IndexFlat
    {
        public IndexFlatL2() : this(0)
        {

        }
        public IndexFlatL2(long dimension) : base(IndexFlatL2SafeHandle.New(dimension))
        {
        }
    }
}