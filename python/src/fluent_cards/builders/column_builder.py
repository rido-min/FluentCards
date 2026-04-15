from __future__ import annotations
from typing import Callable, TYPE_CHECKING
from ..enums import ContainerStyle, Spacing, VerticalAlignment

if TYPE_CHECKING:
    from .action_builder import ActionBuilder
    from .background_image_builder import BackgroundImageBuilder
    from .container_builder import ContainerBuilder
    from .image_builder import ImageBuilder
    from .text_block_builder import TextBlockBuilder


class ColumnBuilder:
    """Fluent builder for creating Column elements within a ColumnSet."""

    def __init__(self):
        self._column: dict = {'type': 'Column', 'items': []}

    def with_id(self, id: str) -> ColumnBuilder:
        """Sets the unique identifier for the Column.

        Args:
            id: The unique identifier.

        Returns:
            The builder instance for method chaining.
        """
        self._column['id'] = id
        return self

    def with_width(self, width: str) -> ColumnBuilder:
        """Sets the width of the column.

        Args:
            width: The column width ('auto', 'stretch', or a pixel/weight value).

        Returns:
            The builder instance for method chaining.
        """
        self._column['width'] = width
        return self

    def with_style(self, style: ContainerStyle) -> ColumnBuilder:
        """Sets the visual style of the column.

        Args:
            style: The container style.

        Returns:
            The builder instance for method chaining.
        """
        self._column['style'] = style.value
        return self

    def with_vertical_content_alignment(self, alignment: VerticalAlignment) -> ColumnBuilder:
        """Sets the vertical alignment of content within the column.

        Args:
            alignment: The vertical alignment.

        Returns:
            The builder instance for method chaining.
        """
        self._column['verticalContentAlignment'] = alignment.value
        return self

    def with_bleed(self, bleed: bool = True) -> ColumnBuilder:
        """Sets whether the column bleeds into its parent padding.

        Args:
            bleed: True to enable bleed.

        Returns:
            The builder instance for method chaining.
        """
        self._column['bleed'] = bleed
        return self

    def with_min_height(self, min_height: str) -> ColumnBuilder:
        """Sets the minimum height of the column.

        Args:
            min_height: The minimum height value (e.g. '100px').

        Returns:
            The builder instance for method chaining.
        """
        self._column['minHeight'] = min_height
        return self

    def with_background_image(self, configure: Callable[[BackgroundImageBuilder], None]) -> ColumnBuilder:
        """Sets the background image for the column.

        Args:
            configure: A callable that receives a BackgroundImageBuilder to configure the image.

        Returns:
            The builder instance for method chaining.
        """
        from .background_image_builder import BackgroundImageBuilder
        b = BackgroundImageBuilder()
        configure(b)
        self._column['backgroundImage'] = b.build()
        return self

    def with_select_action(self, configure: Callable[[ActionBuilder], None]) -> ColumnBuilder:
        """Sets the action invoked when the column is selected.

        Args:
            configure: A callable that receives an ActionBuilder to configure the action.

        Returns:
            The builder instance for method chaining.
        """
        from .action_builder import ActionBuilder
        b = ActionBuilder()
        configure(b)
        self._column['selectAction'] = b.build()
        return self

    def with_is_visible(self, is_visible: bool) -> ColumnBuilder:
        """Sets whether the column is visible.

        Args:
            is_visible: True if visible, False to hide.

        Returns:
            The builder instance for method chaining.
        """
        self._column['isVisible'] = is_visible
        return self

    def with_spacing(self, spacing: Spacing) -> ColumnBuilder:
        """Sets the spacing between this column and the preceding column.

        Args:
            spacing: The spacing value.

        Returns:
            The builder instance for method chaining.
        """
        self._column['spacing'] = spacing.value
        return self

    def with_separator(self, separator: bool = True) -> ColumnBuilder:
        """Sets whether a separator line is drawn at the left of the column.

        Args:
            separator: True to show separator.

        Returns:
            The builder instance for method chaining.
        """
        self._column['separator'] = separator
        return self

    def with_height(self, height: str) -> ColumnBuilder:
        """Sets the height of the column.

        Args:
            height: The height value ('auto' or 'stretch').

        Returns:
            The builder instance for method chaining.
        """
        self._column['height'] = height
        return self

    def with_fallback(self, fallback) -> ColumnBuilder:
        """Sets the fallback behavior when the column is unsupported.

        Args:
            fallback: The fallback value ('drop' or another element dict).

        Returns:
            The builder instance for method chaining.
        """
        self._column['fallback'] = fallback
        return self

    def with_requires(self, key: str, version: str) -> ColumnBuilder:
        """Sets a feature requirement for the column.

        Args:
            key: The feature name.
            version: The minimum version required.

        Returns:
            The builder instance for method chaining.
        """
        if 'requires' not in self._column:
            self._column['requires'] = {}
        self._column['requires'][key] = version
        return self

    def with_rtl(self, rtl: bool = True) -> ColumnBuilder:
        """Sets whether content should be presented right to left.

        Args:
            rtl: True for right-to-left layout.

        Returns:
            The builder instance for method chaining.
        """
        self._column['rtl'] = rtl
        return self

    def add_text_block(self, configure: Callable[[TextBlockBuilder], None]) -> ColumnBuilder:
        """Adds a TextBlock element to the column.

        Args:
            configure: A callable that receives a TextBlockBuilder to configure the element.

        Returns:
            The builder instance for method chaining.
        """
        from .text_block_builder import TextBlockBuilder
        b = TextBlockBuilder()
        configure(b)
        self._column['items'].append(b.build())
        return self

    def add_image(self, configure: Callable[[ImageBuilder], None]) -> ColumnBuilder:
        """Adds an Image element to the column.

        Args:
            configure: A callable that receives an ImageBuilder to configure the element.

        Returns:
            The builder instance for method chaining.
        """
        from .image_builder import ImageBuilder
        b = ImageBuilder()
        configure(b)
        self._column['items'].append(b.build())
        return self

    def add_container(self, configure: Callable[[ContainerBuilder], None]) -> ColumnBuilder:
        """Adds a Container element to the column.

        Args:
            configure: A callable that receives a ContainerBuilder to configure the element.

        Returns:
            The builder instance for method chaining.
        """
        from .container_builder import ContainerBuilder
        b = ContainerBuilder()
        configure(b)
        self._column['items'].append(b.build())
        return self

    def add_element(self, element: dict) -> ColumnBuilder:
        """Adds a pre-built element directly to the column.

        Args:
            element: A pre-built element dictionary.

        Returns:
            The builder instance for method chaining.
        """
        self._column['items'].append(element)
        return self

    def build(self) -> dict:
        """Builds and returns the configured Column dictionary.

        Returns:
            The configured Column dictionary.
        """
        return self._column
