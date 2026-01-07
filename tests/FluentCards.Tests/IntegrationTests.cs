using Xunit;
using System.Text.Json;

namespace FluentCards.Tests;

public class IntegrationTests
{
    [Fact]
    public void ComplexCard_BuildsCorrectly()
    {
        // Arrange & Act
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb
                .WithText("Welcome to FluentCards")
                .WithSize(TextSize.ExtraLarge)
                .WithWeight(TextWeight.Bolder))
            .AddImage(img => img
                .WithUrl("https://example.com/logo.jpg")
                .WithSize(ImageSize.Medium)
                .WithHorizontalAlignment(HorizontalAlignment.Center))
            .AddContainer(c => c
                .WithStyle(ContainerStyle.Emphasis)
                .AddTextBlock(tb => tb.WithText("User Information"))
                .AddFactSet(fs => fs
                    .AddFact("Name", "John Doe")
                    .AddFact("Email", "john@example.com")
                    .AddFact("Role", "Developer")))
            .AddColumnSet(cs => cs
                .AddColumn("auto", col => col
                    .AddTextBlock(tb => tb.WithText("Left Column")))
                .AddColumn("stretch", col => col
                    .AddTextBlock(tb => tb.WithText("Right Column"))))
            .AddInputText(i => i
                .WithId("comment")
                .WithLabel("Comment")
                .WithPlaceholder("Enter your comment")
                .IsMultiline(true)
                .IsRequired(true))
            .AddAction(a => a
                .Submit("Submit")
                .WithStyle(ActionStyle.Positive))
            .Build();

        // Assert
        Assert.NotNull(card);
        Assert.NotNull(card.Body);
        Assert.Equal(5, card.Body.Count);
        Assert.NotNull(card.Actions);
        Assert.Single(card.Actions);

        // Verify specific elements
        var textBlock = card.Body[0] as TextBlock;
        Assert.NotNull(textBlock);
        Assert.Equal("Welcome to FluentCards", textBlock.Text);

        var image = card.Body[1] as Image;
        Assert.NotNull(image);
        Assert.Equal(ImageSize.Medium, image.Size);

        var container = card.Body[2] as Container;
        Assert.NotNull(container);
        Assert.Equal(ContainerStyle.Emphasis, container.Style);
        Assert.Equal(2, container.Items!.Count);

        var columnSet = card.Body[3] as ColumnSet;
        Assert.NotNull(columnSet);
        Assert.Equal(2, columnSet.Columns!.Count);

