using System.Text.Json;
using Xunit;

namespace FluentCards.Tests;

public class TableTests
{
    [Fact]
    public void Table_WithColumnsAndRows_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Table
                {
                    Columns = new List<TableColumnDefinition>
                    {
                        new TableColumnDefinition { Width = "auto" },
                        new TableColumnDefinition { Width = "100px" }
                    },
                    Rows = new List<TableRow>
                    {
                        new TableRow
                        {
                            Cells = new List<TableCell>
                            {
                                new TableCell { Items = new List<AdaptiveElement> { new TextBlock { Text = "Cell 1" } } },
                                new TableCell { Items = new List<AdaptiveElement> { new TextBlock { Text = "Cell 2" } } }
                            }
                        }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"Table\"", json);
        Assert.Contains("\"columns\":", json);
        Assert.Contains("\"rows\":", json);
        Assert.Contains("\"width\": \"auto\"", json);
        Assert.Contains("\"width\": \"100px\"", json);
        Assert.Contains("Cell 1", json);
        Assert.Contains("Cell 2", json);
    }

    [Fact]
    public void Table_WithFirstRowAsHeader_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Table
                {
                    FirstRowAsHeader = true,
                    ShowGridLines = true,
                    Rows = new List<TableRow>
                    {
                        new TableRow
                        {
                            Cells = new List<TableCell>
                            {
                                new TableCell { Items = new List<AdaptiveElement> { new TextBlock { Text = "Header" } } }
                            }
                        }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"firstRowAsHeader\": true", json);
        Assert.Contains("\"showGridLines\": true", json);
    }

