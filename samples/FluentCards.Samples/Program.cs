using FluentCards;

Console.WriteLine("=== FluentCards Demo ===\n");

// Create a card using the fluent builder pattern
var card = AdaptiveCardBuilder.Create()
    .WithVersion("1.5")
    .AddTextBlock(tb => tb
        .WithText("Hello, FluentCards!")
        .WithSize(TextSize.Large)
        .WithWeight(TextWeight.Bolder)
        .WithWrap(true))
    .AddTextBlock(tb => tb
        .WithText("This card was built with a fluent interface.")
        .WithColor(TextColor.Accent))
    .AddAction(a => a
        .OpenUrl("https://adaptivecards.io")
        .WithTitle("Learn More"))
    .Build();

// Serialize to JSON
var json = card.ToJson();
Console.WriteLine(json);

// Demonstrate roundtrip serialization
Console.WriteLine("\n=== Roundtrip Test ===");
var deserializedCard = AdaptiveCardExtensions.FromJson(json);
if (deserializedCard != null)
{
    Console.WriteLine($"✓ Successfully deserialized card");
    Console.WriteLine($"  Version: {deserializedCard.Version}");
    Console.WriteLine($"  Body elements: {deserializedCard.Body?.Count ?? 0}");
    Console.WriteLine($"  Actions: {deserializedCard.Actions?.Count ?? 0}");
}
