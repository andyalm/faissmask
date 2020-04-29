using System;
using System.Collections.Generic;
using System.Linq;
using FaissMask.Extensions;
using FaissMask.Internal;

namespace FaissMask
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

        public IEnumerable<SearchResult> Search(IEnumerable<float[]> request, long kneighbors)
        {
            int count = request.Count();
            float[] distances = new float[kneighbors * count];
            long[] labels = new long[kneighbors * count];
            Handle.Search(count, request.Flatten(), kneighbors, distances, labels);
            var labelDistanceZip = labels.Zip(distances, (l, d) => new
            {
                Label = l,
                Distance = d
            });
            for (int i = 0; i < count; i++)
            {
                var vectorResult = labelDistanceZip.Skip((int)(i * kneighbors))
                    .Take((int)kneighbors);
                foreach(var r in vectorResult)
                {
                    yield return new SearchResult
                    {
                        Label = r.Label,
                        Distance = r.Distance
                    };
                }
            }

        }

        public void Dispose()
        {
            Handle?.Free();
            Handle?.Dispose();
        }
    }
}