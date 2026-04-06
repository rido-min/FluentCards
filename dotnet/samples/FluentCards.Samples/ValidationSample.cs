namespace FluentCards.Samples;

/// <summary>
/// Demonstrates validating Adaptive Cards for structural and semantic issues.
/// </summary>
public static class ValidationSample
{
    /// <summary>
    /// Runs all validation demonstrations.
    /// </summary>
    public static void Run()
    {
        DemonstrateValidCard();
        DemonstrateStructuralErrors();
        DemonstrateInvalidInputRange();
        DemonstrateVersionMismatch();
        DemonstrateValidateAndThrow();
    }

    /// <summary>
    /// Validates a well-formed card — expects zero issues.
    /// </summary>
    static void DemonstrateValidCard()
    {
        Console.WriteLine("\n=== Validation: Valid Card ===");

        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb
                .WithText("All good!")
                .WithSize(TextSize.Large)
                .WithWrap(true))
            .AddAction(a => a
                .OpenUrl("https://adaptivecards.io")
                .WithTitle("Learn More"))
            .Build();

        var issues = AdaptiveCardValidator.Validate(card);
        PrintIssues(issues);
    }

    /// <summary>
    /// Validates a card with missing required fields — expects multiple errors.
    /// </summary>
    static void DemonstrateStructuralErrors()
    {
        Console.WriteLine("\n=== Validation: Structural Errors ===");

        // Build a card with intentional problems: no version, TextBlock with no text, Image with no URL
        var card = new AdaptiveCard
        {
            Version = "",
            Body =
            [
                new TextBlock { Text = "" },
                new Image { Url = null }
            ]
        };

        var issues = AdaptiveCardValidator.Validate(card);
        PrintIssues(issues);
    }

    /// <summary>
    /// Validates an Input.Number with min greater than max — expects a range error.
    /// </summary>
    static void DemonstrateInvalidInputRange()
    {
        Console.WriteLine("\n=== Validation: Invalid Input Range ===");

        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddInputNumber(i => i
                .WithId("qty")
                .WithLabel("Quantity")
                .WithMin(100)
                .WithMax(10))
            .Build();

        var issues = AdaptiveCardValidator.Validate(card);
        PrintIssues(issues);
    }

    /// <summary>
    /// Validates a card that uses elements requiring a newer schema version — expects version mismatch warnings.
    /// </summary>
    static void DemonstrateVersionMismatch()
    {
        Console.WriteLine("\n=== Validation: Version Mismatch ===");

        // Table requires v1.5; declaring v1.0 should trigger a VERSION_MISMATCH warning
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.0")
            .AddTextBlock(tb => tb.WithText("Sales Report").WithWeight(TextWeight.Bolder))
            .AddTable(table => table
                .AddColumn(new TableColumnDefinition { Width = "1" })
                .AddColumn(new TableColumnDefinition { Width = "1" })
                .AddRow(new TableRow
                {
                    Cells =
                    [
                        new TableCell { Items = [new TextBlock { Text = "Product" }] },
                        new TableCell { Items = [new TextBlock { Text = "Sales" }] }
                    ]
                }))
            .Build();

        var issues = AdaptiveCardValidator.Validate(card);
        PrintIssues(issues);
    }

    /// <summary>
    /// Demonstrates ValidateAndThrow — catches the exception thrown on validation errors.
    /// </summary>
    static void DemonstrateValidateAndThrow()
    {
        Console.WriteLine("\n=== Validation: ValidateAndThrow ===");

        var card = new AdaptiveCard { Version = "" };

        try
        {
            AdaptiveCardValidator.ValidateAndThrow(card);
            Console.WriteLine("No errors found.");
        }
        catch (AdaptiveCardValidationException ex)
        {
            Console.WriteLine($"Caught AdaptiveCardValidationException:");
            foreach (var error in ex.Errors)
            {
                Console.WriteLine($"  [{error.Code}] {error.Message}");
            }
        }
    }

    static void PrintIssues(IReadOnlyList<ValidationIssue> issues)
    {
        if (issues.Count == 0)
        {
            Console.WriteLine("✓ Card is valid — no issues found.");
            return;
        }

        Console.WriteLine($"Found {issues.Count} issue(s):");
        foreach (var issue in issues)
        {
            var icon = issue.Severity == ValidationSeverity.Error ? "✗" : "⚠";
            Console.WriteLine($"  {icon} [{issue.Severity}] {issue.Code} at '{issue.Path}': {issue.Message}");
        }
    }
}
