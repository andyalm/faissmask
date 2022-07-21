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

                Console.WriteLine($"vector length={vector.Length}");

                Assert.Equal(8, (int)index.SaCodeSize);
                var bytes = index.EncodeVector(vector, 64);
                Assert.Equal(
                    new byte[]
                    {
                        204, 4, 82, 34, 147, 225, 227, 37, 169, 10, 154, 104, 253, 166, 246, 126, 216, 200, 153, 140,
                        110, 87, 161, 106, 84, 233, 106, 110, 7, 171, 90, 204, 106, 175, 18, 125, 189, 150, 83, 27, 17,
                        116, 107, 65, 137, 218, 200, 228, 19, 193, 124, 99, 26, 67, 10, 52, 141, 187, 167, 33, 145, 154,
                        41, 228
                    }, bytes);
                Console.WriteLine($"codes length={bytes.Length}");
                Console.WriteLine(string.Join(",", bytes));

                var vector2 = index.DecodeVector(bytes);
                Console.WriteLine("decoded");
                Console.WriteLine($"decoded length={vector2.Length}");
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