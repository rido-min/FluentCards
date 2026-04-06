from __future__ import annotations
from ..enums import ContainerStyle, HorizontalAlignment, VerticalAlignment, Spacing


class TableBuilder:
    def __init__(self):
        self._table: dict = {'type': 'Table', 'columns': [], 'rows': []}

    def with_id(self, id: str) -> TableBuilder:
        self._table['id'] = id
        return self

    def with_first_row_as_header(self, first_row_as_header: bool = True) -> TableBuilder:
        self._table['firstRowAsHeader'] = first_row_as_header
        return self

    def with_show_grid_lines(self, show_grid_lines: bool = True) -> TableBuilder:
        self._table['showGridLines'] = show_grid_lines
        return self

    def with_grid_style(self, grid_style: ContainerStyle) -> TableBuilder:
        self._table['gridStyle'] = grid_style.value
        return self

    def with_horizontal_cell_content_alignment(self, alignment: HorizontalAlignment) -> TableBuilder:
        self._table['horizontalCellContentAlignment'] = alignment.value
        return self

    def with_vertical_cell_content_alignment(self, alignment: VerticalAlignment) -> TableBuilder:
        self._table['verticalCellContentAlignment'] = alignment.value
        return self

    def with_spacing(self, spacing: Spacing) -> TableBuilder:
        self._table['spacing'] = spacing.value
        return self

    def add_column(self, column: dict) -> TableBuilder:
        self._table['columns'].append(column)
        return self

    def add_row(self, row: dict) -> TableBuilder:
        self._table['rows'].append(row)
        return self

    def build(self) -> dict:
        return self._table
