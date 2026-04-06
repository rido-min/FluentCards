from __future__ import annotations
from typing import Callable
from ..enums import ContainerStyle, VerticalAlignment


class ColumnBuilder:
    def __init__(self):
        self._column: dict = {'type': 'Column', 'items': []}

    def with_id(self, id: str) -> ColumnBuilder:
        self._column['id'] = id
        return self

    def with_width(self, width: str) -> ColumnBuilder:
        self._column['width'] = width
        return self

    def with_style(self, style: ContainerStyle) -> ColumnBuilder:
        self._column['style'] = style.value
        return self

    def with_vertical_content_alignment(self, alignment: VerticalAlignment) -> ColumnBuilder:
        self._column['verticalContentAlignment'] = alignment.value
        return self

    def with_bleed(self, bleed: bool = True) -> ColumnBuilder:
        self._column['bleed'] = bleed
        return self

    def with_min_height(self, min_height: str) -> ColumnBuilder:
        self._column['minHeight'] = min_height
        return self

    def with_background_image(self, configure: Callable) -> ColumnBuilder:
        from .background_image_builder import BackgroundImageBuilder
        b = BackgroundImageBuilder()
        configure(b)
        self._column['backgroundImage'] = b.build()
        return self

    def with_select_action(self, configure: Callable) -> ColumnBuilder:
        from .action_builder import ActionBuilder
        b = ActionBuilder()
        configure(b)
        self._column['selectAction'] = b.build()
        return self

    def add_text_block(self, configure: Callable) -> ColumnBuilder:
        from .text_block_builder import TextBlockBuilder
        b = TextBlockBuilder()
        configure(b)
        self._column['items'].append(b.build())
        return self

    def add_image(self, configure: Callable) -> ColumnBuilder:
        from .image_builder import ImageBuilder
        b = ImageBuilder()
        configure(b)
        self._column['items'].append(b.build())
        return self

    def add_container(self, configure: Callable) -> ColumnBuilder:
        from .container_builder import ContainerBuilder
        b = ContainerBuilder()
        configure(b)
        self._column['items'].append(b.build())
        return self

    def add_element(self, element: dict) -> ColumnBuilder:
        self._column['items'].append(element)
        return self

    def build(self) -> dict:
        return self._column
