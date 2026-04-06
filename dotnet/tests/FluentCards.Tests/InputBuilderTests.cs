using Xunit;

namespace FluentCards.Tests;

public class InputBuilderTests
{
    [Fact]
    public void InputTextBuilder_CreatesInputText()
    {
        // Arrange & Act
        var input = new InputTextBuilder()
            .WithId("name")
            .WithLabel("Name")
            .WithPlaceholder("Enter your name")
            .Build();

        // Assert
        Assert.NotNull(input);
        Assert.Equal("name", input.Id);
        Assert.Equal("Name", input.Label);
        Assert.Equal("Enter your name", input.Placeholder);
    }

    [Fact]
    public void InputTextBuilder_AllProperties_SetCorrectly()
    {
        // Arrange & Act
        var input = new InputTextBuilder()
            .WithId("email")
            .WithLabel("Email")
            .WithPlaceholder("you@example.com")
            .WithValue("test@test.com")
            .WithMaxLength(100)
            .IsMultiline(false)
            .WithStyle(TextInputStyle.Email)
            .WithRegex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")
            .IsRequired(true)
            .WithErrorMessage("Please enter a valid email")
            .Build();

        // Assert
        Assert.Equal("email", input.Id);
        Assert.Equal("Email", input.Label);
        Assert.Equal("you@example.com", input.Placeholder);
        Assert.Equal("test@test.com", input.Value);
        Assert.Equal(100, input.MaxLength);
        Assert.False(input.IsMultiline);
        Assert.Equal(TextInputStyle.Email, input.Style);
        Assert.Equal(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", input.Regex);
        Assert.True(input.IsRequired);
        Assert.Equal("Please enter a valid email", input.ErrorMessage);
    }

    [Fact]
    public void InputNumberBuilder_CreatesInputNumber()
    {
        // Arrange & Act
        var input = new InputNumberBuilder()
            .WithId("age")
            .WithLabel("Age")
            .WithValue(25)
            .WithMin(0)
            .WithMax(120)
            .IsRequired(true)
            .Build();

        // Assert
        Assert.Equal("age", input.Id);
        Assert.Equal("Age", input.Label);
        Assert.Equal(25, input.Value);
        Assert.Equal(0, input.Min);
        Assert.Equal(120, input.Max);
        Assert.True(input.IsRequired);
    }

    [Fact]
    public void InputDateBuilder_CreatesInputDate()
    {
        // Arrange & Act
        var input = new InputDateBuilder()
            .WithId("birthdate")
            .WithLabel("Birth Date")
            .WithValue("2000-01-01")
            .WithMin("1900-01-01")
            .WithMax("2023-12-31")
            .IsRequired(true)
            .Build();

        // Assert
        Assert.Equal("birthdate", input.Id);
        Assert.Equal("Birth Date", input.Label);
        Assert.Equal("2000-01-01", input.Value);
        Assert.Equal("1900-01-01", input.Min);
        Assert.Equal("2023-12-31", input.Max);
        Assert.True(input.IsRequired);
    }

    [Fact]
    public void InputTimeBuilder_CreatesInputTime()
    {
        // Arrange & Act
        var input = new InputTimeBuilder()
            .WithId("appointmentTime")
            .WithLabel("Appointment Time")
            .WithValue("14:30")
            .WithMin("09:00")
            .WithMax("17:00")
            .Build();

        // Assert
        Assert.Equal("appointmentTime", input.Id);
        Assert.Equal("Appointment Time", input.Label);
        Assert.Equal("14:30", input.Value);
        Assert.Equal("09:00", input.Min);
        Assert.Equal("17:00", input.Max);
    }

    [Fact]
    public void InputToggleBuilder_CreatesInputToggle()
    {
        // Arrange & Act
        var input = new InputToggleBuilder()
            .WithId("newsletter")
            .WithLabel("Newsletter")
            .WithTitle("Subscribe to newsletter")
            .WithValueOn("yes")
            .WithValueOff("no")
            .WithValue("yes")
            .IsRequired(false)
            .WithWrap(true)
            .Build();

        // Assert
        Assert.Equal("newsletter", input.Id);
        Assert.Equal("Newsletter", input.Label);
        Assert.Equal("Subscribe to newsletter", input.Title);
        Assert.Equal("yes", input.ValueOn);
        Assert.Equal("no", input.ValueOff);
        Assert.Equal("yes", input.Value);
        Assert.False(input.IsRequired);
        Assert.True(input.Wrap);
    }

    [Fact]
    public void InputChoiceSetBuilder_CreatesInputChoiceSet()
    {
        // Arrange & Act
        var input = new InputChoiceSetBuilder()
            .WithId("color")
            .WithLabel("Favorite Color")
            .WithStyle(ChoiceInputStyle.Compact)
            .AddChoice("Red", "red")
            .AddChoice("Green", "green")
            .AddChoice("Blue", "blue")
            .WithValue("red")
            .IsRequired(true)
            .Build();

        // Assert
        Assert.Equal("color", input.Id);
        Assert.Equal("Favorite Color", input.Label);
        Assert.Equal(ChoiceInputStyle.Compact, input.Style);
        Assert.NotNull(input.Choices);
        Assert.Equal(3, input.Choices.Count);
        Assert.Equal("Red", input.Choices[0].Title);
        Assert.Equal("red", input.Choices[0].Value);
        Assert.Equal("red", input.Value);
        Assert.True(input.IsRequired);
    }

    [Fact]
    public void InputChoiceSetBuilder_MultiSelect_Works()
    {
        // Arrange & Act
        var input = new InputChoiceSetBuilder()
            .WithId("hobbies")
            .IsMultiSelect(true)
            .WithStyle(ChoiceInputStyle.Expanded)
            .AddChoice("Reading", "reading")
            .AddChoice("Gaming", "gaming")
            .AddChoice("Sports", "sports")
            .Build();

        // Assert
        Assert.Equal("hobbies", input.Id);
        Assert.True(input.IsMultiSelect);
        Assert.Equal(ChoiceInputStyle.Expanded, input.Style);
        Assert.Equal(3, input.Choices!.Count);
    }

    [Fact]
    public void InputChoiceSetBuilder_WithChoicesData_String_SetsDataQuery()
    {
        // Arrange & Act
        var input = new InputChoiceSetBuilder()
            .WithId("people-picker")
            .WithLabel("Select users")
            .IsMultiSelect()
            .WithChoicesData("graph.microsoft.com/users")
            .Build();

        // Assert
        Assert.NotNull(input.ChoicesData);
        Assert.Equal("Data.Query", input.ChoicesData.Type);
        Assert.Equal("graph.microsoft.com/users", input.ChoicesData.Dataset);
    }

    [Fact]
    public void InputChoiceSetBuilder_WithChoicesData_Delegate_ConfiguresDataQuery()
    {
        // Arrange & Act
        var input = new InputChoiceSetBuilder()
            .WithId("people-picker")
            .WithChoicesData(dq =>
            {
                dq.Dataset = "graph.microsoft.com/users";
                dq.Count = 25;
                dq.Skip = 0;
            })
            .Build();

        // Assert
        Assert.NotNull(input.ChoicesData);
        Assert.Equal("graph.microsoft.com/users", input.ChoicesData.Dataset);
        Assert.Equal(25, input.ChoicesData.Count);
        Assert.Equal(0, input.ChoicesData.Skip);
    }

    [Fact]
    public void AdaptiveCardBuilder_AddInputs_AddsToBody()
    {
        // Arrange & Act
        var card = AdaptiveCardBuilder.Create()
            .AddInputText(i => i.WithId("name").WithLabel("Name"))
            .AddInputNumber(i => i.WithId("age").WithLabel("Age"))
            .AddInputDate(i => i.WithId("date").WithLabel("Date"))
            .AddInputTime(i => i.WithId("time").WithLabel("Time"))
            .AddInputToggle(i => i.WithId("toggle").WithTitle("Accept"))
            .AddInputChoiceSet(i => i.WithId("choice").AddChoice("A", "a"))
            .Build();

        // Assert
        Assert.NotNull(card.Body);
        Assert.Equal(6, card.Body.Count);
        Assert.IsType<InputText>(card.Body[0]);
        Assert.IsType<InputNumber>(card.Body[1]);
        Assert.IsType<InputDate>(card.Body[2]);
        Assert.IsType<InputTime>(card.Body[3]);
        Assert.IsType<InputToggle>(card.Body[4]);
        Assert.IsType<InputChoiceSet>(card.Body[5]);
    }
}
