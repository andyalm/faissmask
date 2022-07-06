using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

// ReSharper disable Xunit.XunitTestWithConsoleOutput

namespace FaissMask.Test
{
    public class EncodeDecodeAndAssignTests
    {
        private static readonly System.Random _rand = new System.Random();
        private readonly ITestOutputHelper _output;

        public EncodeDecodeAndAssignTests(ITestOutputHelper output)
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
                index.MakeDirectMap();

                var vector = index.ReconstructVector(1);
                var labels = index.Assign(vector, 2);
                Assert.Equal(new long[] { 1, 97 }, labels);

                vector = index.ReconstructVector(10);
                labels = index.Assign(vector, 2);
                Assert.Equal(new long[] { 10, 62 }, labels);

                Assert.Equal(8, (int)index.SaCodeSize);
                var bytes = index.EncodeVector(vector);
                Assert.Equal(new byte[] { 204, 4, 82, 34, 147, 225, 227, 37 }, bytes);

                var vector2 = index.DecodeVector(bytes);
                Assert.InRange(vector.CosineSimilarityWith(vector2), 0.9, 1.0);
            }
        }

        private static float DRand()
        {
            return (float)_rand.NextDouble();
        }
    }

    public static class VectorExtensions
    {
        public static float CosineSimilarityWith<T>(this T[] vector1, T[] vector2)
        {
            var length = vector1.Length < vector2.Length ? vector2.Length : vector1.Length;
            double dot = 0;
            double mag1 = 0;
            double mag2 = 0;
            var n = 0;
            while (n < length)
            {
                var double1 = Convert.ToDouble(vector1[n]);
                var double2 = Convert.ToDouble(vector2[n]);
                dot += double1 * double2;
                mag1 += Math.Pow(double1, 2);
                mag2 += Math.Pow(double2, 2);

                n += 1;
            }

            return Convert.ToSingle(dot / (Math.Sqrt(mag1) * Math.Sqrt(mag2)));
        }

        public static float CosineDifferenceWith<T>(this T[] vector1, T[] vector2)
        {
            return 1 - vector1.CosineSimilarityWith(vector2);
        }
    }
}