import pytest
from fluent_cards import (
    AdaptiveCardBuilder,
    ContainerBuilder,
    ColumnSetBuilder,
    ColumnBuilder,
    FactSetBuilder,
    RichTextBlockBuilder,
    ActionSetBuilder,
    MediaBuilder,
    ImageSetBuilder,
    ImageBuilder,
    TableBuilder,
    to_json,
    ContainerStyle,
    Spacing,
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


class TestColumnBuilder:
    def test_with_is_visible(self):
        col = ColumnBuilder().with_is_visible(False).build()
        assert col['isVisible'] is False

    def test_with_spacing(self):
        col = ColumnBuilder().with_spacing(Spacing.Large).build()
        assert col['spacing'] == 'large'

    def test_with_separator(self):
        col = ColumnBuilder().with_separator().build()
        assert col['separator'] is True

    def test_with_separator_false(self):
        col = ColumnBuilder().with_separator(False).build()
        assert col['separator'] is False

    def test_with_height(self):
        col = ColumnBuilder().with_height('stretch').build()
        assert col['height'] == 'stretch'

    def test_with_fallback_drop(self):
        col = ColumnBuilder().with_fallback('drop').build()
        assert col['fallback'] == 'drop'

    def test_with_fallback_element(self):
        fallback = {'type': 'TextBlock', 'text': 'Fallback'}
        col = ColumnBuilder().with_fallback(fallback).build()
        assert col['fallback'] == fallback

    def test_with_requires(self):
        col = (ColumnBuilder()
               .with_requires('adaptiveCards', '1.2')
               .with_requires('myFeature', '1.0')
               .build())
        assert col['requires'] == {'adaptiveCards': '1.2', 'myFeature': '1.0'}

    def test_with_rtl(self):
        col = ColumnBuilder().with_rtl().build()
        assert col['rtl'] is True

    def test_new_methods_chain_with_existing(self):
        col = (ColumnBuilder()
               .with_id('col1')
               .with_width('auto')
               .with_spacing(Spacing.Medium)
               .with_separator()
               .with_is_visible(True)
               .with_height('auto')
               .with_rtl(False)
               .add_text_block(lambda b: b.with_text('Hello'))
               .build())
        assert col['id'] == 'col1'
        assert col['width'] == 'auto'
        assert col['spacing'] == 'medium'
        assert col['separator'] is True
        assert col['isVisible'] is True
        assert col['height'] == 'auto'
        assert col['rtl'] is False
        assert len(col['items']) == 1


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

    def test_adds_caption_source_with_url_first_signature(self):
        media = (MediaBuilder()
                 .add_source('https://example.com/video.mp4', 'video/mp4')
                 .add_caption_source('https://example.com/video-en.vtt', 'text/vtt', 'English')
                 .build())

        assert len(media['captionSources']) == 1
        assert media['captionSources'][0]['url'] == 'https://example.com/video-en.vtt'
        assert media['captionSources'][0]['mimeType'] == 'text/vtt'
        assert media['captionSources'][0]['label'] == 'English'

    def test_adds_caption_source_with_legacy_signature_order(self):
        media = (MediaBuilder()
                 .add_source('https://example.com/video.mp4', 'video/mp4')
                 .add_caption_source('text/vtt', 'https://example.com/video-en.vtt', 'English')
                 .build())

        assert media['captionSources'][0]['url'] == 'https://example.com/video-en.vtt'
        assert media['captionSources'][0]['mimeType'] == 'text/vtt'

    def test_adds_caption_source_from_dict(self):
        caption = {
            'type': 'CaptionSource',
            'mimeType': 'text/vtt',
            'url': 'https://example.com/video-es.vtt',
            'label': 'Spanish',
        }
        media = (
            MediaBuilder()
            .add_source('https://example.com/video.mp4', 'video/mp4')
            .add_caption_source(caption)
            .build()
        )

        assert media['captionSources'][0] == caption

    def test_raises_when_dict_caption_source_receives_extra_args(self):
        caption = {
            'type': 'CaptionSource',
            'mimeType': 'text/vtt',
            'url': 'https://example.com/video-es.vtt',
            'label': 'Spanish',
        }

        with pytest.raises(ValueError):
            MediaBuilder().add_caption_source(caption, 'text/vtt', 'Spanish')


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
