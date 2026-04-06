import pytest
from fluent_cards import (
    AdaptiveCardBuilder,
    ContainerBuilder,
    ColumnSetBuilder,
    FactSetBuilder,
    RichTextBlockBuilder,
    ActionSetBuilder,
    MediaBuilder,
    ImageSetBuilder,
    ImageBuilder,
    TableBuilder,
    to_json,
    ContainerStyle,
    VerticalAlignment,
    HorizontalAlignment,
    ImageSize,
    TextColor,
    TextWeight,
)


class TestContainerBuilder:
    def test_builds_container_with_items(self):
        container = (ContainerBuilder()
                     .with_id('c1')
                     .with_style(ContainerStyle.Emphasis)
                     .with_vertical_content_alignment(VerticalAlignment.Center)
                     .with_bleed()
                     .with_min_height('50px')
                     .add_text_block(lambda b: b.with_text('Hello'))
                     .add_image(lambda b: b.with_url('https://example.com/img.png'))
                     .build())

        assert container['type'] == 'Container'
        assert container['id'] == 'c1'
        assert container['style'] == ContainerStyle.Emphasis.value
        assert container['verticalContentAlignment'] == VerticalAlignment.Center.value
        assert container['bleed'] is True
        assert container['minHeight'] == '50px'
        assert len(container['items']) == 2

    def test_add_element_accepts_pre_built_element(self):
        img = ImageBuilder().with_url('https://example.com/x.png').build()
        container = ContainerBuilder().add_element(img).build()
        assert len(container['items']) == 1
        assert container['items'][0]['type'] == 'Image'

    def test_is_added_to_card_body_via_add_container(self):
        card = (AdaptiveCardBuilder.create()
                .add_container(lambda c: c.add_text_block(lambda b: b.with_text('Nested')))
                .build())
        assert card['body'][0]['type'] == 'Container'
        assert card['body'][0]['items'][0]['text'] == 'Nested'

    def test_serializes_container_style_as_camel_case(self):
        card = (AdaptiveCardBuilder.create()
                .add_container(lambda c: c.with_style(ContainerStyle.Accent)
                               .add_text_block(lambda b: b.with_text('x')))
                .build())
        assert '"style": "accent"' in to_json(card)


class TestColumnSetBuilder:
    def test_builds_columns_with_two_argument_overload(self):
        cs = (ColumnSetBuilder()
              .add_column('auto', lambda col: col.add_text_block(lambda b: b.with_text('A')))
              .add_column('stretch', lambda col: col.add_text_block(lambda b: b.with_text('B')))
              .build())

        assert cs['type'] == 'ColumnSet'
        assert len(cs['columns']) == 2
        assert cs['columns'][0]['width'] == 'auto'
        assert cs['columns'][1]['width'] == 'stretch'

    def test_builds_columns_with_single_argument_overload(self):
        cs = (ColumnSetBuilder()
              .add_column(lambda col: col.with_width('1').add_text_block(lambda b: b.with_text('Col')))
              .build())
        assert len(cs['columns']) == 1
        assert cs['columns'][0]['width'] == '1'

    def test_serializes_columns(self):
        card = (AdaptiveCardBuilder.create()
                .add_column_set(lambda cs: cs
                                .add_column('auto', lambda col: col.add_text_block(lambda b: b.with_text('Left')))
                                .add_column('auto', lambda col: col.add_text_block(lambda b: b.with_text('Right'))))
                .build())
        json_str = to_json(card)
        assert '"type": "ColumnSet"' in json_str
        assert '"type": "Column"' in json_str
        assert '"text": "Left"' in json_str
        assert '"text": "Right"' in json_str


class TestFactSetBuilder:
    def test_builds_fact_set_with_string_overload(self):
        fs = (FactSetBuilder()
              .add_fact('Name', 'Alice')
              .add_fact('Age', '30')
              .build())

        assert fs['type'] == 'FactSet'
        assert len(fs['facts']) == 2
        assert fs['facts'][0]['title'] == 'Name'
        assert fs['facts'][0]['value'] == 'Alice'

    def test_builds_fact_set_with_object_overload(self):
        fs = FactSetBuilder().add_fact({'title': 'City', 'value': 'NYC'}).build()
        assert fs['facts'][0]['title'] == 'City'


