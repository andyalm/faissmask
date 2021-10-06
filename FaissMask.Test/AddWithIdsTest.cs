using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace FaissMask.Test
{
    public class Test
    {
        private static readonly System.Random _rand = new System.Random();
        private readonly ITestOutputHelper _output;

        public Test(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void AddWithIdsTest()
        {
            int dimension = 128;
            int vectorsCount = 5;
            int idStart = 5;
            int k = 5;
            List<float[]> vectors = new List<float[]>();

            for (int i = 0; i < vectorsCount; i++)
            {
                vectors.Add(new float[dimension]);
                for (int j = 0; j < dimension; j++)
                {
                    vectors[i][j] = DRand();
                }
            }
            using (var index = new IndexFlatL2(dimension))
            {
                using (var idIndex = new IndexIDMap(index))
                {
                    idIndex.Add(vectors, Enumerable.Range(idStart, idStart + vectorsCount).Select(r => (long)r));
                    var result = idIndex.Search(vectors.Select(v => v), k);
                    foreach (var m in result)
                    {
                        Assert.InRange(m.Label, idStart, idStart + vectorsCount - 1);
                    }
                }
            }
        }

        [Fact]
        public void CanReadIndex()
        {
            using var index = IndexFlat.Read("data/faiss.index");
            var results = index.Search(new float[]
            {
                20, 48
            }, 5);
            Assert.NotNull(results);
            _output.WriteLine($"Number of results: {results.Count()}");
            Assert.True(results.Any());
            _output.WriteLine($"Number of dimensions: {index.Dimensions}");
            Assert.Equal(2048, index.Dimensions);
        }
        private static float DRand()
        {
            return (float)_rand.NextDouble();
        }
    }
}