    [Fact]
    public void TableCell_WithNestedElements_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Table
                {
                    Rows = new List<TableRow>
                    {
                        new TableRow
                        {
                            Cells = new List<TableCell>
                            {
                                new TableCell
                                {
                                    Items = new List<AdaptiveElement>
                                    {
                                        new TextBlock { Text = "Title", Weight = TextWeight.Bolder },
                                        new TextBlock { Text = "Description", Size = TextSize.Small },
                                        new FactSet
                                        {
                                            Facts = new List<Fact>
                                            {
                                                new Fact { Title = "Key", Value = "Value" }
                                            }
                                        }
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
        var table = deserializedCard.Body![0] as Table;
        Assert.NotNull(table);
        Assert.NotNull(table.Rows);
        var cell = table.Rows[0].Cells![0];
        Assert.NotNull(cell.Items);
        Assert.Equal(3, cell.Items.Count);
        Assert.IsType<TextBlock>(cell.Items[0]);
        Assert.IsType<TextBlock>(cell.Items[1]);
        Assert.IsType<FactSet>(cell.Items[2]);
    }

    [Fact]
    public void Table_ColumnWidthVariations_SerializeCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Table
                {
                    Columns = new List<TableColumnDefinition>
                    {
                        new TableColumnDefinition { Width = "auto" },
                        new TableColumnDefinition { Width = "50px" },
                        new TableColumnDefinition { Width = "100" }
                    },
                    Rows = new List<TableRow>
                    {
                        new TableRow
                        {
                            Cells = new List<TableCell>
                            {
                                new TableCell { Items = new List<AdaptiveElement> { new TextBlock { Text = "A" } } },
                                new TableCell { Items = new List<AdaptiveElement> { new TextBlock { Text = "B" } } },
                                new TableCell { Items = new List<AdaptiveElement> { new TextBlock { Text = "C" } } }
                            }
                        }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"width\": \"auto\"", json);
        Assert.Contains("\"width\": \"50px\"", json);
        Assert.Contains("\"width\": \"100\"", json);
    }

    [Fact]
    public void Table_WithEmptyTable_HandlesGracefully()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Table
                {
                    Columns = new List<TableColumnDefinition>(),
                    Rows = new List<TableRow>()
                }
            }
        };

        // Act
        var json = card.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        var table = deserializedCard.Body![0] as Table;
        Assert.NotNull(table);
        Assert.NotNull(table.Columns);
        Assert.Empty(table.Columns);
        Assert.NotNull(table.Rows);
        Assert.Empty(table.Rows);
    }

    [Fact]
    public void Table_RoundtripSerialization_PreservesTableStructure()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Table
                {
                    Id = "table1",
                    FirstRowAsHeader = true,
                    ShowGridLines = true,
                    GridStyle = ContainerStyle.Emphasis,
                    HorizontalCellContentAlignment = HorizontalAlignment.Center,
                    VerticalCellContentAlignment = VerticalAlignment.Center,
                    Columns = new List<TableColumnDefinition>
                    {
                        new TableColumnDefinition
                        {
                            Width = "auto",
                            HorizontalCellContentAlignment = HorizontalAlignment.Left
                        },
                        new TableColumnDefinition
                        {
                            Width = "200px",
                            VerticalCellContentAlignment = VerticalAlignment.Top
                        }
                    },
                    Rows = new List<TableRow>
                    {
                        new TableRow
                        {
                            Style = ContainerStyle.Accent,
                            Cells = new List<TableCell>
                            {
                                new TableCell { Items = new List<AdaptiveElement> { new TextBlock { Text = "Header 1" } } },
                                new TableCell { Items = new List<AdaptiveElement> { new TextBlock { Text = "Header 2" } } }
                            }
                        },
                        new TableRow
                        {
                            Cells = new List<TableCell>
                            {
                                new TableCell { Items = new List<AdaptiveElement> { new TextBlock { Text = "Data 1" } } },
                                new TableCell { Items = new List<AdaptiveElement> { new TextBlock { Text = "Data 2" } } }
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
        var table = deserializedCard.Body![0] as Table;
        Assert.NotNull(table);
        Assert.Equal("table1", table.Id);
        Assert.True(table.FirstRowAsHeader);
        Assert.True(table.ShowGridLines);
        Assert.Equal(ContainerStyle.Emphasis, table.GridStyle);
        Assert.Equal(HorizontalAlignment.Center, table.HorizontalCellContentAlignment);
        Assert.Equal(VerticalAlignment.Center, table.VerticalCellContentAlignment);
        Assert.NotNull(table.Columns);
        Assert.Equal(2, table.Columns.Count);
        Assert.NotNull(table.Rows);
        Assert.Equal(2, table.Rows.Count);
    }

    [Fact]
    public void TableCell_WithAllProperties_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Table
                {
                    Rows = new List<TableRow>
                    {
                        new TableRow
                        {
                            Cells = new List<TableCell>
                            {
                                new TableCell
                                {
                                    Items = new List<AdaptiveElement> { new TextBlock { Text = "Test" } },
                                    Style = ContainerStyle.Good,
                                    VerticalContentAlignment = VerticalAlignment.Bottom,
                                    Bleed = true,
                                    MinHeight = "50px",
                                    Rtl = true,
                                    BackgroundImage = new BackgroundImage { Url = "https://example.com/bg.jpg" }
                                }
                            }
                        }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"style\": \"good\"", json);
        Assert.Contains("\"verticalContentAlignment\": \"bottom\"", json);
        Assert.Contains("\"bleed\": true", json);
        Assert.Contains("\"minHeight\": \"50px\"", json);
        Assert.Contains("\"rtl\": true", json);
        Assert.Contains("\"backgroundImage\":", json);
    }

    [Fact]
    public void Table_WithComplexNestedStructure_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Table
                {
                    Rows = new List<TableRow>
                    {
                        new TableRow
                        {
                            Cells = new List<TableCell>
                            {
                                new TableCell
                                {
                                    Items = new List<AdaptiveElement>
                                    {
                                        new TextBlock { Text = "Header" },
                                        new FactSet
                                        {
                                            Facts = new List<Fact>
                                            {
                                                new Fact { Title = "Key1", Value = "Value1" }
                                            }
                                        },
                                        new ActionSet
                                        {
                                            Actions = new List<AdaptiveAction>
                                            {
                                                new SubmitAction { Title = "Submit" }
                                            }
                                        }
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
        var table = deserializedCard.Body![0] as Table;
        Assert.NotNull(table);
        var cell = table.Rows![0].Cells![0];
        Assert.NotNull(cell.Items);
        Assert.Equal(3, cell.Items.Count);
        Assert.IsType<TextBlock>(cell.Items[0]);
        Assert.IsType<FactSet>(cell.Items[1]);
        Assert.IsType<ActionSet>(cell.Items[2]);
    }

    [Fact]
    public void Table_AlignmentEnumValues_SerializeCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Table
                {
                    HorizontalCellContentAlignment = HorizontalAlignment.Right,
                    VerticalCellContentAlignment = VerticalAlignment.Bottom,
                    Rows = new List<TableRow>
                    {
                        new TableRow
                        {
                            HorizontalCellContentAlignment = HorizontalAlignment.Center,
                            VerticalCellContentAlignment = VerticalAlignment.Top,
                            Cells = new List<TableCell>
                            {
                                new TableCell { Items = new List<AdaptiveElement> { new TextBlock { Text = "Test" } } }
                            }
                        }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"horizontalCellContentAlignment\": \"right\"", json);
        Assert.Contains("\"verticalCellContentAlignment\": \"bottom\"", json);
        Assert.Contains("\"horizontalCellContentAlignment\": \"center\"", json);
        Assert.Contains("\"verticalCellContentAlignment\": \"top\"", json);
    }
}
