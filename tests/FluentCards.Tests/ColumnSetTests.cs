using System.Text.Json;
using Xunit;

namespace FluentCards.Tests;

public class ColumnSetTests
{
    [Fact]
    public void ColumnSetWithMultipleColumns_Serialization_ContainsAllColumns()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new ColumnSet
                {
                    Columns = new List<Column>
                    {
                        new Column
                        {
                            Items = new List<AdaptiveElement>
                            {
                                new TextBlock { Text = "Column 1" }
                            }
                        },
                        new Column
                        {
                            Items = new List<AdaptiveElement>
                            {
                                new TextBlock { Text = "Column 2" }
                            }
                        }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"ColumnSet\"", json);
        Assert.Contains("\"columns\":", json);
        Assert.Contains("\"text\": \"Column 1\"", json);
        Assert.Contains("\"text\": \"Column 2\"", json);
    }

    [Fact]
    public void ColumnWidthVariations_Serialization_PreservesAllFormats()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new ColumnSet
                {
                    Columns = new List<Column>
                    {
                        new Column { Width = "auto", Items = new List<AdaptiveElement>() },
                        new Column { Width = "stretch", Items = new List<AdaptiveElement>() },
                        new Column { Width = "1", Items = new List<AdaptiveElement>() },
                        new Column { Width = "2", Items = new List<AdaptiveElement>() },
                        new Column { Width = "50px", Items = new List<AdaptiveElement>() }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"width\": \"auto\"", json);
        Assert.Contains("\"width\": \"stretch\"", json);
        Assert.Contains("\"width\": \"1\"", json);
        Assert.Contains("\"width\": \"2\"", json);
        Assert.Contains("\"width\": \"50px\"", json);
    }

