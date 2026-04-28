namespace FluentCards.Tests;

/// <summary>
/// Tests that ActionBuilder methods throw InvalidOperationException when called
/// on incompatible action types (issue #66).
/// </summary>
public class ActionBuilderTypeGuardTests
{
    // ── WithData (JsonElement) on wrong types ──────────────────────────

    [Theory]
    [InlineData("OpenUrl")]
    [InlineData("ShowCard")]
    [InlineData("ToggleVisibility")]
    public void WithData_JsonElement_ThrowsOnIncompatibleType(string actionType)
    {
        using var doc = System.Text.Json.JsonDocument.Parse("""{"key":"value"}""");
        var element = doc.RootElement.Clone();

        var builder = CreateBuilderWithType(actionType);
        var ex = Assert.Throws<InvalidOperationException>(() => builder.WithData(element));

        Assert.Contains("WithData()", ex.Message);
        Assert.Contains("Submit or Execute", ex.Message);
    }

    [Theory]
    [InlineData("Submit")]
    [InlineData("Execute")]
    public void WithData_JsonElement_WorksOnCompatibleType(string actionType)
    {
        using var doc = System.Text.Json.JsonDocument.Parse("""{"key":"value"}""");
        var element = doc.RootElement.Clone();

        var builder = CreateBuilderWithType(actionType);
        var result = builder.WithData(element);
        Assert.NotNull(result);
    }

    // ── WithData (string) on wrong types ───────────────────────────────

    [Theory]
    [InlineData("OpenUrl")]
    [InlineData("ShowCard")]
    [InlineData("ToggleVisibility")]
    public void WithData_String_ThrowsOnIncompatibleType(string actionType)
    {
        var builder = CreateBuilderWithType(actionType);
        var ex = Assert.Throws<InvalidOperationException>(() => builder.WithData("""{"key":"value"}"""));

        Assert.Contains("WithData()", ex.Message);
        Assert.Contains("Submit or Execute", ex.Message);
    }

    [Theory]
    [InlineData("Submit")]
    [InlineData("Execute")]
    public void WithData_String_WorksOnCompatibleType(string actionType)
    {
        var builder = CreateBuilderWithType(actionType);
        var result = builder.WithData("""{"key":"value"}""");
        Assert.NotNull(result);
    }

    // ── WithVerb on wrong types ────────────────────────────────────────

    [Theory]
    [InlineData("OpenUrl")]
    [InlineData("Submit")]
    [InlineData("ShowCard")]
    [InlineData("ToggleVisibility")]
    public void WithVerb_ThrowsOnNonExecuteType(string actionType)
    {
        var builder = CreateBuilderWithType(actionType);
        var ex = Assert.Throws<InvalidOperationException>(() => builder.WithVerb("doStuff"));

        Assert.Contains("WithVerb()", ex.Message);
        Assert.Contains("Execute actions", ex.Message);
    }

    [Fact]
    public void WithVerb_WorksOnExecute()
    {
        var builder = new ActionBuilder().Execute("Run");
        var result = builder.WithVerb("doStuff");
        Assert.NotNull(result);

        var action = result.Build();
        Assert.IsType<ExecuteAction>(action);
        Assert.Equal("doStuff", ((ExecuteAction)action).Verb);
    }

    // ── WithAssociatedInputs on wrong types ────────────────────────────

    [Theory]
    [InlineData("OpenUrl")]
    [InlineData("ShowCard")]
    [InlineData("ToggleVisibility")]
    public void WithAssociatedInputs_ThrowsOnIncompatibleType(string actionType)
    {
        var builder = CreateBuilderWithType(actionType);
        var ex = Assert.Throws<InvalidOperationException>(
            () => builder.WithAssociatedInputs(AssociatedInputs.Auto));

        Assert.Contains("WithAssociatedInputs()", ex.Message);
        Assert.Contains("Submit or Execute", ex.Message);
    }

    [Theory]
    [InlineData("Submit")]
    [InlineData("Execute")]
    public void WithAssociatedInputs_WorksOnCompatibleType(string actionType)
    {
        var builder = CreateBuilderWithType(actionType);
        var result = builder.WithAssociatedInputs(AssociatedInputs.Auto);
        Assert.NotNull(result);
    }

    // ── Exception message format verification ──────────────────────────

    [Fact]
    public void WithData_ExceptionMessage_MatchesExpectedFormat()
    {
        var builder = new ActionBuilder().OpenUrl("https://example.com");
        var ex = Assert.Throws<InvalidOperationException>(
            () => builder.WithData("""{"x":1}"""));

        Assert.Equal(
            "WithData() is only available on Submit or Execute actions. Call Submit() or Execute() before using this method.",
            ex.Message);
    }

    [Fact]
    public void WithVerb_ExceptionMessage_MatchesExpectedFormat()
    {
        var builder = new ActionBuilder().Submit();
        var ex = Assert.Throws<InvalidOperationException>(
            () => builder.WithVerb("action"));

        Assert.Equal(
            "WithVerb() is only available on Execute actions. Call Execute() before using this method.",
            ex.Message);
    }

    [Fact]
    public void WithAssociatedInputs_ExceptionMessage_MatchesExpectedFormat()
    {
        var builder = new ActionBuilder().OpenUrl("https://example.com");
        var ex = Assert.Throws<InvalidOperationException>(
            () => builder.WithAssociatedInputs(AssociatedInputs.Auto));

        Assert.Equal(
            "WithAssociatedInputs() is only available on Submit or Execute actions. Call Submit() or Execute() before using this method.",
            ex.Message);
    }

    // ── Helper ─────────────────────────────────────────────────────────

    private static ActionBuilder CreateBuilderWithType(string actionType) => actionType switch
    {
        "OpenUrl" => new ActionBuilder().OpenUrl("https://example.com"),
        "Submit" => new ActionBuilder().Submit(),
        "ShowCard" => new ActionBuilder().ShowCard(),
        "ToggleVisibility" => new ActionBuilder().ToggleVisibility(),
        "Execute" => new ActionBuilder().Execute(),
        _ => throw new System.ArgumentException($"Unknown action type: {actionType}")
    };
}
