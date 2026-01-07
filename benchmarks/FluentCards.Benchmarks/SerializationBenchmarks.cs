using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using FluentCards;
using FluentCards.Serialization;
using System.Text.Json;

namespace FluentCards.Benchmarks;

/// <summary>
/// Benchmarks for serialization and deserialization operations.
/// </summary>
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
public class SerializationBenchmarks
{
    private AdaptiveCard _simpleCard = null!;
    private AdaptiveCard _complexCard = null!;
    private string _simpleJson = null!;
    private string _complexJson = null!;
    private byte[] _simpleUtf8 = null!;
    private byte[] _complexUtf8 = null!;

    [GlobalSetup]
    public void Setup()
    {
        _simpleCard = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb.WithText("Hello World"))
            .Build();

        _complexCard = CreateComplexCard();
        
        _simpleJson = _simpleCard.ToJson();
        _complexJson = _complexCard.ToJson();
        _simpleUtf8 = AdaptiveCardSerializer.SerializeToUtf8Bytes(_simpleCard);
        _complexUtf8 = AdaptiveCardSerializer.SerializeToUtf8Bytes(_complexCard);
    }

    private static AdaptiveCard CreateComplexCard()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddContainer(c => c
                .WithStyle(ContainerStyle.Emphasis)
                .AddTextBlock(tb => tb
                    .WithText("Contact Form")
                    .WithSize(TextSize.Large)
                    .WithWeight(TextWeight.Bolder)))
            .AddInputText(i => i
                .WithId("name")
                .WithLabel("Name")
                .IsRequired(true))
            .AddInputText(i => i
                .WithId("email")
                .WithLabel("Email")
                .WithStyle(TextInputStyle.Email))
            .AddInputChoiceSet(i => i
                .WithId("department")
                .WithLabel("Department")
                .AddChoice("Engineering", "eng")
                .AddChoice("Sales", "sales")
                .AddChoice("Marketing", "mkt"))
            .AddColumnSet(cs => cs
                .AddColumn("auto", col => col
                    .AddImage(img => img.WithUrl("https://example.com/avatar.jpg")))
                .AddColumn("stretch", col => col
                    .AddTextBlock(tb => tb.WithText("Additional details go here"))))
            .AddFactSet(fs => fs
                .AddFact("Status", "Active")
                .AddFact("Created", "2026-01-07")
                .AddFact("Priority", "High"))
            .AddAction(a => a.Submit("Submit").WithStyle(ActionStyle.Positive))
            .AddAction(a => a.OpenUrl("https://help.example.com").WithTitle("Help"))
            .Build();
    }

    [Benchmark(Description = "Serialize simple card (string)")]
    public string SerializeSimple() => _simpleCard.ToJson();

    [Benchmark(Description = "Serialize complex card (string)")]
    public string SerializeComplex() => _complexCard.ToJson();

    [Benchmark(Description = "Serialize simple card (UTF-8 bytes)")]
    public byte[] SerializeSimpleUtf8() => AdaptiveCardSerializer.SerializeToUtf8Bytes(_simpleCard);

    [Benchmark(Description = "Serialize complex card (UTF-8 bytes)")]
    public byte[] SerializeComplexUtf8() => AdaptiveCardSerializer.SerializeToUtf8Bytes(_complexCard);

    [Benchmark(Description = "Deserialize simple card (string)")]
    public AdaptiveCard? DeserializeSimple() => AdaptiveCardExtensions.FromJson(_simpleJson);

    [Benchmark(Description = "Deserialize complex card (string)")]
    public AdaptiveCard? DeserializeComplex() => AdaptiveCardExtensions.FromJson(_complexJson);

    [Benchmark(Description = "Deserialize simple card (UTF-8 bytes)")]
    public AdaptiveCard? DeserializeSimpleUtf8() => AdaptiveCardSerializer.DeserializeFromUtf8Bytes(_simpleUtf8);

    [Benchmark(Description = "Deserialize complex card (UTF-8 bytes)")]
    public AdaptiveCard? DeserializeComplexUtf8() => AdaptiveCardSerializer.DeserializeFromUtf8Bytes(_complexUtf8);

    [Benchmark(Description = "Roundtrip simple card")]
    public AdaptiveCard? RoundtripSimple()
    {
        var json = _simpleCard.ToJson();
        return AdaptiveCardExtensions.FromJson(json);
    }

    [Benchmark(Description = "Roundtrip complex card")]
    public AdaptiveCard? RoundtripComplex()
    {
        var json = _complexCard.ToJson();
        return AdaptiveCardExtensions.FromJson(json);
    }
}
