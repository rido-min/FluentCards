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

    [Benchmark(Description = "Build card with v1.6 properties")]
    public AdaptiveCard BuildCardWithV16Properties()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.6")
            .WithLang("en")
            .WithMinHeight("200px")
            .WithFallbackText("Upgrade your client")
            .WithVerticalContentAlignment(VerticalAlignment.Center)
            .AddTextBlock(tb => tb
                .WithText("Heading")
                .WithStyle(TextBlockStyle.Heading)
                .WithFontType(FontType.Monospace)
                .WithIsSubtle(true))
            .AddInputText(i => i
                .WithId("name")
                .WithLabel("Name")
                .WithLabelPosition(InputLabelPosition.Above)
                .WithLabelWidth("40%")
                .WithInputStyle(InputStyle.RevealOnHover)
                .IsRequired(true))
            .AddMedia(m => m
                .AddSource("https://example.com/video.mp4", "video/mp4")
                .AddCaptionSource("English", "https://example.com/en.vtt", "text/vtt"))
            .AddAction(a => a
                .Submit("Submit")
                .WithMode(ActionMode.Primary)
                .WithTooltip("Submit form"))
            .Build();
    }

    [Benchmark(Description = "Build card with all element types")]
    public AdaptiveCard BuildCardWithAllElements()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.6")
            .AddTextBlock(tb => tb.WithText("All Elements"))
            .AddImage(img => img.WithUrl("https://example.com/img.png"))
            .AddContainer(c => c.AddTextBlock(tb => tb.WithText("In container")))
            .AddColumnSet(cs => cs
                .AddColumn(col => col
                    .WithWidth("stretch")
                    .WithSpacing(Spacing.Medium)
                    .WithSeparator(true)
                    .AddTextBlock(tb => tb.WithText("Column"))))
            .AddFactSet(fs => fs.AddFact("Key", "Value"))
            .AddRichTextBlock(rtb => rtb
                .AddTextRun(tr => tr.WithText("Rich text")))
            .AddImageSet(imgSet => imgSet
                .WithImageSize(ImageSize.Small)
                .AddImage(img => img.WithUrl("https://example.com/img1.png"))
                .AddImage(img => img.WithUrl("https://example.com/img2.png")))
            .AddTable(t => t
                .WithFirstRowAsHeader(true)
                .AddColumn(new TableColumnDefinition { Width = "1" })
                .AddRow(new TableRow
                {
                    Cells = new List<TableCell>
                    {
                        new TableCell { Items = new List<AdaptiveElement> { new TextBlock { Text = "Cell" } } }
                    }
                }))
            .AddActionSet(acs => acs
                .AddAction(a => a.OpenUrl("https://example.com").WithTitle("Open")))
            .AddMedia(m => m.AddSource("https://example.com/vid.mp4", "video/mp4"))
            .AddAction(a => a.Submit("Submit"))
            .AddAction(a => a.OpenUrl("https://example.com").WithTitle("Open"))
            .Build();
    }
}
