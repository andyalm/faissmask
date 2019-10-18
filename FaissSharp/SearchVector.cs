namespace FaissSharp
{
    public class SearchVector
    {
        public float[] Vector { get; private set; }
        public SearchVector(float[] vector)
        {
            Vector = vector;
        }
    }
}