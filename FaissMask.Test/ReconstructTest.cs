using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace FaissMask.Test
{
    public class ReconstructTest
    {
        private static readonly System.Random _rand = new System.Random();
        private readonly ITestOutputHelper _output;

        public ReconstructTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void ReconstructTestIndexFlat()
        {
            const int dimension = 128;
            const int vectorsCount = 5;
            var vectors = new List<float[]>();

            for (var i = 0; i < vectorsCount; i++)
            {
                vectors.Add(new float[dimension]);
                for (int j = 0; j < dimension; j++)
                {
                    vectors[i][j] = DRand();
                }
            }

            using var index = new IndexFlatL2(dimension);
            index.Add(vectors);
            // index.MakeDirectMap();
            var reconstructedVector = index.ReconstructVector(0, dimension);
            Assert.Equal(vectors[0], reconstructedVector);

            var reconstructedVectors = index.ReconstructVectors(0, vectorsCount, dimension);
            Assert.Equal(vectors, reconstructedVectors.ToList());
        }

        [Fact]
        public void ReconstructTestIndexIvf()
        {
            const int dimension = 768;
            const int vectorsCount = 3;

            using (var index = IndexIVF.Read("data/index_ivfpq.index"))
            {
                // before making the direct map, all reconstructed vectors are zeros
                float[] reconstructedVector = index.ReconstructVector(0, dimension);
                Assert.All(reconstructedVector, f => Assert.Equal(0, f));
                index.MakeDirectMap();
                // now they will not be zeros
                reconstructedVector = index.ReconstructVector(0, dimension);

                Assert.NotNull(reconstructedVector);
                Assert.Equal(dimension, reconstructedVector.Length);
                Assert.All(reconstructedVector, f => Assert.NotEqual(0, f));

                var reconstructedVectors = index.ReconstructVectors(0, vectorsCount, dimension);
                Assert.NotNull(reconstructedVectors);
                Assert.Equal(vectorsCount, reconstructedVectors.Length);
                Assert.All(reconstructedVectors, v =>
                {
                    Assert.NotNull(v);
                    Assert.Equal(dimension, v.Length);
                    Assert.All(reconstructedVector, f => Assert.NotEqual(0, f));
                });
            }
        }

        private static float DRand()
        {
            return (float) _rand.NextDouble();
        }
    }
}