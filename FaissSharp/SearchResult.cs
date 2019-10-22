using System.Collections.Generic;

namespace FaissSharp
{
    public class SearchResult
    {
        public long Label { get; set; }
        public float Distance { get; set; }
    }
}