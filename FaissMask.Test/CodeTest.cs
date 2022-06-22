using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

// ReSharper disable Xunit.XunitTestWithConsoleOutput

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
            Console.WriteLine("aaaa");
            _output.WriteLine("bbbb");

            const int dimension = 768;
            const int vectorsCount = 3;

            using (var index = IndexIVF.Read("data/index_ivfpq.index"))
            {
                index.MakeDirectMap();

                var vector = index.ReconstructVector(1);
                Console.WriteLine(string.Join(",", vector.Select(v => v.ToString())));
                var labels = index.Assign(vector, 2);
                Console.WriteLine(string.Join(",", labels.Select(v => v.ToString())));

                vector = index.ReconstructVector(10);
                Console.WriteLine(string.Join(",", vector.Select(v => v.ToString())));
                labels = index.Assign(vector, 2);
                Console.WriteLine(string.Join(",", labels.Select(v => v.ToString())));

                Console.WriteLine("cccc");
                Console.WriteLine(index.SaCodeSize);
                var bytes = index.EncodeVector(vector);
                Console.WriteLine(bytes);
                Console.WriteLine(string.Join(",", bytes.Select(v => v.ToString())));
                Console.WriteLine(bytes.Length.ToString());
                Console.WriteLine(index.SaCodeSize.ToString());
                var vector2 = index.DecodeVector(bytes);
                Console.WriteLine(vector2);
                Console.WriteLine(string.Join(",", vector2.Select(v => v.ToString())));
            }
        }

        private static float DRand()
        {
            return (float)_rand.NextDouble();
        }
    }
}