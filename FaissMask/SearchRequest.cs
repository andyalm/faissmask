using System.Collections.Generic;
using System.Linq;

namespace FaissMask
{
    public class SearchRequest
    {
        public long KNeighbors { get; private set; }
        public IEnumerable<SearchVector> SearchVectors { get; private set; }
        public long Count { get; private set; }
        public SearchRequest(IEnumerable<SearchVector> searchVectors, long kNeighbors)
        {
            KNeighbors = kNeighbors;
            Count = searchVectors.Count();
            SearchVectors = searchVectors;
        }
        internal float[] FlattenVectors()
        {
            return SearchVectors.SelectMany(v => v.Vector).ToArray();
        }
    }
}