    [Fact]
    public void ColumnSetRoundtripSerialization_PreservesColumnOrder()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new ColumnSet
                {
                    Columns = new List<Column>
                    {
                        new Column
                        {
                            Width = "1",
                            Items = new List<AdaptiveElement>
                            {
                                new TextBlock { Text = "First" }
                            }
                        },
                        new Column
                        {
                            Width = "2",
                            Items = new List<AdaptiveElement>
                            {
                                new TextBlock { Text = "Second" }
                            }
                        },
                        new Column
                        {
                            Width = "1",
                            Items = new List<AdaptiveElement>
                            {
                                new TextBlock { Text = "Third" }
                            }
                        }
                    }
                }
            }
        };

        // Act
        var json = originalCard.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        Assert.NotNull(deserializedCard.Body);
        Assert.Single(deserializedCard.Body);

        var columnSet = deserializedCard.Body[0] as ColumnSet;
        Assert.NotNull(columnSet);
        Assert.NotNull(columnSet.Columns);
        Assert.Equal(3, columnSet.Columns.Count);

        Assert.Equal("1", columnSet.Columns[0].Width);
        Assert.NotNull(columnSet.Columns[0].Items);
        var first = columnSet.Columns[0].Items[0] as TextBlock;
        Assert.NotNull(first);
        Assert.Equal("First", first.Text);

        Assert.Equal("2", columnSet.Columns[1].Width);
        Assert.NotNull(columnSet.Columns[1].Items);
        var second = columnSet.Columns[1].Items[0] as TextBlock;
        Assert.NotNull(second);
        Assert.Equal("Second", second.Text);

        Assert.Equal("1", columnSet.Columns[2].Width);
        Assert.NotNull(columnSet.Columns[2].Items);
        var third = columnSet.Columns[2].Items[0] as TextBlock;
        Assert.NotNull(third);
        Assert.Equal("Third", third.Text);
    }

    [Fact]
    public void NestedElementsWithinColumns_Serialization_PreservesStructure()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new ColumnSet
                {
                    Columns = new List<Column>
                    {
                        new Column
                        {
                            Items = new List<AdaptiveElement>
                            {
                                new TextBlock { Text = "Text 1" },
                                new Image { Url = "https://example.com/img1.jpg" },
                                new TextBlock { Text = "Text 2" }
                            }
                        }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        Assert.NotNull(deserializedCard.Body);
        var columnSet = deserializedCard.Body[0] as ColumnSet;
        Assert.NotNull(columnSet);
        Assert.NotNull(columnSet.Columns);
        var column = columnSet.Columns[0];
        Assert.NotNull(column.Items);
        Assert.Equal(3, column.Items.Count);
        Assert.IsType<TextBlock>(column.Items[0]);
        Assert.IsType<Image>(column.Items[1]);
        Assert.IsType<TextBlock>(column.Items[2]);
    }

    [Fact]
    public void EmptyColumnsArray_Serialization_ProducesEmptyArray()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new ColumnSet { Columns = new List<Column>() }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"ColumnSet\"", json);
        Assert.Contains("\"columns\": []", json);
    }

    [Fact]
    public void ColumnSetWithAllProperties_Serialization_ContainsAllFields()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new ColumnSet
                {
                    Id = "columnSet1",
                    Style = ContainerStyle.Emphasis,
                    Bleed = true,
                    MinHeight = "200px",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Columns = new List<Column>()
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"id\": \"columnSet1\"", json);
        Assert.Contains("\"style\": \"emphasis\"", json);
        Assert.Contains("\"bleed\": true", json);
        Assert.Contains("\"minHeight\": \"200px\"", json);
        Assert.Contains("\"horizontalAlignment\": \"center\"", json);
    }

    [Fact]
    public void ColumnSetWithSelectAction_Serialization_ContainsAction()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new ColumnSet
                {
                    Columns = new List<Column>(),
                    SelectAction = new OpenUrlAction
                    {
                        Url = "https://example.com",
                        Title = "Click ColumnSet"
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"selectAction\":", json);
        Assert.Contains("\"type\": \"Action.OpenUrl\"", json);
    }

    [Fact]
    public void ColumnWithAllProperties_Serialization_ContainsAllFields()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new ColumnSet
                {
                    Columns = new List<Column>
                    {
                        new Column
                        {
                            Id = "col1",
                            Width = "auto",
                            Style = ContainerStyle.Attention,
                            VerticalContentAlignment = VerticalAlignment.Bottom,
                            Bleed = true,
                            MinHeight = "150px",
                            BackgroundImage = new BackgroundImage
                            {
                                Url = "https://example.com/bg.jpg"
                            },
                            Items = new List<AdaptiveElement>()
                        }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"id\": \"col1\"", json);
        Assert.Contains("\"width\": \"auto\"", json);
        Assert.Contains("\"style\": \"attention\"", json);
        Assert.Contains("\"verticalContentAlignment\": \"bottom\"", json);
        Assert.Contains("\"bleed\": true", json);
        Assert.Contains("\"minHeight\": \"150px\"", json);
        Assert.Contains("\"backgroundImage\":", json);
    }

    [Fact]
    public void ColumnWithNestedContainer_Serialization_HandlesComplexNesting()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new ColumnSet
                {
                    Columns = new List<Column>
                    {
                        new Column
                        {
                            Items = new List<AdaptiveElement>
                            {
                                new Container
                                {
                                    Items = new List<AdaptiveElement>
                                    {
                                        new TextBlock { Text = "Nested in Container in Column" }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        Assert.NotNull(deserializedCard.Body);
        var columnSet = deserializedCard.Body[0] as ColumnSet;
        Assert.NotNull(columnSet);
        Assert.NotNull(columnSet.Columns);
        var column = columnSet.Columns[0];
        Assert.NotNull(column.Items);
        var container = column.Items[0] as Container;
        Assert.NotNull(container);
        Assert.NotNull(container.Items);
        var textBlock = container.Items[0] as TextBlock;
        Assert.NotNull(textBlock);
        Assert.Equal("Nested in Container in Column", textBlock.Text);
    }

    [Fact]
    public void ColumnSetWithNullColumns_Serialization_DoesNotIncludeColumns()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new ColumnSet { Columns = null }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"ColumnSet\"", json);
        Assert.DoesNotContain("\"columns\":", json);
    }

    [Fact]
    public void ColumnWithNullItems_Serialization_DoesNotIncludeItems()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new ColumnSet
                {
                    Columns = new List<Column>
                    {
                        new Column { Width = "1", Items = null }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"width\": \"1\"", json);
        Assert.DoesNotContain("\"items\":", json);
    }

    [Fact]
    public void PolymorphicDeserialization_ContainerWithMixedElements_DeserializesCorrectly()
    {
        // Arrange
        var jsonString = @"{
            ""type"": ""AdaptiveCard"",
            ""version"": ""1.5"",
            ""body"": [
                {
                    ""type"": ""Container"",
                    ""items"": [
                        {
                            ""type"": ""TextBlock"",
                            ""text"": ""Text Element""
                        },
                        {
                            ""type"": ""Image"",
                            ""url"": ""https://example.com/img.jpg""
                        }
                    ]
                }
            ]
        }";

        // Act
        var card = AdaptiveCardExtensions.FromJson(jsonString);

        // Assert
        Assert.NotNull(card);
        Assert.NotNull(card.Body);
        var container = card.Body[0] as Container;
        Assert.NotNull(container);
        Assert.NotNull(container.Items);
        Assert.Equal(2, container.Items.Count);
        Assert.IsType<TextBlock>(container.Items[0]);
        Assert.IsType<Image>(container.Items[1]);
    }

    [Fact]
    public void PolymorphicDeserialization_ColumnSetFromJson_DeserializesCorrectly()
    {
        // Arrange
        var jsonString = @"{
            ""type"": ""AdaptiveCard"",
            ""version"": ""1.5"",
            ""body"": [
                {
                    ""type"": ""ColumnSet"",
                    ""columns"": [
                        {
                            ""width"": ""auto"",
                            ""items"": [
                                {
                                    ""type"": ""TextBlock"",
                                    ""text"": ""Left Column""
                                }
                            ]
                        },
                        {
                            ""width"": ""stretch"",
                            ""items"": [
                                {
                                    ""type"": ""TextBlock"",
                                    ""text"": ""Right Column""
                                }
                            ]
                        }
                    ]
                }
            ]
        }";

        // Act
        var card = AdaptiveCardExtensions.FromJson(jsonString);

        // Assert
        Assert.NotNull(card);
        Assert.NotNull(card.Body);
        var columnSet = card.Body[0] as ColumnSet;
        Assert.NotNull(columnSet);
        Assert.NotNull(columnSet.Columns);
        Assert.Equal(2, columnSet.Columns.Count);
        Assert.Equal("auto", columnSet.Columns[0].Width);
        Assert.Equal("stretch", columnSet.Columns[1].Width);
    }
}
