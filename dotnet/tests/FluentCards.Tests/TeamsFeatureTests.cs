using System.Text.Json;

namespace FluentCards.Tests;

/// <summary>
/// Tests for Microsoft Teams-specific features: card-level, action-level, and data-level msteams extensions.
/// </summary>
public class TeamsFeatureTests
{
    // ── Model & Serialization Tests ──

    [Fact]
    public void TeamsCardProperties_SerializesWidthFull()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithTeamsCard(t => t.WithFullWidth())
            .AddTextBlock(tb => tb.WithText("Hello"))
            .Build();

        var json = card.ToJson();
        using var doc = JsonDocument.Parse(json);
        var msteams = doc.RootElement.GetProperty("msteams");

        Assert.Equal("Full", msteams.GetProperty("width").GetString());
    }

    [Fact]
    public void TeamsCardProperties_OmitsWidthWhenNull()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithTeamsCard(t => t.AddMention("John", "29:123"))
            .AddTextBlock(tb => tb.WithText("<at>John</at>"))
            .Build();

        var json = card.ToJson();
        using var doc = JsonDocument.Parse(json);
        var msteams = doc.RootElement.GetProperty("msteams");

        Assert.False(msteams.TryGetProperty("width", out _));
    }

    [Fact]
    public void TeamsCardProperties_OmittedWhenNull()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddTextBlock(tb => tb.WithText("Hello"))
            .Build();

        var json = card.ToJson();
        using var doc = JsonDocument.Parse(json);

        Assert.False(doc.RootElement.TryGetProperty("msteams", out _));
    }

    [Fact]
    public void TeamsCardProperties_SerializesMentions()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithTeamsCard(t => t.AddMention("John Doe", "29:1241241"))
            .AddTextBlock(tb => tb.WithText("Hello <at>John Doe</at>!"))
            .Build();

        var json = card.ToJson();
        using var doc = JsonDocument.Parse(json);
        var entities = doc.RootElement.GetProperty("msteams").GetProperty("entities");

        Assert.Equal(1, entities.GetArrayLength());
        var mention = entities[0];
        Assert.Equal("mention", mention.GetProperty("type").GetString());
        Assert.Equal("<at>John Doe</at>", mention.GetProperty("text").GetString());
        Assert.Equal("29:1241241", mention.GetProperty("mentioned").GetProperty("id").GetString());
        Assert.Equal("John Doe", mention.GetProperty("mentioned").GetProperty("name").GetString());
    }

    [Fact]
    public void TeamsSubmitActionProperties_SerializesFeedback()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddTextBlock(tb => tb.WithText("Test"))
            .AddAction(a => a
                .Submit("Silent Submit")
                .WithTeamsSubmitFeedback(ts => ts.WithFeedbackHidden()))
            .Build();

        var json = card.ToJson();
        using var doc = JsonDocument.Parse(json);
        var action = doc.RootElement.GetProperty("actions")[0];
        var msteams = action.GetProperty("msteams");

        Assert.True(msteams.GetProperty("feedback").GetProperty("hide").GetBoolean());
    }

    [Fact]
    public void TeamsData_SerializesMsteamsAndProperties()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddTextBlock(tb => tb.WithText("Test"))
            .AddAction(a => a
                .Submit("Task Module")
                .WithTeamsData(td => td
                    .WithTaskFetch()
                    .WithProperty("customField", "customValue")))
            .Build();

        var json = card.ToJson();
        using var doc = JsonDocument.Parse(json);
        var data = doc.RootElement.GetProperty("actions")[0].GetProperty("data");

        Assert.Equal("task/fetch", data.GetProperty("msteams").GetProperty("type").GetString());
        Assert.Equal("customValue", data.GetProperty("customField").GetString());
    }

    [Fact]
    public void TeamsData_SerializesMsteamsOnly()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddTextBlock(tb => tb.WithText("Test"))
            .AddAction(a => a
                .Submit("Task Module")
                .WithTeamsTaskFetch())
            .Build();

        var json = card.ToJson();
        using var doc = JsonDocument.Parse(json);
        var data = doc.RootElement.GetProperty("actions")[0].GetProperty("data");

        Assert.Equal("task/fetch", data.GetProperty("msteams").GetProperty("type").GetString());
        // Should only have msteams, no other keys
        Assert.Equal(1, data.EnumerateObject().Count());
    }

    [Fact]
    public void TeamsData_SerializesPropertiesOnly()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddTextBlock(tb => tb.WithText("Test"))
            .AddAction(a => a
                .Submit("Feedback")
                .WithTeamsData(td => td
                    .WithProperty("message", "Button Clicked")
                    .WithProperty("source", "card")))
            .Build();

        var json = card.ToJson();
        using var doc = JsonDocument.Parse(json);
        var data = doc.RootElement.GetProperty("actions")[0].GetProperty("data");

        Assert.Equal("Button Clicked", data.GetProperty("message").GetString());
        Assert.Equal("card", data.GetProperty("source").GetString());
        Assert.False(data.TryGetProperty("msteams", out _));
    }

    // ── Builder Tests ──

    [Fact]
    public void WithTeamsCard_FullWidth_SetsCardWidth()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithTeamsCard(t => t.WithFullWidth())
            .AddTextBlock(tb => tb.WithText("Test"))
            .Build();

        Assert.NotNull(card.Msteams);
        Assert.Equal(TeamsCardWidth.Full, card.Msteams.Width);
    }

    [Fact]
    public void AddMention_AutoGeneratesAtText()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithTeamsCard(t => t.AddMention("John", "29:123"))
            .AddTextBlock(tb => tb.WithText("<at>John</at>"))
            .Build();

        Assert.NotNull(card.Msteams?.Entities);
        Assert.Single(card.Msteams.Entities);
        Assert.Equal("<at>John</at>", card.Msteams.Entities[0].Text);
        Assert.Equal("29:123", card.Msteams.Entities[0].Mentioned.Id);
        Assert.Equal("John", card.Msteams.Entities[0].Mentioned.Name);
    }

    [Fact]
    public void AddMention_MultipleMentions()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithTeamsCard(t => t
                .AddMention("Alice", "29:111")
                .AddMention("Bob", "29:222"))
            .AddTextBlock(tb => tb.WithText("<at>Alice</at> and <at>Bob</at>"))
            .Build();

        Assert.Equal(2, card.Msteams!.Entities!.Count);
        Assert.Equal("<at>Alice</at>", card.Msteams.Entities[0].Text);
        Assert.Equal("<at>Bob</at>", card.Msteams.Entities[1].Text);
    }

    [Fact]
    public void WithTeamsTaskFetch_SetsDataCorrectly()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddTextBlock(tb => tb.WithText("Test"))
            .AddAction(a => a
                .Submit("Open Module")
                .WithTeamsTaskFetch())
            .Build();

        var json = card.ToJson();
        using var doc = JsonDocument.Parse(json);
        var data = doc.RootElement.GetProperty("actions")[0].GetProperty("data");

        Assert.Equal("task/fetch", data.GetProperty("msteams").GetProperty("type").GetString());
    }

    [Fact]
    public void WithTeamsData_TaskFetchAndCustom()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddTextBlock(tb => tb.WithText("Test"))
            .AddAction(a => a
                .Submit("Module")
                .WithTeamsData(td => td
                    .WithTaskFetch()
                    .WithProperty("customField", "customValue")))
            .Build();

        var json = card.ToJson();
        using var doc = JsonDocument.Parse(json);
        var data = doc.RootElement.GetProperty("actions")[0].GetProperty("data");

        Assert.Equal("task/fetch", data.GetProperty("msteams").GetProperty("type").GetString());
        Assert.Equal("customValue", data.GetProperty("customField").GetString());
    }

    [Fact]
    public void WithTeamsData_CustomPropertiesOnly()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddTextBlock(tb => tb.WithText("Test"))
            .AddAction(a => a
                .Submit("Submit")
                .WithTeamsData(td => td
                    .WithProperty("key1", "val1")
                    .WithProperty("key2", 42)
                    .WithProperty("key3", true)))
            .Build();

        var json = card.ToJson();
        using var doc = JsonDocument.Parse(json);
        var data = doc.RootElement.GetProperty("actions")[0].GetProperty("data");

        Assert.Equal("val1", data.GetProperty("key1").GetString());
        Assert.Equal(42, data.GetProperty("key2").GetInt32());
        Assert.True(data.GetProperty("key3").GetBoolean());
    }

    [Fact]
    public void WithTeamsSubmitFeedback_FeedbackHidden()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddTextBlock(tb => tb.WithText("Test"))
            .AddAction(a => a
                .Submit("Silent")
                .WithTeamsSubmitFeedback(ts => ts.WithFeedbackHidden()))
            .Build();

        var action = card.Actions![0] as SubmitAction;
        Assert.NotNull(action?.Msteams);
        Assert.True(action.Msteams.Feedback?.Hide);
    }

    // ── Submit-Only Gating Tests ──

    [Fact]
    public void WithTeamsTaskFetch_OnExecute_ThrowsImmediately()
    {
        var builder = new ActionBuilder();
        builder.Execute("Test Action");

        Assert.Throws<InvalidOperationException>(() => builder.WithTeamsTaskFetch());
    }

    [Fact]
    public void WithTeamsData_OnExecute_ThrowsImmediately()
    {
        var builder = new ActionBuilder();
        builder.Execute("Test Action");

        Assert.Throws<InvalidOperationException>(() =>
            builder.WithTeamsData(td => td.WithTaskFetch()));
    }

    [Fact]
    public void WithTeamsSubmitFeedback_OnExecute_ThrowsImmediately()
    {
        var builder = new ActionBuilder();
        builder.Execute("Test Action");

        Assert.Throws<InvalidOperationException>(() =>
            builder.WithTeamsSubmitFeedback(ts => ts.WithFeedbackHidden()));
    }

    [Fact]
    public void WithTeamsTaskFetch_OnOpenUrl_ThrowsImmediately()
    {
        var builder = new ActionBuilder();
        builder.OpenUrl("https://example.com");

        Assert.Throws<InvalidOperationException>(() => builder.WithTeamsTaskFetch());
    }

    [Fact]
    public void WithTeamsSubmitRaw_OnExecute_ThrowsImmediately()
    {
        var builder = new ActionBuilder();
        builder.Execute("Test");

        Assert.Throws<InvalidOperationException>(() =>
            builder.WithTeamsSubmitRaw("""{"feedback":{"hide":true}}"""));
    }

    // ── Conflict Detection Tests ──

    [Fact]
    public void WithData_And_WithTeamsData_ThrowsImmediately()
    {
        var builder = new ActionBuilder();
        builder.Submit("Test");
        builder.WithTeamsData(td => td.WithTaskFetch());

        Assert.Throws<InvalidOperationException>(() =>
            builder.WithData("""{"key":"value"}"""));
    }

    [Fact]
    public void WithData_And_WithTeamsTaskFetch_ThrowsImmediately()
    {
        var builder = new ActionBuilder();
        builder.Submit("Test");
        builder.WithData("""{"key":"value"}""");

        Assert.Throws<InvalidOperationException>(() => builder.WithTeamsTaskFetch());
    }

    [Fact]
    public void WithTeamsCard_And_WithTeamsCardRaw_Throws()
    {
        var builder = AdaptiveCardBuilder.Create();
        builder.WithTeamsCard(t => t.WithFullWidth());

        Assert.Throws<InvalidOperationException>(() =>
            builder.WithTeamsCardRaw("""{"width":"Full"}"""));
    }

    [Fact]
    public void WithTeamsCardRaw_And_WithTeamsCard_Throws()
    {
        var builder = AdaptiveCardBuilder.Create();
        builder.WithTeamsCardRaw("""{"width":"Full"}""");

        Assert.Throws<InvalidOperationException>(() =>
            builder.WithTeamsCard(t => t.WithFullWidth()));
    }

    [Fact]
    public void WithTeamsSubmitFeedback_And_WithTeamsSubmitRaw_Throws()
    {
        var builder = new ActionBuilder();
        builder.Submit("Test");
        builder.WithTeamsSubmitFeedback(ts => ts.WithFeedbackHidden());

        Assert.Throws<InvalidOperationException>(() =>
            builder.WithTeamsSubmitRaw("""{"feedback":{"hide":true}}"""));
    }

    [Fact]
    public void WithTeamsSubmitRaw_And_WithTeamsSubmitFeedback_Throws()
    {
        var builder = new ActionBuilder();
        builder.Submit("Test");
        builder.WithTeamsSubmitRaw("""{"feedback":{"hide":true}}""");

        Assert.Throws<InvalidOperationException>(() =>
            builder.WithTeamsSubmitFeedback(ts => ts.WithFeedbackHidden()));
    }

    [Fact]
    public void TeamsDataBuilder_RejectsMsteamsKey()
    {
        var builder = new TeamsDataBuilder();

        Assert.Throws<ArgumentException>(() => builder.WithProperty("msteams", "value"));
    }

    [Fact]
    public void TeamsDataBuilder_RejectsMsteamsKey_CaseInsensitive()
    {
        var builder = new TeamsDataBuilder();

        Assert.Throws<ArgumentException>(() => builder.WithProperty("MsTeams", "value"));
    }

    // ── Validation Tests ──

    [Fact]
    public void Validator_WarnsOrphanedMentionEntity()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithTeamsCard(t => t.AddMention("John", "29:123"))
            .AddTextBlock(tb => tb.WithText("Hello world")) // no <at>John</at>
            .Build();

        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Contains(issues, i => i.Code == "ORPHANED_MENTION_ENTITY");
    }

    [Fact]
    public void Validator_WarnsOrphanedAtToken()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddTextBlock(tb => tb.WithText("Hello <at>John</at>")) // no mention entity
            .Build();

        // No msteams entities at all — validator should not crash
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.DoesNotContain(issues, i => i.Code == "ORPHANED_AT_TOKEN");

        // Now with msteams but wrong entity
        var card2 = AdaptiveCardBuilder.Create()
            .WithTeamsCard(t => t.AddMention("Alice", "29:456"))
            .AddTextBlock(tb => tb.WithText("Hello <at>John</at>"))
            .Build();

        var issues2 = AdaptiveCardValidator.Validate(card2);
        Assert.Contains(issues2, i => i.Code == "ORPHANED_AT_TOKEN");
    }

    [Fact]
    public void Validator_PassesMatchedMentions()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithTeamsCard(t => t
                .AddMention("John", "29:123")
                .AddMention("Alice", "29:456"))
            .AddTextBlock(tb => tb.WithText("Hello <at>John</at> and <at>Alice</at>"))
            .Build();

        var issues = AdaptiveCardValidator.Validate(card);
        Assert.DoesNotContain(issues, i => i.Code == "ORPHANED_MENTION_ENTITY");
        Assert.DoesNotContain(issues, i => i.Code == "ORPHANED_AT_TOKEN");
    }

    // ── Extensibility Tests ──

    [Fact]
    public void WithTeamsCardRaw_String_PassesThrough()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithTeamsCardRaw("""{"width":"Full"}""")
            .AddTextBlock(tb => tb.WithText("Test"))
            .Build();

        var json = card.ToJson();
        using var doc = JsonDocument.Parse(json);
        var msteams = doc.RootElement.GetProperty("msteams");

        Assert.Equal("Full", msteams.GetProperty("width").GetString());
    }

    [Fact]
    public void WithTeamsCardRaw_JsonElement_PassesThrough()
    {
        using var rawDoc = JsonDocument.Parse("""{"width":"Full","someNewProp":"value"}""");
        var card = AdaptiveCardBuilder.Create()
            .WithTeamsCardRaw(rawDoc.RootElement)
            .AddTextBlock(tb => tb.WithText("Test"))
            .Build();

        Assert.NotNull(card.Msteams);
        Assert.Equal(TeamsCardWidth.Full, card.Msteams.Width);
    }

    [Fact]
    public void WithTeamsSubmitRaw_SerializesCorrectly()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddTextBlock(tb => tb.WithText("Test"))
            .AddAction(a => a
                .Submit("Silent")
                .WithTeamsSubmitRaw("""{"feedback":{"hide":true}}"""))
            .Build();

        var json = card.ToJson();
        using var doc = JsonDocument.Parse(json);
        var action = doc.RootElement.GetProperty("actions")[0];

        Assert.True(action.GetProperty("msteams").GetProperty("feedback").GetProperty("hide").GetBoolean());
    }

    [Fact]
    public void WithTeamsCardRaw_RejectsNonObject()
    {
        var builder = AdaptiveCardBuilder.Create();

        Assert.Throws<ArgumentException>(() => builder.WithTeamsCardRaw(@"""hello"""));
    }

    [Fact]
    public void TeamsDataBuilder_WithMsteams_RejectsNonObject()
    {
        var builder = new TeamsDataBuilder();

        Assert.Throws<ArgumentException>(() => builder.WithMsteams(@"""hello"""));
    }

    // ── Integration Tests ──

    [Fact]
    public void FullTeamsCard_MatchesExpectedJson()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .WithTeamsCard(teams => teams
                .WithFullWidth()
                .AddMention("John Doe", "29:1241241"))
            .AddTextBlock(t => t.WithText("Hello <at>John Doe</at>!"))
            .AddAction(a => a
                .Submit("Open Task Module")
                .WithTeamsTaskFetch())
            .AddAction(a => a
                .Submit("Silent Submit")
                .WithTeamsSubmitFeedback(ts => ts.WithFeedbackHidden()))
            .Build();

        var json = card.ToJson();
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        // Card-level msteams
        var msteams = root.GetProperty("msteams");
        Assert.Equal("Full", msteams.GetProperty("width").GetString());
        Assert.Equal(1, msteams.GetProperty("entities").GetArrayLength());

        // Task/fetch action
        var actions = root.GetProperty("actions");
        var taskFetchData = actions[0].GetProperty("data");
        Assert.Equal("task/fetch", taskFetchData.GetProperty("msteams").GetProperty("type").GetString());

        // Silent submit action
        var silentSubmit = actions[1];
        Assert.True(silentSubmit.GetProperty("msteams").GetProperty("feedback").GetProperty("hide").GetBoolean());
    }

    [Fact]
    public void ExistingCards_IdenticalOutput()
    {
        // Ensure cards without Teams features are unaffected
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb.WithText("Hello"))
            .AddAction(a => a.Submit("OK").WithData("""{"key":"value"}"""))
            .Build();

        var json = card.ToJson();
        using var doc = JsonDocument.Parse(json);

        Assert.False(doc.RootElement.TryGetProperty("msteams", out _));
        Assert.Equal("Action.Submit", doc.RootElement.GetProperty("actions")[0].GetProperty("type").GetString());
    }

    // ── AOT / Property Type Tests ──

    [Fact]
    public void TeamsDataBuilder_AllPropertyTypes_SerializeCorrectly()
    {
        var builder = new TeamsDataBuilder();
        builder.WithProperty("strProp", "hello");
        builder.WithProperty("intProp", 42);
        builder.WithProperty("boolProp", true);

        using var extraDoc = JsonDocument.Parse("""{"nested":"value"}""");
        builder.WithProperty("jsonProp", extraDoc.RootElement);

        var result = builder.Build();
        using var doc = JsonDocument.Parse(result.GetRawText());

        Assert.Equal("hello", doc.RootElement.GetProperty("strProp").GetString());
        Assert.Equal(42, doc.RootElement.GetProperty("intProp").GetInt32());
        Assert.True(doc.RootElement.GetProperty("boolProp").GetBoolean());
        Assert.Equal("value", doc.RootElement.GetProperty("jsonProp").GetProperty("nested").GetString());
    }

    [Fact]
    public void WithTeamsData_WithMsteamsRawJson()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddTextBlock(tb => tb.WithText("Test"))
            .AddAction(a => a
                .Submit("Custom")
                .WithTeamsData(td => td
                    .WithMsteams("""{"type":"task/fetch","custom":"prop"}""")
                    .WithProperty("extra", "data")))
            .Build();

        var json = card.ToJson();
        using var doc = JsonDocument.Parse(json);
        var data = doc.RootElement.GetProperty("actions")[0].GetProperty("data");

        Assert.Equal("task/fetch", data.GetProperty("msteams").GetProperty("type").GetString());
        Assert.Equal("prop", data.GetProperty("msteams").GetProperty("custom").GetString());
        Assert.Equal("data", data.GetProperty("extra").GetString());
    }
}
