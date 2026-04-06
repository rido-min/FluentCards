from __future__ import annotations
from typing import Callable, Optional, Union
from ..enums import ContainerStyle, HorizontalAlignment, Spacing


class ColumnSetBuilder:
    """Fluent builder for creating ColumnSet elements."""

    def __init__(self):
        self._column_set: dict = {'type': 'ColumnSet', 'columns': []}

    def with_id(self, id: str) -> ColumnSetBuilder:
        """Sets the unique identifier for the ColumnSet.

        Args:
            id: The unique identifier.

        Returns:
            The builder instance for method chaining.
        """
        self._column_set['id'] = id
        return self

    def with_style(self, style: ContainerStyle) -> ColumnSetBuilder:
        """Sets the visual style of the column set.

        Args:
            style: The container style.

        Returns:
            The builder instance for method chaining.
        """
        self._column_set['style'] = style.value
        return self

    def with_bleed(self, bleed: bool = True) -> ColumnSetBuilder:
        """Sets whether the column set bleeds into its parent padding.

        Args:
            bleed: True to enable bleed.

        Returns:
            The builder instance for method chaining.
        """
        self._column_set['bleed'] = bleed
        return self

    def with_min_height(self, min_height: str) -> ColumnSetBuilder:
        """Sets the minimum height of the column set.

        Args:
            min_height: The minimum height value (e.g. '100px').

        Returns:
            The builder instance for method chaining.
        """
        self._column_set['minHeight'] = min_height
        return self

    def with_horizontal_alignment(self, alignment: HorizontalAlignment) -> ColumnSetBuilder:
        """Sets the horizontal alignment of the column set.

        Args:
            alignment: The horizontal alignment.

        Returns:
            The builder instance for method chaining.
        """
        self._column_set['horizontalAlignment'] = alignment.value
        return self

    def with_spacing(self, spacing: Spacing) -> ColumnSetBuilder:
        """Sets the spacing before the column set.

        Args:
            spacing: The spacing value.

        Returns:
            The builder instance for method chaining.
        """
        self._column_set['spacing'] = spacing.value
        return self

    def with_separator(self, separator: bool) -> ColumnSetBuilder:
        """Sets whether a separator line is drawn above the column set.

        Args:
            separator: True to draw a separator line above the element.

        Returns:
            The builder instance for method chaining.
        """
        self._column_set['separator'] = separator
        return self

    def with_select_action(self, configure: Callable) -> ColumnSetBuilder:
        """Sets the action invoked when the column set is selected.

        Args:
            configure: A callable that receives an ActionBuilder to configure the action.

        Returns:
            The builder instance for method chaining.
        """
        from .action_builder import ActionBuilder
        b = ActionBuilder()
        configure(b)
        self._column_set['selectAction'] = b.build()
        return self

    def add_column(self, configure_or_width: Union[str, Callable],
                   configure: Optional[Callable] = None) -> ColumnSetBuilder:
        """Adds a Column to the column set.

        Args:
            configure_or_width: Either a width string (e.g. 'auto', 'stretch', '1') followed by
                a configure callable, or a single callable that receives a ColumnBuilder.
            configure: A callable that receives a ColumnBuilder, required when configure_or_width
                is a width string.

        Returns:
            The builder instance for method chaining.
        """
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
        """Builds and returns the configured ColumnSet dictionary.

        Returns:
            The configured ColumnSet dictionary.
        """
        return self._column_set
