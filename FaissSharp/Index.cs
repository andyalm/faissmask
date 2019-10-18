using System;
using System.Collections.Generic;
using System.Linq;
using FaissSharp.Internal;

namespace FaissSharp
{
    public abstract class Index : IDisposable
    {
        internal readonly IndexSafeHandle Handle;
        public long Count
        {
            get => Handle.NTotal();
        }
        public bool IsTrained { get => Handle.IsTrained(); }
        protected Index(object handle)
        {
            Handle = handle as IndexSafeHandle;
        }
        public void Add(IEnumerable<float[]> vectors)
        {
            var count = vectors.Count();
            Add(count, vectors.SelectMany(v => v).ToArray());
        }
        private void Add(long count, float[] vectors)
        {
            Handle.Add(count, vectors);
        }

        public IEnumerable<SearchResult> Search(SearchRequest request)
        {
            float[] distances = new float[request.KNeighbors * request.Count];
            long[] labels = new long[request.KNeighbors * request.Count];
            Handle.Search(request.Count, request.FlattenVectors(), request.KNeighbors, distances, labels);
            var labelDistanceZip = labels.Zip(distances, (l, d) => new
            {
                Label = l,
                Distance = d
            });
            for (int i = 0; i < request.Count; i++)
            {
                var vectorResult = labelDistanceZip.Skip((int)(i * request.KNeighbors))
                    .Take((int)request.KNeighbors);
                yield return new SearchResult
                {
                    Matchs = vectorResult.Select(ld => new SearchResultMatch
                    {
                        Label = ld.Label,
                        Distance = ld.Distance
                    }).ToList()
                };
            }

        }

        public void Dispose()
        {
            Handle?.Free();
            Handle?.Dispose();
        }
    }
}