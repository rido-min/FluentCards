namespace FluentCards.Benchmarks.Tests;

public class BenchmarkValidationTests
{
    [Fact]
    public void SerializationBenchmarks_CanExecute()
    {
        var benchmark = new SerializationBenchmarks();
        benchmark.Setup();
        
        Assert.NotNull(benchmark.SerializeSimple());
        Assert.NotNull(benchmark.SerializeComplex());
        Assert.NotNull(benchmark.DeserializeSimple());
        Assert.NotNull(benchmark.DeserializeComplex());
    }

    [Fact]
    public void BuilderBenchmarks_CanExecute()
    {
        var benchmark = new BuilderBenchmarks();
        
        Assert.NotNull(benchmark.BuildSimpleCard());
        Assert.NotNull(benchmark.BuildCardWith10TextBlocks());
        Assert.NotNull(benchmark.BuildNestedContainers());
        Assert.NotNull(benchmark.BuildFormCard());
        Assert.NotNull(benchmark.BuildColumnSetCard());
    }
}