        var input = card.Body[4] as InputText;
        Assert.NotNull(input);
        Assert.Equal("comment", input.Id);
        Assert.True(input.IsRequired);
    }

    [Fact]
    public void ComplexCard_SerializesAndDeserializes()
    {
        // Arrange
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddContainer(c => c
                .WithId("container1")
                .WithStyle(ContainerStyle.Emphasis)
                .AddTextBlock(tb => tb.WithText("Hello"))
                .AddImage(i => i.WithUrl("https://example.com/img.jpg")))
            .AddColumnSet(cs => cs
                .WithId("columns1")
                .AddColumn(col => col
                    .WithWidth("auto")
                    .AddTextBlock(tb => tb.WithText("Col1")))
                .AddColumn(col => col
                    .WithWidth("stretch")
                    .AddTextBlock(tb => tb.WithText("Col2"))))
            .AddInputText(i => i
                .WithId("input1")
                .WithLabel("Name")
                .IsRequired(true))
            .Build();

        // Act
        var json = JsonSerializer.Serialize(card, FluentCardsJsonContext.Default.AdaptiveCard);
        var deserialized = JsonSerializer.Deserialize<AdaptiveCard>(json, FluentCardsJsonContext.Default.AdaptiveCard);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(card.Version, deserialized.Version);
        Assert.Equal(card.Body!.Count, deserialized.Body!.Count);

        // Verify container
        var container = deserialized.Body[0] as Container;
        Assert.NotNull(container);
        Assert.Equal("container1", container.Id);
        Assert.Equal(ContainerStyle.Emphasis, container.Style);
        Assert.Equal(2, container.Items!.Count);

        // Verify column set
        var columnSet = deserialized.Body[1] as ColumnSet;
        Assert.NotNull(columnSet);
        Assert.Equal("columns1", columnSet.Id);
        Assert.Equal(2, columnSet.Columns!.Count);

        // Verify input
        var input = deserialized.Body[2] as InputText;
        Assert.NotNull(input);
        Assert.Equal("input1", input.Id);
        Assert.True(input.IsRequired);
    }

    [Fact]
    public void NestedContainers_BuildAndSerializeCorrectly()
    {
        // Arrange & Act
        var card = AdaptiveCardBuilder.Create()
            .AddContainer(c1 => c1
                .WithId("outer")
                .AddContainer(c2 => c2
                    .WithId("middle")
                    .AddContainer(c3 => c3
                        .WithId("inner")
                        .AddTextBlock(tb => tb.WithText("Deeply nested")))))
            .Build();

        var json = JsonSerializer.Serialize(card, FluentCardsJsonContext.Default.AdaptiveCard);
        var deserialized = JsonSerializer.Deserialize<AdaptiveCard>(json, FluentCardsJsonContext.Default.AdaptiveCard);

        // Assert
        Assert.NotNull(deserialized);
        var outer = deserialized.Body![0] as Container;
        Assert.NotNull(outer);
        Assert.Equal("outer", outer.Id);

        var middle = outer.Items![0] as Container;
        Assert.NotNull(middle);
        Assert.Equal("middle", middle.Id);

        var inner = middle.Items![0] as Container;
        Assert.NotNull(inner);
        Assert.Equal("inner", inner.Id);

        var textBlock = inner.Items![0] as TextBlock;
        Assert.NotNull(textBlock);
        Assert.Equal("Deeply nested", textBlock.Text);
    }

    [Fact]
    public void CardWithRefreshAndAuthentication_BuildsCorrectly()
    {
        // Arrange & Act
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb.WithText("Protected Content"))
            .WithRefresh(r => r
                .WithAction(a => a.Execute("refresh"))
                .AddUserId("user1")
                .AddUserId("user2")
                .WithExpires("2024-12-31T23:59:59Z"))
            .WithAuthentication(auth => auth
                .WithText("Please sign in")
                .WithConnectionName("oauth2"))
            .WithMetadata("https://example.com/card")
            .Build();

        // Assert
        Assert.NotNull(card);
        Assert.NotNull(card.Refresh);
        Assert.NotNull(card.Refresh.Action);
        Assert.NotNull(card.Refresh.UserIds);
        Assert.Equal(2, card.Refresh.UserIds.Count);
        Assert.Equal("2024-12-31T23:59:59Z", card.Refresh.Expires);

        Assert.NotNull(card.Authentication);
        Assert.Equal("Please sign in", card.Authentication.Text);
        Assert.Equal("oauth2", card.Authentication.ConnectionName);

        Assert.NotNull(card.Metadata);
        Assert.Equal("https://example.com/card", card.Metadata.WebUrl);
    }

    [Fact]
    public void FormCard_AllInputTypes_BuildsCorrectly()
    {
        // Arrange & Act
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb
                .WithText("User Registration Form")
                .WithSize(TextSize.Large)
                .WithWeight(TextWeight.Bolder))
            .AddInputText(i => i
                .WithId("name")
                .WithLabel("Full Name")
                .IsRequired(true))
            .AddInputText(i => i
                .WithId("email")
                .WithLabel("Email")
                .WithStyle(TextInputStyle.Email)
                .IsRequired(true))
            .AddInputNumber(i => i
                .WithId("age")
                .WithLabel("Age")
                .WithMin(18)
                .WithMax(100))
            .AddInputDate(i => i
                .WithId("birthdate")
                .WithLabel("Birth Date"))
            .AddInputTime(i => i
                .WithId("preferredTime")
                .WithLabel("Preferred Contact Time"))
            .AddInputToggle(i => i
                .WithId("terms")
                .WithTitle("I agree to terms and conditions")
                .WithValueOn("accepted")
                .WithValueOff("declined")
                .IsRequired(true))
            .AddInputChoiceSet(i => i
                .WithId("country")
                .WithLabel("Country")
                .WithStyle(ChoiceInputStyle.Compact)
                .AddChoice("USA", "us")
                .AddChoice("Canada", "ca")
                .AddChoice("UK", "uk"))
            .AddAction(a => a
                .Submit("Register")
                .WithStyle(ActionStyle.Positive))
            .Build();

        // Assert
        Assert.NotNull(card);
        Assert.Equal(8, card.Body!.Count);
        Assert.Single(card.Actions!);

        // Verify all input types are present
        Assert.IsType<TextBlock>(card.Body[0]);
        Assert.IsType<InputText>(card.Body[1]);
        Assert.IsType<InputText>(card.Body[2]);
        Assert.IsType<InputNumber>(card.Body[3]);
        Assert.IsType<InputDate>(card.Body[4]);
        Assert.IsType<InputTime>(card.Body[5]);
        Assert.IsType<InputToggle>(card.Body[6]);
        Assert.IsType<InputChoiceSet>(card.Body[7]);
    }
}
