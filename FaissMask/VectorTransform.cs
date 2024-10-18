using System;
using FaissMask.Extensions;
using FaissMask.Internal;

namespace FaissMask
{

    public class VectorTransform(VectorTransformSafeHandle handle, int dimIn, int dimOut) : IDisposable
    {
        public static VectorTransform Read(string filename)
        {
            var handle = VectorTransformSafeHandle.Read(filename, ptr => new VectorTransformSafeHandle(ptr));
            var dimIn = handle.DimensionIn;
            var dimOut = handle.DimensionOut;
            return new VectorTransform(handle, dimIn, dimOut);
        }
        
        public int DimensionIn => handle.DimensionIn;
        public int DimensionOut => handle.DimensionOut;

        public float[][] Apply(float[][] vectors)
        {
            var count = vectors.Length;
            var vectorsFlattened = vectors.Flatten();

            if (vectorsFlattened.Length != count * dimIn)
            {
                throw new ArgumentException($"Invalid input vectors, each should have a length of {dimIn}", nameof(vectors));
            }
            
            var output = Apply(count, vectorsFlattened);
            
            // convert to jagged array
            // a possible optimization would be to use the flattened array signature
            // directly and then pass that result to subsequent transforms or the search method,
            // but these methods would need to be public.
            var result = new float[count][];
            for (var i = 0; i < count; i++)
            {
                result[i] = new float[dimOut];
                Array.Copy(output, i * dimOut, result[i], 0, dimOut);
            }
            
            return result;            
        }

        private float[] Apply(int count, float[] vectors)
        {
            var output = new float[count * dimOut];
            handle.Apply(count, vectors, output);
            return output;
        }
        
        public void Dispose()
        {
            handle?.Free();
            handle?.Dispose();
        }
    }

}