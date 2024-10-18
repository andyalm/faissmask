using System;
using System.IO;
using Xunit;

namespace FaissMask.Test;

public class VectorTransformTests
{
    private const string VectorTransformFileName = "data/vector_transform_1024_512.bin";
    private const int ExpectedDimensionIn = 1024;
    private const int ExpectedDimensionOut = 512;
    private readonly Random _random = new(42);
    
    [Fact]
    public void ReadsVectorTransformFile()
    {
        using var vectorTransform = VectorTransform.Read(VectorTransformFileName);
        Assert.NotNull(vectorTransform);
        Assert.Equal(ExpectedDimensionIn, vectorTransform.DimensionIn);
        Assert.Equal(ExpectedDimensionOut, vectorTransform.DimensionOut);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    public void AppliesAVectorTransform(int count)
    {
        using var vectorTransform = VectorTransform.Read(VectorTransformFileName);
        
        var vectors = CreateRandomVectors(vectorTransform.DimensionIn, count);
        
        var transformed = vectorTransform.Apply(vectors);
        
        Assert.Equal(count, transformed.Length);
        foreach (var vector in transformed)
        {
            Assert.Equal(vectorTransform.DimensionOut, vector.Length);
        }

    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    public void ThrowsExceptionWhenInputVectorsHaveInvalidLength(int count)
    {
        using var vectorTransform = VectorTransform.Read("data/vector_transform_1024_512.bin");
        
        var vectors = CreateRandomVectors(vectorTransform.DimensionIn - 1, count);
        
        var ex = Assert.Throws<ArgumentException>(() => vectorTransform.Apply(vectors));
        Assert.Equal(
            $"Invalid input vectors, each should have a length of {vectorTransform.DimensionIn} (Parameter 'vectors')",
            ex.Message);
    }
    
    [Fact]
    public void ThrowsExceptionWhenVectorTransformFilenameParameterIsNull()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => VectorTransform.Read(null));
        Assert.Equal("Value cannot be null. (Parameter 'filename')", ex.Message);
    }    
    
    [Fact]
    public void ThrowsExceptionWhenVectorTransformFileDoesNotExist()
    {
        var ex = Assert.Throws<FileNotFoundException>(() => VectorTransform.Read("non_existent_file.bin"));
        Assert.Equal("The file non_existent_file.bin does not exist", ex.Message);
    }

    private float[][] CreateRandomVectors(int dimension, int count)
    {
        var vectors = new float[count][];
        for (var i = 0; i < count; i++)
        {
            var vector = new float[dimension];
            for (var j = 0; j < vector.Length; j++)
            {
                vector[j] = _random.NextSingle();
            }
            vectors[i] = vector;
        }
        return vectors;
    }

}