using System;
using FaissMask.Extensions;
using FaissMask.Internal;

namespace FaissMask
{

    public class VectorTransform(VectorTransformSafeHandle handle, int dimIn, int dimOut) : IDisposable
    {
        public static VectorTransform Read(string filename)
        {
            var handle = VectorTransformSafeHandle.Read(filename);
            var dimIn = handle.DimensionIn;
            var dimOut = handle.DimensionOut;
            return new VectorTransform(handle, dimIn, dimOut);
        }
        
        public int DimensionIn => dimIn;
        public int DimensionOut => dimOut;
        
        public float[] Apply(float[] vector)
        {
            return Apply(1, vector);
        }

        public float[][] Apply(float[][] vectors)
        {
            var count = vectors.Length;
            var vectorsFlattened = vectors.Flatten();

            if (vectorsFlattened.Length != count * dimIn)
            {
                throw new ArgumentException($"Invalid input vectors, each should have a length of {dimIn}", nameof(vectors));
            }
            
            var output = Apply(count, vectorsFlattened);
            
            // convert to an array of arrays
            // the other signatures are more efficient if this response is re-flattened
            // e.g., Apply(float[] vector) or Apply(int count, float[] flattenedVectors)

            var result = new float[count][];
            for (var i = 0; i < count; i++)
            {
                result[i] = new float[dimOut];
                Array.Copy(output, i * dimOut, result[i], 0, dimOut);
            }
            
            return result;            
        }

        public float[] Apply(int count, float[] flattenedVectors)
        {
            if (count * dimIn != flattenedVectors.Length)
            {
                throw new ArgumentException($"Invalid input vector, length for count {count} should be {count*dimIn}, got {flattenedVectors.Length}", nameof(flattenedVectors));
            }
            var output = new float[count * dimOut];
            handle.Apply(count, flattenedVectors, output);
            return output;
        }
        
        public void Dispose()
        {
            handle?.Free();
            handle?.Dispose();
        }
    }

}