from __future__ import annotations
from typing import Callable
from ..enums import ContainerStyle, VerticalAlignment, Spacing


class ContainerBuilder:
    def __init__(self):
        self._container: dict = {'type': 'Container', 'items': []}

    def with_id(self, id: str) -> ContainerBuilder:
        self._container['id'] = id
        return self

    def with_style(self, style: ContainerStyle) -> ContainerBuilder:
        self._container['style'] = style.value
        return self

    def with_vertical_content_alignment(self, alignment: VerticalAlignment) -> ContainerBuilder:
        self._container['verticalContentAlignment'] = alignment.value
        return self

    def with_bleed(self, bleed: bool = True) -> ContainerBuilder:
        self._container['bleed'] = bleed
        return self

    def with_min_height(self, min_height: str) -> ContainerBuilder:
        self._container['minHeight'] = min_height
        return self

    def with_spacing(self, spacing: Spacing) -> ContainerBuilder:
        self._container['spacing'] = spacing.value
        return self

    def with_separator(self, separator: bool) -> ContainerBuilder:
        self._container['separator'] = separator
        return self

    def with_is_visible(self, is_visible: bool) -> ContainerBuilder:
        self._container['isVisible'] = is_visible
        return self

    def with_background_image(self, configure: Callable) -> ContainerBuilder:
        from .background_image_builder import BackgroundImageBuilder
        b = BackgroundImageBuilder()
        configure(b)
        self._container['backgroundImage'] = b.build()
        return self

    def with_select_action(self, configure: Callable) -> ContainerBuilder:
        from .action_builder import ActionBuilder
        b = ActionBuilder()
        configure(b)
        self._container['selectAction'] = b.build()
        return self

    def add_text_block(self, configure: Callable) -> ContainerBuilder:
        from .text_block_builder import TextBlockBuilder
        b = TextBlockBuilder()
        configure(b)
        self._container['items'].append(b.build())
        return self

    def add_image(self, configure: Callable) -> ContainerBuilder:
        from .image_builder import ImageBuilder
        b = ImageBuilder()
        configure(b)
        self._container['items'].append(b.build())
        return self

    def add_container(self, configure: Callable) -> ContainerBuilder:
        b = ContainerBuilder()
        configure(b)
        self._container['items'].append(b.build())
        return self

    def add_column_set(self, configure: Callable) -> ContainerBuilder:
        from .column_set_builder import ColumnSetBuilder
        b = ColumnSetBuilder()
        configure(b)
        self._container['items'].append(b.build())
        return self

    def add_fact_set(self, configure: Callable) -> ContainerBuilder:
        from .fact_set_builder import FactSetBuilder
        b = FactSetBuilder()
        configure(b)
        self._container['items'].append(b.build())
        return self

    def add_rich_text_block(self, configure: Callable) -> ContainerBuilder:
        from .rich_text_block_builder import RichTextBlockBuilder
        b = RichTextBlockBuilder()
        configure(b)
        self._container['items'].append(b.build())
        return self

    def add_action_set(self, configure: Callable) -> ContainerBuilder:
        from .action_set_builder import ActionSetBuilder
        b = ActionSetBuilder()
        configure(b)
        self._container['items'].append(b.build())
        return self

    def add_element(self, element: dict) -> ContainerBuilder:
        self._container['items'].append(element)
        return self

    def build(self) -> dict:
        return self._container
