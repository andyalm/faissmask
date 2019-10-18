using System;
using FaissSharp.Internal;

namespace FaissSharp
{
    public class IndexFlatL2 : Index
    {
        public IndexFlatL2() : this(0)
        {

        }
        public IndexFlatL2(long dimension) : base(IndexFlatL2SafeHandle.NewIndexFlatL2(dimension))
        {
        }
    }
}