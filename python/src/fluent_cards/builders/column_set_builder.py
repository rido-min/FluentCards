from __future__ import annotations
from typing import Callable, Optional, Union
from ..enums import ContainerStyle, HorizontalAlignment, Spacing


class ColumnSetBuilder:
    def __init__(self):
        self._column_set: dict = {'type': 'ColumnSet', 'columns': []}

    def with_id(self, id: str) -> ColumnSetBuilder:
        self._column_set['id'] = id
        return self

    def with_style(self, style: ContainerStyle) -> ColumnSetBuilder:
        self._column_set['style'] = style.value
        return self

    def with_bleed(self, bleed: bool = True) -> ColumnSetBuilder:
        self._column_set['bleed'] = bleed
        return self

    def with_min_height(self, min_height: str) -> ColumnSetBuilder:
        self._column_set['minHeight'] = min_height
        return self

    def with_horizontal_alignment(self, alignment: HorizontalAlignment) -> ColumnSetBuilder:
        self._column_set['horizontalAlignment'] = alignment.value
        return self

    def with_spacing(self, spacing: Spacing) -> ColumnSetBuilder:
        self._column_set['spacing'] = spacing.value
        return self

    def with_separator(self, separator: bool) -> ColumnSetBuilder:
        self._column_set['separator'] = separator
        return self

    def with_select_action(self, configure: Callable) -> ColumnSetBuilder:
        from .action_builder import ActionBuilder
        b = ActionBuilder()
        configure(b)
        self._column_set['selectAction'] = b.build()
        return self

    def add_column(self, configure_or_width: Union[str, Callable],
                   configure: Optional[Callable] = None) -> ColumnSetBuilder:
        from .column_builder import ColumnBuilder
        b = ColumnBuilder()
        if isinstance(configure_or_width, str):
            b.with_width(configure_or_width)
            configure(b)
        else:
            configure_or_width(b)
        self._column_set['columns'].append(b.build())
        return self

    def build(self) -> dict:
        return self._column_set
