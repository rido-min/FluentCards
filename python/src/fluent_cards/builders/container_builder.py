from __future__ import annotations
from typing import Callable
from ..enums import ContainerStyle, VerticalAlignment, Spacing


class ContainerBuilder:
    """Fluent builder for creating Container elements."""

    def __init__(self):
        self._container: dict = {'type': 'Container', 'items': []}

    def with_id(self, id: str) -> ContainerBuilder:
        """Sets the unique identifier for the Container.

        Args:
            id: The unique identifier.

        Returns:
            The builder instance for method chaining.
        """
        self._container['id'] = id
        return self

    def with_style(self, style: ContainerStyle) -> ContainerBuilder:
        """Sets the visual style of the container.

        Args:
            style: The container style.

        Returns:
            The builder instance for method chaining.
        """
        self._container['style'] = style.value
        return self

    def with_vertical_content_alignment(self, alignment: VerticalAlignment) -> ContainerBuilder:
        """Sets the vertical alignment of content within the container.

        Args:
            alignment: The vertical alignment.

        Returns:
            The builder instance for method chaining.
        """
        self._container['verticalContentAlignment'] = alignment.value
        return self

    def with_bleed(self, bleed: bool = True) -> ContainerBuilder:
        """Sets whether the container bleeds into its parent padding.

        Args:
            bleed: True to enable bleed.

        Returns:
            The builder instance for method chaining.
        """
        self._container['bleed'] = bleed
        return self

    def with_min_height(self, min_height: str) -> ContainerBuilder:
        """Sets the minimum height of the container.

        Args:
            min_height: The minimum height value (e.g. '100px').

        Returns:
            The builder instance for method chaining.
        """
        self._container['minHeight'] = min_height
        return self

    def with_spacing(self, spacing: Spacing) -> ContainerBuilder:
        """Sets the spacing before the container.

        Args:
            spacing: The spacing value.

        Returns:
            The builder instance for method chaining.
        """
        self._container['spacing'] = spacing.value
        return self

    def with_separator(self, separator: bool) -> ContainerBuilder:
        """Sets whether a separator line is drawn above the container.

        Args:
            separator: True to draw a separator line above the element.

        Returns:
            The builder instance for method chaining.
        """
        self._container['separator'] = separator
        return self

    def with_is_visible(self, is_visible: bool) -> ContainerBuilder:
        """Sets whether the container is visible.

        Args:
            is_visible: False to hide the element initially.

        Returns:
            The builder instance for method chaining.
        """
        self._container['isVisible'] = is_visible
        return self

    def with_background_image(self, configure: Callable) -> ContainerBuilder:
        """Sets the background image for the container.

        Args:
            configure: A callable that receives a BackgroundImageBuilder to configure the image.

        Returns:
            The builder instance for method chaining.
        """
        from .background_image_builder import BackgroundImageBuilder
        b = BackgroundImageBuilder()
        configure(b)
        self._container['backgroundImage'] = b.build()
        return self

    def with_select_action(self, configure: Callable) -> ContainerBuilder:
        """Sets the action invoked when the container is selected.

        Args:
            configure: A callable that receives an ActionBuilder to configure the action.

        Returns:
            The builder instance for method chaining.
        """
        from .action_builder import ActionBuilder
        b = ActionBuilder()
        configure(b)
        self._container['selectAction'] = b.build()
        return self

    def add_text_block(self, configure: Callable) -> ContainerBuilder:
        """Adds a TextBlock element to the container.

        Args:
            configure: A callable that receives a TextBlockBuilder to configure the element.

        Returns:
            The builder instance for method chaining.
        """
        from .text_block_builder import TextBlockBuilder
        b = TextBlockBuilder()
        configure(b)
        self._container['items'].append(b.build())
        return self

    def add_image(self, configure: Callable) -> ContainerBuilder:
        """Adds an Image element to the container.

        Args:
            configure: A callable that receives an ImageBuilder to configure the element.

        Returns:
            The builder instance for method chaining.
        """
        from .image_builder import ImageBuilder
        b = ImageBuilder()
        configure(b)
        self._container['items'].append(b.build())
        return self

    def add_container(self, configure: Callable) -> ContainerBuilder:
        """Adds a nested Container element.

        Args:
            configure: A callable that receives a ContainerBuilder to configure the element.

        Returns:
            The builder instance for method chaining.
        """
        b = ContainerBuilder()
        configure(b)
        self._container['items'].append(b.build())
        return self

    def add_column_set(self, configure: Callable) -> ContainerBuilder:
        """Adds a ColumnSet element to the container.

        Args:
            configure: A callable that receives a ColumnSetBuilder to configure the element.

        Returns:
            The builder instance for method chaining.
        """
        from .column_set_builder import ColumnSetBuilder
        b = ColumnSetBuilder()
        configure(b)
        self._container['items'].append(b.build())
        return self

    def add_fact_set(self, configure: Callable) -> ContainerBuilder:
        """Adds a FactSet element to the container.

        Args:
            configure: A callable that receives a FactSetBuilder to configure the element.

        Returns:
            The builder instance for method chaining.
        """
        from .fact_set_builder import FactSetBuilder
        b = FactSetBuilder()
        configure(b)
        self._container['items'].append(b.build())
        return self

    def add_rich_text_block(self, configure: Callable) -> ContainerBuilder:
        """Adds a RichTextBlock element to the container.

        Args:
            configure: A callable that receives a RichTextBlockBuilder to configure the element.

        Returns:
            The builder instance for method chaining.
        """
        from .rich_text_block_builder import RichTextBlockBuilder
        b = RichTextBlockBuilder()
        configure(b)
        self._container['items'].append(b.build())
        return self

    def add_action_set(self, configure: Callable) -> ContainerBuilder:
        """Adds an ActionSet element to the container.

        Args:
            configure: A callable that receives an ActionSetBuilder to configure the element.

        Returns:
            The builder instance for method chaining.
        """
        from .action_set_builder import ActionSetBuilder
        b = ActionSetBuilder()
        configure(b)
        self._container['items'].append(b.build())
        return self

    def add_element(self, element: dict) -> ContainerBuilder:
        """Adds a pre-built element directly to the container.

        Args:
            element: A pre-built element dictionary.

        Returns:
            The builder instance for method chaining.
        """
        self._container['items'].append(element)
        return self

    def build(self) -> dict:
        """Builds and returns the configured Container dictionary.

        Returns:
            The configured Container dictionary.
        """
        return self._container
