using Xunit;

namespace FluentCards.Tests;

public class ColumnSetBuilderTests
{
    [Fact]
    public void ColumnSetBuilder_CreatesColumnSet()
    {
        // Arrange & Act
        var columnSet = new ColumnSetBuilder()
            .WithId("columns1")
            .Build();

        // Assert
        Assert.NotNull(columnSet);
        Assert.Equal("columns1", columnSet.Id);
        Assert.NotNull(columnSet.Columns);
    }

    [Fact]
    public void ColumnSetBuilder_AllProperties_SetCorrectly()
    {
        // Arrange & Act
        var columnSet = new ColumnSetBuilder()
            .WithId("columns1")
            .WithStyle(ContainerStyle.Accent)
            .WithBleed(true)
            .WithMinHeight("50px")
            .WithHorizontalAlignment(HorizontalAlignment.Center)
            .Build();

        // Assert
        Assert.Equal("columns1", columnSet.Id);
        Assert.Equal(ContainerStyle.Accent, columnSet.Style);
        Assert.True(columnSet.Bleed);
        Assert.Equal("50px", columnSet.MinHeight);
        Assert.Equal(HorizontalAlignment.Center, columnSet.HorizontalAlignment);
    }

    [Fact]
    public void ColumnSetBuilder_AddColumn_AddsToColumns()
    {
        // Arrange & Act
        var columnSet = new ColumnSetBuilder()
            .AddColumn(c => c
                .WithWidth("auto")
                .AddTextBlock(tb => tb.WithText("Col1")))
            .AddColumn(c => c
                .WithWidth("stretch")
                .AddTextBlock(tb => tb.WithText("Col2")))
            .Build();

        // Assert
        Assert.NotNull(columnSet.Columns);
        Assert.Equal(2, columnSet.Columns.Count);
        Assert.Equal("auto", columnSet.Columns[0].Width);
        Assert.Equal("stretch", columnSet.Columns[1].Width);
    }

    [Fact]
    public void ColumnSetBuilder_AddColumnWithWidth_SetsWidth()
    {
        // Arrange & Act
        var columnSet = new ColumnSetBuilder()
            .AddColumn("100px", c => c.AddTextBlock(tb => tb.WithText("Fixed width")))
            .Build();

        // Assert
        Assert.NotNull(columnSet.Columns);
        Assert.Single(columnSet.Columns);
        Assert.Equal("100px", columnSet.Columns[0].Width);
    }

    [Fact]
    public void ColumnBuilder_CreatesColumn()
    {
        // Arrange & Act
        var column = new ColumnBuilder()
            .WithWidth("auto")
            .WithId("col1")
            .Build();

        // Assert
        Assert.NotNull(column);
        Assert.Equal("auto", column.Width);
        Assert.Equal("col1", column.Id);
        Assert.NotNull(column.Items);
    }

    [Fact]
    public void ColumnBuilder_AllProperties_SetCorrectly()
    {
        // Arrange & Act
        var column = new ColumnBuilder()
            .WithWidth("2")
            .WithId("col1")
            .WithStyle(ContainerStyle.Good)
            .WithVerticalContentAlignment(VerticalAlignment.Bottom)
            .WithBleed(true)
            .WithMinHeight("75px")
            .Build();

        // Assert
        Assert.Equal("2", column.Width);
        Assert.Equal("col1", column.Id);
        Assert.Equal(ContainerStyle.Good, column.Style);
        Assert.Equal(VerticalAlignment.Bottom, column.VerticalContentAlignment);
        Assert.True(column.Bleed);
        Assert.Equal("75px", column.MinHeight);
    }

    [Fact]
    public void ColumnBuilder_AddElements_AddsToItems()
    {
        // Arrange & Act
        var column = new ColumnBuilder()
            .AddTextBlock(tb => tb.WithText("Text"))
            .AddImage(i => i.WithUrl("https://example.com/img.jpg"))
            .AddContainer(c => c.WithId("nested"))
            .Build();

        // Assert
        Assert.NotNull(column.Items);
        Assert.Equal(3, column.Items.Count);
        Assert.IsType<TextBlock>(column.Items[0]);
        Assert.IsType<Image>(column.Items[1]);
        Assert.IsType<Container>(column.Items[2]);
    }

    [Fact]
    public void AdaptiveCardBuilder_AddColumnSet_AddsColumnSetToBody()
    {
        // Arrange & Act
        var card = AdaptiveCardBuilder.Create()
            .AddColumnSet(cs => cs
                .AddColumn(c => c.AddTextBlock(tb => tb.WithText("Left")))
                .AddColumn(c => c.AddTextBlock(tb => tb.WithText("Right"))))
            .Build();

        // Assert
        Assert.NotNull(card.Body);
        Assert.Single(card.Body);
        var columnSet = card.Body[0] as ColumnSet;
        Assert.NotNull(columnSet);
        Assert.Equal(2, columnSet.Columns!.Count);
    }
}
