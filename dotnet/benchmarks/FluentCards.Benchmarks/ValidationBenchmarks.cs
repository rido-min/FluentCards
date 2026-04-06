using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using FluentCards;

namespace FluentCards.Benchmarks;

/// <summary>
/// Benchmarks for card validation operations.
/// </summary>
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
public class ValidationBenchmarks
{
    private AdaptiveCard _simpleCard = null!;
    private AdaptiveCard _complexCard = null!;
    private AdaptiveCard _cardWithIssues = null!;

    [GlobalSetup]
    public void Setup()
    {
        _simpleCard = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb.WithText("Hello World"))
            .Build();

        _complexCard = AdaptiveCardBuilder.Create()
            .WithVersion("1.6")
            .WithLang("en")
            .WithMinHeight("200px")
            .AddContainer(c => c
                .WithStyle(ContainerStyle.Emphasis)
                .AddTextBlock(tb => tb
                    .WithText("Contact Form")
                    .WithSize(TextSize.Large)
                    .WithWeight(TextWeight.Bolder)
                    .WithStyle(TextBlockStyle.Heading))
                .AddColumnSet(cs => cs
                    .AddColumn(col => col
                        .WithWidth("auto")
                        .AddImage(img => img.WithUrl("https://example.com/avatar.jpg")))
                    .AddColumn(col => col
                        .WithWidth("stretch")
                        .AddTextBlock(tb => tb.WithText("Details")))))
            .AddInputText(i => i
                .WithId("name")
                .WithLabel("Name")
                .WithLabelPosition(InputLabelPosition.Above)
                .IsRequired(true))
            .AddInputNumber(i => i
                .WithId("age")
                .WithLabel("Age")
                .WithMin(0)
                .WithMax(150))
            .AddInputChoiceSet(i => i
                .WithId("dept")
                .WithLabel("Department")
                .AddChoice("Engineering", "eng")
                .AddChoice("Sales", "sales"))
            .AddFactSet(fs => fs
                .AddFact("Status", "Active")
                .AddFact("Created", "2026-01-07"))
            .AddTable(t => t
                .WithFirstRowAsHeader(true)
                .AddColumn(new TableColumnDefinition { Width = "1" })
                .AddColumn(new TableColumnDefinition { Width = "1" })
                .AddRow(new TableRow
                {
                    Cells = new List<TableCell>
                    {
                        new TableCell { Items = new List<AdaptiveElement> { new TextBlock { Text = "H1" } } },
                        new TableCell { Items = new List<AdaptiveElement> { new TextBlock { Text = "H2" } } }
                    }
                }))
            .AddAction(a => a.Submit("Submit").WithStyle(ActionStyle.Positive))
            .AddAction(a => a.OpenUrl("https://help.example.com").WithTitle("Help"))
            .Build();

        _cardWithIssues = new AdaptiveCard
        {
            Version = "1.5",
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "" },
                new Image { Url = "not-a-url" },
                new InputText { Placeholder = "No ID" },
                new Container { Items = null },
                new FactSet { Facts = null },
                new InputNumber { Id = "n1", Min = 100, Max = 0 }
            }
        };
    }

    [Benchmark(Description = "Validate simple card")]
    public IReadOnlyList<ValidationIssue> ValidateSimple() =>
        AdaptiveCardValidator.Validate(_simpleCard);

    [Benchmark(Description = "Validate complex card")]
    public IReadOnlyList<ValidationIssue> ValidateComplex() =>
        AdaptiveCardValidator.Validate(_complexCard);

    [Benchmark(Description = "Validate card with issues")]
    public IReadOnlyList<ValidationIssue> ValidateWithIssues() =>
        AdaptiveCardValidator.Validate(_cardWithIssues);

    [Benchmark(Description = "ValidateAndThrow valid card")]
    public void ValidateAndThrowSimple() =>
        AdaptiveCardValidator.ValidateAndThrow(_simpleCard);

    [Benchmark(Description = "ValidateAndThrow complex card")]
    public void ValidateAndThrowComplex() =>
        AdaptiveCardValidator.ValidateAndThrow(_complexCard);
}
