namespace FaissSharp
{
    public class SearchVector
    {
        public string Id { get; private set; }
        public float[] Vector { get; private set; }

        public SearchVector(float[] vector) : this(System.Guid.NewGuid().ToString("N"), vector)
        {
        }
        public SearchVector(string id, float[] vector)
        {
            Id = id;
            Vector = vector;
        }
    }
}