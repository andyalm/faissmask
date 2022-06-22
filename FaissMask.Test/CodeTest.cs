using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace FaissMask.Test
{
    public class CodeTest
    {
        private static readonly System.Random _rand = new System.Random();
        private readonly ITestOutputHelper _output;

        public CodeTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void EncodeAndDecodeIVFPQ()
        {
            const int dimension = 768;
            const int vectorsCount = 3;

            using (var index = IndexIVF.Read("data/index_ivfpq.index"))
            {
                Assert.True(index.SaCodeSize > 0);
                var vector = index.ReconstructVector(0);
                var bytes = index.EncodeVector(vector);
                Assert.True(bytes != null);
                Console.WriteLine(bytes.Length);
                Console.WriteLine(index.SaCodeSize);
                var vector2 = index.DecodeVector(bytes);
                Assert.True(vector2 != null);
            }
        }

        private static float DRand()
        {
            return (float)_rand.NextDouble();
        }
    }
}