class TestRichTextBlockBuilder:
    def test_builds_inlines_with_strings_and_text_runs(self):
        rtb = (RichTextBlockBuilder()
               .add_text('Plain text ')
               .add_text_run(lambda r: r.with_text('Bold')
                             .with_weight(TextWeight.Bolder)
                             .with_color(TextColor.Accent))
               .build())

        assert rtb['type'] == 'RichTextBlock'
        assert len(rtb['inlines']) == 2
        assert rtb['inlines'][0] == 'Plain text '
        run = rtb['inlines'][1]
        assert run['type'] == 'TextRun'
        assert run['text'] == 'Bold'
        assert run['weight'] == TextWeight.Bolder.value
        assert run['color'] == TextColor.Accent.value

    def test_serializes_rich_text_block_inlines(self):
        card = (AdaptiveCardBuilder.create()
                .add_rich_text_block(lambda b: b
                                     .add_text('Hello ')
                                     .add_text_run(lambda r: r.with_text('World').with_weight(TextWeight.Bolder)))
                .build())
        json_str = to_json(card)
        assert '"type": "RichTextBlock"' in json_str
        assert '"type": "TextRun"' in json_str
        assert '"Hello "' in json_str
        assert '"weight": "bolder"' in json_str


class TestMediaBuilder:
    def test_builds_media_element(self):
        media = (MediaBuilder()
                 .with_id('m1')
                 .with_poster('https://example.com/poster.jpg')
                 .with_alt_text('A video')
                 .add_source('https://example.com/video.mp4', 'video/mp4')
                 .add_source('https://example.com/video.webm', 'video/webm')
                 .build())

        assert media['type'] == 'Media'
        assert media['poster'] == 'https://example.com/poster.jpg'
        assert len(media['sources']) == 2
        assert media['sources'][0]['mimeType'] == 'video/mp4'


class TestImageSetBuilder:
    def test_builds_image_set_with_images(self):
        is_ = (ImageSetBuilder()
               .with_image_size(ImageSize.Medium)
               .add_image(lambda b: b.with_url('https://example.com/a.png'))
               .add_image(lambda b: b.with_url('https://example.com/b.png'))
               .build())

        assert is_['type'] == 'ImageSet'
        assert is_['imageSize'] == ImageSize.Medium.value
        assert len(is_['images']) == 2


class TestTableBuilder:
    def test_builds_table_with_columns_and_rows(self):
        col = {'width': '1'}
        cell = {'type': 'TableCell', 'items': [{'type': 'TextBlock', 'text': 'Cell'}]}
        row = {'type': 'TableRow', 'cells': [cell]}

        table = (TableBuilder()
                 .with_first_row_as_header()
                 .with_show_grid_lines()
                 .add_column(col)
                 .add_row(row)
                 .build())

        assert table['type'] == 'Table'
        assert table['firstRowAsHeader'] is True
        assert table['showGridLines'] is True
        assert len(table['columns']) == 1
        assert len(table['rows']) == 1

    def test_serializes_table_correctly(self):
        def build_table(t):
            t.with_first_row_as_header()
            t.add_column({'width': 'auto'})
            t.add_row({
                'type': 'TableRow',
                'cells': [{'type': 'TableCell', 'items': [{'type': 'TextBlock', 'text': 'Header'}]}],
            })

        card = AdaptiveCardBuilder.create().add_table(build_table).build()
        json_str = to_json(card)
        assert '"type": "Table"' in json_str
        assert '"type": "TableRow"' in json_str
        assert '"type": "TableCell"' in json_str
        assert '"firstRowAsHeader": true' in json_str


class TestActionSetBuilder:
    def test_builds_action_set_with_actions(self):
        action_set = (ActionSetBuilder()
                      .with_id('as1')
                      .add_action(lambda b: b.open_url('https://example.com', 'Visit'))
                      .build())

        assert action_set['type'] == 'ActionSet'
        assert action_set['id'] == 'as1'
        assert len(action_set['actions']) == 1
        assert action_set['actions'][0]['type'] == 'Action.OpenUrl'
