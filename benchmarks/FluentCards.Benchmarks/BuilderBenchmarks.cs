using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using FluentCards;

namespace FluentCards.Benchmarks;

/// <summary>
/// Benchmarks for card builder operations.
/// </summary>
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
public class BuilderBenchmarks
{
    [Benchmark(Description = "Build simple card")]
    public AdaptiveCard BuildSimpleCard()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb.WithText("Hello World"))
            .Build();
    }

    [Benchmark(Description = "Build card with 10 text blocks")]
    public AdaptiveCard BuildCardWith10TextBlocks()
    {
        var builder = AdaptiveCardBuilder.Create().WithVersion("1.5");
        for (int i = 0; i < 10; i++)
        {
            builder.AddTextBlock(tb => tb.WithText($"Text block {i}"));
        }
        return builder.Build();
    }

    [Benchmark(Description = "Build card with nested containers")]
    public AdaptiveCard BuildNestedContainers()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddContainer(c1 => c1
                .WithStyle(ContainerStyle.Default)
                .AddContainer(c2 => c2
                    .WithStyle(ContainerStyle.Emphasis)
                    .AddContainer(c3 => c3
                        .WithStyle(ContainerStyle.Good)
                        .AddTextBlock(tb => tb.WithText("Deep nested content")))))
            .Build();
    }

    [Benchmark(Description = "Build form card with inputs")]
    public AdaptiveCard BuildFormCard()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddInputText(i => i.WithId("name").WithLabel("Name").IsRequired(true))
            .AddInputText(i => i.WithId("email").WithLabel("Email").WithStyle(TextInputStyle.Email))
            .AddInputNumber(i => i.WithId("age").WithLabel("Age").WithMin(0).WithMax(150))
            .AddInputDate(i => i.WithId("birthdate").WithLabel("Birth Date"))
            .AddInputToggle(i => i.WithId("subscribe").WithTitle("Subscribe to newsletter"))
            .AddInputChoiceSet(i => i
                .WithId("country")
                .WithLabel("Country")
                .AddChoice("USA", "us")
                .AddChoice("Canada", "ca")
                .AddChoice("UK", "uk"))
            .AddAction(a => a.Submit("Submit"))
            .Build();
    }

    [Benchmark(Description = "Build card with column set")]
    public AdaptiveCard BuildColumnSetCard()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddColumnSet(cs => cs
                .AddColumn("auto", col => col
                    .AddImage(img => img.WithUrl("https://example.com/logo.png").WithSize(ImageSize.Small)))
                .AddColumn("stretch", col => col
                    .AddTextBlock(tb => tb.WithText("Title").WithWeight(TextWeight.Bolder))
                    .AddTextBlock(tb => tb.WithText("Subtitle").WithColor(TextColor.Default)))
                .AddColumn("auto", col => col
                    .AddTextBlock(tb => tb.WithText("Action"))))
            .Build();
    }
}
