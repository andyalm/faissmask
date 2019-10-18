using System.Collections.Generic;

namespace FaissSharp
{
    public class SearchResult
    {
        public string Id { get; internal set; }
        public IList<SearchResultMatch> Matchs;
    }
}