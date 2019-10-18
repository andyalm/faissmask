using System;
using System.Collections.Generic;
using System.Linq;
using FaissSharp;

namespace FaissSharpExample
{
    class Program
    {

        private static readonly System.Random _rand = new System.Random();
        static void Main(string[] args)
        {
            int dimension = 128;
            int vectorsCount = 100000;
            int k = 5;
            List<float[]> vectors = new List<float[]>();

            System.Console.WriteLine($"Generating {vectorsCount} vectors with sequential ids from 0 to {vectorsCount - 1}...");

            for (int i = 0; i < vectorsCount; i++)
            {
                vectors.Add(new float[dimension]);
                for (int j = 0; j < dimension; j++)
                {
                    vectors[i][j] = DRand();
                }
            }
            System.Console.WriteLine("Building index");
            using (var index = new IndexFlatL2(dimension))
            {
                System.Console.WriteLine($"IsTrained {index.IsTrained}");

                index.Add(vectors);
                System.Console.WriteLine($"Elements in index: {index.Count}");

                System.Console.WriteLine("Searching...");
                var result = index.Search(
                    new SearchRequest(
                        vectors.Take(5).Select(v => new SearchVector(v)),
                        k
                    )
                );
                foreach (var r in result)
                {
                    foreach (var m in r.Matchs)
                    {
                        Console.Write($"{m.Label,5} (d={m.Distance:#.000})  ");
                    }
                    Console.WriteLine();
                }
            }
        }
        private static float DRand()
        {
            return (float)_rand.NextDouble();
        }
    }
}

