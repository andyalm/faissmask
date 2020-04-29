using System;
using System.Collections.Generic;
using System.Linq;
using FaissMask;

namespace FaissMaskExample
{
    class Program
    {

        private static readonly System.Random _rand = new System.Random();
        static void Main(string[] args)
        {
            int dimension = 128;
            int vectorsCount = 25;
            int k = 5;
            int idStart = 50;
            List<float[]> vectors = new List<float[]>();

            System.Console.WriteLine($"Generating {vectorsCount} vectors with users generated ids from {idStart} to {vectorsCount - 1}...");

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
                using (var idIndex = new IndexIDMap(index))
                {
                    System.Console.WriteLine($"IsTrained {index.IsTrained}");

                    idIndex.Add(vectors, Enumerable.Range(idStart, idStart + vectorsCount).Select(r => (long)r));
                    System.Console.WriteLine($"Elements in index: {index.Count}");

                    System.Console.WriteLine("Searching...");
                    var result = idIndex.Search(vectors.Take(5).Select(v => v), k).ToArray();
                    for (int i = 0; i < 5; i++)
                    {
                        var partialResult = result[(i * k)..(i * k + k)];
                        foreach (var m in partialResult)
                        {
                            Console.Write($"{m.Label,5} (d={m.Distance:#.000})  ");
                        }
                        Console.WriteLine();
                    }
                }
            }
        }
        private static float DRand()
        {
            return (float)_rand.NextDouble();
        }
    }
}

