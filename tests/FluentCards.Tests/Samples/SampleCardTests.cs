using FluentCards.Samples;
using Xunit;

namespace FluentCards.Tests.Samples;

public class SampleCardTests
{
    [Fact]
    public void BasicCardSample_CreateWelcomeCard_ProducesValidCard()
    {
        // Act
        var card = BasicCardSample.CreateWelcomeCard();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        Assert.NotEmpty(card.Body);
        Assert.NotNull(card.Actions);
        Assert.NotEmpty(card.Actions);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void BasicCardSample_CreateNotificationCard_ProducesValidCard()
    {
        // Act
        var card = BasicCardSample.CreateNotificationCard();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void BasicCardSample_CreateImageCard_ProducesValidCard()
    {
        // Act
        var card = BasicCardSample.CreateImageCard();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        Assert.Contains(card.Body, e => e is Image);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void FormCardSample_CreateContactForm_ProducesValidCard()
    {
        // Act
        var card = FormCardSample.CreateContactForm();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        Assert.Contains(card.Body, e => e is InputText);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void FormCardSample_CreateSurveyForm_ProducesValidCard()
    {
        // Act
        var card = FormCardSample.CreateSurveyForm();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        Assert.Contains(card.Body, e => e is InputChoiceSet);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void FormCardSample_CreateRegistrationForm_ProducesValidCard()
    {
        // Act
        var card = FormCardSample.CreateRegistrationForm();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void LayoutCardSample_CreateTwoColumnCard_ProducesValidCard()
    {
        // Act
        var card = LayoutCardSample.CreateTwoColumnCard();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        Assert.Contains(card.Body, e => e is ColumnSet);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void LayoutCardSample_CreateStyledContainerCard_ProducesValidCard()
    {
        // Act
        var card = LayoutCardSample.CreateStyledContainerCard();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        Assert.Contains(card.Body, e => e is Container);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void LayoutCardSample_CreateFactSetCard_ProducesValidCard()
    {
        // Act
        var card = LayoutCardSample.CreateFactSetCard();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        Assert.Contains(card.Body, e => e is FactSet);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void LayoutCardSample_CreateNestedContainerCard_ProducesValidCard()
    {
        // Act
        var card = LayoutCardSample.CreateNestedContainerCard();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void RichContentSample_CreateRichTextCard_ProducesValidCard()
    {
        // Act
        var card = RichContentSample.CreateRichTextCard();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        Assert.Contains(card.Body, e => e is RichTextBlock);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void RichContentSample_CreateImageSetCard_ProducesValidCard()
    {
        // Act
        var card = RichContentSample.CreateImageSetCard();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        Assert.Contains(card.Body, e => e is ImageSet);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void RichContentSample_CreateTableCard_ProducesValidCard()
    {
        // Act
        var card = RichContentSample.CreateTableCard();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        Assert.Contains(card.Body, e => e is Table);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void RichContentSample_CreateMediaCard_ProducesValidCard()
    {
        // Act
        var card = RichContentSample.CreateMediaCard();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        Assert.Contains(card.Body, e => e is Media);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void RichContentSample_CreateComprehensiveCard_ProducesValidCard()
    {
        // Act
        var card = RichContentSample.CreateComprehensiveCard();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void AllSamples_SerializeToValidJson()
    {
        // Arrange
        var cards = new[]
        {
            BasicCardSample.CreateWelcomeCard(),
            BasicCardSample.CreateNotificationCard(),
            BasicCardSample.CreateImageCard(),
            FormCardSample.CreateContactForm(),
            FormCardSample.CreateSurveyForm(),
            FormCardSample.CreateRegistrationForm(),
            LayoutCardSample.CreateTwoColumnCard(),
            LayoutCardSample.CreateStyledContainerCard(),
            LayoutCardSample.CreateFactSetCard(),
            LayoutCardSample.CreateNestedContainerCard(),
            RichContentSample.CreateRichTextCard(),
            RichContentSample.CreateImageSetCard(),
            RichContentSample.CreateTableCard(),
            RichContentSample.CreateMediaCard(),
            RichContentSample.CreateComprehensiveCard()
        };

        // Act & Assert
        foreach (var card in cards)
        {
            var json = card.ToJson();
            Assert.NotNull(json);
            Assert.NotEmpty(json);
            
            // Verify it can be deserialized back
            var deserializedCard = AdaptiveCardExtensions.FromJson(json);
            Assert.NotNull(deserializedCard);
        }
    }
}
