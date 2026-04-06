from __future__ import annotations
from ..enums import ContainerStyle, HorizontalAlignment, VerticalAlignment, Spacing


class TableBuilder:
    """Fluent builder for creating Table elements."""

    def __init__(self):
        self._table: dict = {'type': 'Table', 'columns': [], 'rows': []}

    def with_id(self, id: str) -> TableBuilder:
        """Sets the unique identifier for the Table.

        Args:
            id: The unique identifier.

        Returns:
            The builder instance for method chaining.
        """
        self._table['id'] = id
        return self

    def with_first_row_as_header(self, first_row_as_header: bool = True) -> TableBuilder:
        """Sets whether the first row is treated as a header row.

        Args:
            first_row_as_header: True to style the first row as a header.

        Returns:
            The builder instance for method chaining.
        """
        self._table['firstRowAsHeader'] = first_row_as_header
        return self

    def with_show_grid_lines(self, show_grid_lines: bool = True) -> TableBuilder:
        """Sets whether grid lines are shown between cells.

        Args:
            show_grid_lines: True to display grid lines.

        Returns:
            The builder instance for method chaining.
        """
        self._table['showGridLines'] = show_grid_lines
        return self

    def with_grid_style(self, grid_style: ContainerStyle) -> TableBuilder:
        """Sets the grid style of the table.

        Args:
            grid_style: The container style to apply to the grid.

        Returns:
            The builder instance for method chaining.
        """
        self._table['gridStyle'] = grid_style.value
        return self

    def with_horizontal_cell_content_alignment(self, alignment: HorizontalAlignment) -> TableBuilder:
        """Sets the default horizontal alignment for cell content.

        Args:
            alignment: The horizontal alignment.

        Returns:
            The builder instance for method chaining.
        """
        self._table['horizontalCellContentAlignment'] = alignment.value
        return self

    def with_vertical_cell_content_alignment(self, alignment: VerticalAlignment) -> TableBuilder:
        """Sets the default vertical alignment for cell content.

        Args:
            alignment: The vertical alignment.

        Returns:
            The builder instance for method chaining.
        """
        self._table['verticalCellContentAlignment'] = alignment.value
        return self

    def with_spacing(self, spacing: Spacing) -> TableBuilder:
        """Sets the spacing before the table.

        Args:
            spacing: The spacing value.

        Returns:
            The builder instance for method chaining.
        """
        self._table['spacing'] = spacing.value
        return self

    def add_column(self, column: dict) -> TableBuilder:
        """Adds a column definition to the table.

        Args:
            column: A pre-built TableColumnDefinition dictionary.

        Returns:
            The builder instance for method chaining.
        """
        self._table['columns'].append(column)
        return self

    def add_row(self, row: dict) -> TableBuilder:
        """Adds a row to the table.

        Args:
            row: A pre-built TableRow dictionary.

        Returns:
            The builder instance for method chaining.
        """
        self._table['rows'].append(row)
        return self

    def build(self) -> dict:
        """Builds and returns the configured Table dictionary.

        Returns:
            The configured Table dictionary.
        """
        return self._table
