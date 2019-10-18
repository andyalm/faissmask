using System;
using System.Collections.Generic;
using System.Linq;
using FaissSharp.Internal;

namespace FaissSharp
{
    public abstract class Index : IDisposable
    {
        private readonly IndexSafeHandle _handle;
        public long Count
        {
            get => _handle.NTotal();
        }
        public bool IsTrained { get => _handle.IsTrained(); }
        protected Index(object handle)
        {
            _handle = handle as IndexSafeHandle;
        }
        public void Add(IEnumerable<float[]> vectors)
        {
            var count = vectors.Count();
            Add(count, vectors.SelectMany(v => v).ToArray());
        }
        private void Add(long count, float[] vectors)
        {
            _handle.Add(count, vectors);
        }

        public IEnumerable<SearchResult> Search(SearchRequest request)
        {
            float[] distances = new float[request.KNeighbors * request.Count];
            long[] labels = new long[request.KNeighbors * request.Count];
            _handle.Search(request.Count, request.FlattenVectors(), request.KNeighbors, distances, labels);
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
                    Id = request.SearchVectors.ElementAt(i).Id,
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
            _handle?.Free();
            _handle?.Dispose();
        }
    }
}