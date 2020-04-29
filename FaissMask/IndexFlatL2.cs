using System;
using FaissMask.Internal;

namespace FaissMask
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