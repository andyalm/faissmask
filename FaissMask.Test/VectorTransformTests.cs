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
    
    [Fact]
    public void AppliesASingletonVectorTransform()
    {
        using var vectorTransform = VectorTransform.Read(VectorTransformFileName);
        
        var vectors = CreateRandomVector(vectorTransform.DimensionIn);
        
        var transformed = vectorTransform.Apply(vectors);
        
        Assert.Equal(vectorTransform.DimensionOut, transformed.Length);

    }    
    
    [Theory]
    [InlineData(2)]
    [InlineData(10)]
    public void AppliesAFlattenedVectorTransform(int count)
    {
        using var vectorTransform = VectorTransform.Read(VectorTransformFileName);
        
        var vectorsFlattened = CreateRandomVector(vectorTransform.DimensionIn * count);
        
        var transformed = vectorTransform.Apply(count, vectorsFlattened);
        
        Assert.Equal(count * vectorTransform.DimensionOut, transformed.Length);

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
    
    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    public void ThrowsExceptionWhenInputFlattenedVectorsHaveInvalidLength(int count)
    {
        using var vectorTransform = VectorTransform.Read("data/vector_transform_1024_512.bin");
        
        var vectors = CreateRandomVector(vectorTransform.DimensionIn - 1 * count);
        
        var ex = Assert.Throws<ArgumentException>(() => vectorTransform.Apply(count, vectors));
        Assert.Equal(
            $"Invalid input vector, length for count {count} should be {count * vectorTransform.DimensionIn}, got {vectors.Length} (Parameter 'flattenedVectors')",
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
            vectors[i] = CreateRandomVector(dimension);
        }
        return vectors;
    }
    
    private float[] CreateRandomVector(int dimension)
    {
        var vector = new float[dimension];
        for (var i = 0; i< vector.Length; i++)
        {
            vector[i] = _random.NextSingle();
        }
        return vector;
    }

}