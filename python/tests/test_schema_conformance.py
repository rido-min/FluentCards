"""Schema conformance tests — builder output matches expected JSON structure."""

import pytest
from fluent_cards import (
    AdaptiveCardBuilder,
    TextBlockBuilder,
    ImageBuilder,
    ContainerBuilder,
    ColumnSetBuilder,
    ColumnBuilder,
    FactSetBuilder,
    RichTextBlockBuilder,
    ActionSetBuilder,
    TableBuilder,
    MediaBuilder,
    ImageSetBuilder,
    ActionBuilder,
    RefreshBuilder,
    AuthenticationBuilder,
    BackgroundImageBuilder,
    TextSize,
    TextWeight,
    TextColor,
    HorizontalAlignment,
    VerticalAlignment,
    ImageSize,
    ImageStyle,
    ActionStyle,
    ContainerStyle,
    FontType,
    Spacing,
    TextBlockStyle,
    BackgroundImageFillMode,
    TextInputStyle,
    ChoiceInputStyle,
    AssociatedInputs,
    ActionMode,
    InputLabelPosition,
    InputStyle,
    BlockElementHeight,
)


class TestTextBlockSchemaConformance:
    def test_text_block_all_properties(self):
        tb = (
            TextBlockBuilder()
            .with_id("tb1")
            .with_text("Hello World")
            .with_size(TextSize.ExtraLarge)
            .with_weight(TextWeight.Bolder)
            .with_color(TextColor.Accent)
            .with_is_subtle(True)
            .with_wrap(True)
            .with_max_lines(3)
            .with_horizontal_alignment(HorizontalAlignment.Center)
            .with_font_type(FontType.Monospace)
            .with_style(TextBlockStyle.Heading)
            .with_spacing(Spacing.Large)
            .with_separator(True)
            .with_is_visible(False)
            .build()
        )
        assert tb["type"] == "TextBlock"
        assert tb["id"] == "tb1"
        assert tb["text"] == "Hello World"
        assert tb["size"] == "extraLarge"
        assert tb["weight"] == "bolder"
        assert tb["color"] == "accent"
        assert tb["isSubtle"] is True
        assert tb["wrap"] is True
        assert tb["maxLines"] == 3
        assert tb["horizontalAlignment"] == "center"
        assert tb["fontType"] == "monospace"
        assert tb["style"] == "heading"
        assert tb["spacing"] == "large"
        assert tb["separator"] is True
        assert tb["isVisible"] is False

    def test_text_block_minimal(self):
        tb = TextBlockBuilder().with_text("Hi").build()
        assert tb == {"type": "TextBlock", "text": "Hi"}

    def test_text_block_with_select_action(self):
        action = {"type": "Action.OpenUrl", "url": "https://example.com"}
        tb = TextBlockBuilder().with_text("Click").with_select_action(action).build()
        assert tb["selectAction"] == action


class TestImageSchemaConformance:
    def test_image_all_properties(self):
        img = (
            ImageBuilder()
            .with_id("img1")
            .with_url("https://example.com/image.png")
            .with_alt_text("An image")
            .with_size(ImageSize.Large)
            .with_style(ImageStyle.Person)
            .with_width("100px")
            .with_height("100px")
            .with_horizontal_alignment(HorizontalAlignment.Right)
            .with_background_color("#FF0000")
            .with_spacing(Spacing.Medium)
            .with_separator(True)
            .build()
        )
        assert img["type"] == "Image"
        assert img["id"] == "img1"
        assert img["url"] == "https://example.com/image.png"
        assert img["altText"] == "An image"
        assert img["size"] == "large"
        assert img["style"] == "person"
        assert img["width"] == "100px"
        assert img["height"] == "100px"
        assert img["horizontalAlignment"] == "right"
        assert img["backgroundColor"] == "#FF0000"
        assert img["spacing"] == "medium"
        assert img["separator"] is True

    def test_image_with_select_action(self):
        img = (
            ImageBuilder()
            .with_url("https://example.com/img.png")
            .with_select_action(lambda a: a.open_url("https://example.com"))
            .build()
        )
        assert img["selectAction"]["type"] == "Action.OpenUrl"
        assert img["selectAction"]["url"] == "https://example.com"


class TestContainerSchemaConformance:
    def test_container_with_items_and_properties(self):
        c = (
            ContainerBuilder()
            .with_id("c1")
            .with_style(ContainerStyle.Emphasis)
            .with_bleed(True)
            .with_min_height("100px")
            .with_vertical_content_alignment(VerticalAlignment.Center)
            .with_spacing(Spacing.Large)
            .with_separator(True)
            .with_is_visible(True)
            .add_text_block(lambda tb: tb.with_text("Inside"))
            .build()
        )
        assert c["type"] == "Container"
        assert c["id"] == "c1"
        assert c["style"] == "emphasis"
        assert c["bleed"] is True
        assert c["minHeight"] == "100px"
        assert c["verticalContentAlignment"] == "center"
        assert c["spacing"] == "large"
        assert c["separator"] is True
        assert c["isVisible"] is True
        assert len(c["items"]) == 1
        assert c["items"][0]["type"] == "TextBlock"

    def test_container_with_background_image(self):
        c = (
            ContainerBuilder()
            .with_background_image(
                lambda bg: bg.with_url("https://example.com/bg.png")
                .with_fill_mode(BackgroundImageFillMode.Cover)
            )
            .add_text_block(lambda tb: tb.with_text("Over BG"))
            .build()
        )
        assert c["backgroundImage"]["url"] == "https://example.com/bg.png"
        assert c["backgroundImage"]["fillMode"] == "cover"

    def test_container_with_select_action(self):
        c = (
            ContainerBuilder()
            .with_select_action(lambda a: a.open_url("https://example.com"))
            .add_text_block(lambda tb: tb.with_text("Clickable"))
            .build()
        )
        assert c["selectAction"]["type"] == "Action.OpenUrl"

    def test_container_with_multiple_item_types(self):
        c = (
            ContainerBuilder()
            .add_text_block(lambda tb: tb.with_text("Text"))
            .add_image(lambda img: img.with_url("https://example.com/img.png"))
            .add_container(lambda inner: inner.add_text_block(lambda tb: tb.with_text("Nested")))
            .build()
        )
        assert len(c["items"]) == 3
        assert c["items"][0]["type"] == "TextBlock"
        assert c["items"][1]["type"] == "Image"
        assert c["items"][2]["type"] == "Container"


class TestColumnSetSchemaConformance:
    def test_column_set_with_columns(self):
        cs = (
            ColumnSetBuilder()
            .with_id("cs1")
            .add_column(lambda col: col.with_width("1").add_text_block(lambda tb: tb.with_text("Col1")))
            .add_column(lambda col: col.with_width("2").add_text_block(lambda tb: tb.with_text("Col2")))
            .build()
        )
        assert cs["type"] == "ColumnSet"
        assert cs["id"] == "cs1"
        assert len(cs["columns"]) == 2
        assert cs["columns"][0]["type"] == "Column"
        assert cs["columns"][0]["width"] == "1"
        assert cs["columns"][1]["width"] == "2"

    def test_column_set_with_style_and_bleed(self):
        cs = (
            ColumnSetBuilder()
            .with_style(ContainerStyle.Accent)
            .with_bleed(True)
            .with_min_height("50px")
            .add_column(lambda col: col.with_width("auto"))
            .build()
        )
        assert cs["style"] == "accent"
        assert cs["bleed"] is True
        assert cs["minHeight"] == "50px"

    def test_column_with_width_shorthand(self):
        cs = (
            ColumnSetBuilder()
            .add_column("stretch", lambda col: col.add_text_block(lambda tb: tb.with_text("A")))
            .build()
        )
        assert cs["columns"][0]["width"] == "stretch"


class TestFactSetSchemaConformance:
    def test_fact_set_with_facts(self):
        fs = (
            FactSetBuilder()
            .with_id("fs1")
            .add_fact("Name", "John Doe")
            .add_fact("Age", "30")
            .build()
        )
        assert fs["type"] == "FactSet"
        assert fs["id"] == "fs1"
        assert len(fs["facts"]) == 2
        assert fs["facts"][0] == {"title": "Name", "value": "John Doe"}
        assert fs["facts"][1] == {"title": "Age", "value": "30"}

    def test_fact_set_with_dict_facts(self):
        fs = (
            FactSetBuilder()
            .add_fact({"title": "Custom", "value": "Val"})
            .build()
        )
        assert fs["facts"][0] == {"title": "Custom", "value": "Val"}


class TestRichTextBlockSchemaConformance:
    def test_rich_text_block_with_inlines(self):
        rtb = (
            RichTextBlockBuilder()
            .with_id("rtb1")
            .with_horizontal_alignment(HorizontalAlignment.Center)
            .add_text("Plain text ")
            .add_text_run(
                lambda tr: tr.with_text("Bold text")
                .with_weight(TextWeight.Bolder)
                .with_color(TextColor.Accent)
                .is_italic(True)
            )
            .build()
        )
        assert rtb["type"] == "RichTextBlock"
        assert rtb["id"] == "rtb1"
        assert rtb["horizontalAlignment"] == "center"
        assert len(rtb["inlines"]) == 2
        assert rtb["inlines"][0] == "Plain text "
        run = rtb["inlines"][1]
        assert run["type"] == "TextRun"
        assert run["text"] == "Bold text"
        assert run["weight"] == "bolder"
        assert run["color"] == "accent"
        assert run["italic"] is True

    def test_rich_text_block_text_run_all_decorations(self):
        rtb = (
            RichTextBlockBuilder()
            .add_text_run(
                lambda tr: tr.with_text("Decorated")
                .is_subtle(True)
                .is_strikethrough(True)
                .is_underline(True)
                .is_highlight(True)
            )
            .build()
        )
        run = rtb["inlines"][0]
        assert run["isSubtle"] is True
        assert run["strikethrough"] is True
        assert run["underline"] is True
        assert run["highlight"] is True


class TestTableSchemaConformance:
    def test_table_with_columns_rows_cells(self):
        t = (
            TableBuilder()
            .with_id("tbl1")
            .with_first_row_as_header(True)
            .with_show_grid_lines(True)
            .with_grid_style(ContainerStyle.Default)
            .with_horizontal_cell_content_alignment(HorizontalAlignment.Center)
            .with_vertical_cell_content_alignment(VerticalAlignment.Center)
            .add_column({"width": 1})
            .add_column({"width": 2})
            .add_row(
                {
                    "type": "TableRow",
                    "cells": [
                        {"type": "TableCell", "items": [{"type": "TextBlock", "text": "Header1"}]},
                        {"type": "TableCell", "items": [{"type": "TextBlock", "text": "Header2"}]},
                    ],
                }
            )
            .build()
        )
        assert t["type"] == "Table"
        assert t["id"] == "tbl1"
        assert t["firstRowAsHeader"] is True
        assert t["showGridLines"] is True
        assert t["gridStyle"] == "default"
        assert t["horizontalCellContentAlignment"] == "center"
        assert t["verticalCellContentAlignment"] == "center"
        assert len(t["columns"]) == 2
        assert len(t["rows"]) == 1
        assert len(t["rows"][0]["cells"]) == 2


class TestActionSetSchemaConformance:
    def test_action_set_with_mixed_actions(self):
        aset = (
            ActionSetBuilder()
            .with_id("as1")
            .add_action(lambda a: a.open_url("https://example.com").with_title("Open"))
            .add_action(lambda a: a.submit("Submit").with_data({"key": "value"}))
            .add_action(lambda a: a.execute("Run").with_verb("doSomething"))
            .add_action(
                lambda a: a.toggle_visibility("Toggle").add_target_element("elem1", True)
            )
            .build()
        )
        assert aset["type"] == "ActionSet"
        assert aset["id"] == "as1"
        assert len(aset["actions"]) == 4
        assert aset["actions"][0]["type"] == "Action.OpenUrl"
        assert aset["actions"][1]["type"] == "Action.Submit"
        assert aset["actions"][1]["data"] == {"key": "value"}
        assert aset["actions"][2]["type"] == "Action.Execute"
        assert aset["actions"][2]["verb"] == "doSomething"
        assert aset["actions"][3]["type"] == "Action.ToggleVisibility"

    def test_action_set_with_show_card(self):
        inner_card = AdaptiveCardBuilder.create().add_text_block(lambda tb: tb.with_text("Inner")).build()
        aset = (
            ActionSetBuilder()
            .add_action(lambda a: a.show_card("Details").with_card(inner_card))
            .build()
        )
        assert aset["actions"][0]["type"] == "Action.ShowCard"
        assert aset["actions"][0]["card"]["type"] == "AdaptiveCard"


class TestCardLevelSchemaConformance:
    def test_card_version_and_schema(self):
        card = AdaptiveCardBuilder.create().with_version("1.5").build()
        assert card["type"] == "AdaptiveCard"
        assert card["version"] == "1.5"
        assert "$schema" in card

    def test_card_fallback_text(self):
        card = AdaptiveCardBuilder.create().with_fallback_text("Fallback content").build()
        assert card["fallbackText"] == "Fallback content"

    def test_card_speak(self):
        card = AdaptiveCardBuilder.create().with_speak("Spoken text").build()
        assert card["speak"] == "Spoken text"

    def test_card_lang(self):
        card = AdaptiveCardBuilder.create().with_lang("en-US").build()
        assert card["lang"] == "en-US"

    def test_card_rtl(self):
        card = AdaptiveCardBuilder.create().with_rtl(True).build()
        assert card["rtl"] is True

    def test_card_min_height(self):
        card = AdaptiveCardBuilder.create().with_min_height("300px").build()
        assert card["minHeight"] == "300px"

    def test_card_metadata(self):
        card = AdaptiveCardBuilder.create().with_metadata("https://example.com/page").build()
        assert card["metadata"] == {"webUrl": "https://example.com/page"}

    def test_card_background_image(self):
        card = (
            AdaptiveCardBuilder.create()
            .with_background_image(
                lambda bg: bg.with_url("https://example.com/bg.png")
                .with_fill_mode(BackgroundImageFillMode.RepeatHorizontally)
                .with_horizontal_alignment(HorizontalAlignment.Center)
                .with_vertical_alignment(VerticalAlignment.Center)
            )
            .build()
        )
        bg = card["backgroundImage"]
        assert bg["url"] == "https://example.com/bg.png"
        assert bg["fillMode"] == "repeatHorizontally"
        assert bg["horizontalAlignment"] == "center"
        assert bg["verticalAlignment"] == "center"

    def test_card_with_all_level_properties(self):
        card = (
            AdaptiveCardBuilder.create()
            .with_version("1.6")
            .with_fallback_text("Fallback")
            .with_speak("Speak this")
            .with_lang("fr-FR")
            .with_rtl(True)
            .with_min_height("200px")
            .with_metadata("https://example.com")
            .add_text_block(lambda tb: tb.with_text("Body"))
            .build()
        )
        assert card["version"] == "1.6"
        assert card["fallbackText"] == "Fallback"
        assert card["speak"] == "Speak this"
        assert card["lang"] == "fr-FR"
        assert card["rtl"] is True
        assert card["minHeight"] == "200px"
        assert card["metadata"]["webUrl"] == "https://example.com"
        assert len(card["body"]) == 1

    def test_card_select_action(self):
        action = {"type": "Action.OpenUrl", "url": "https://example.com"}
        card = AdaptiveCardBuilder.create().with_select_action(action).build()
        assert card["selectAction"] == action


class TestInputSchemaConformance:
    def test_input_text_all_properties(self):
        card = (
            AdaptiveCardBuilder.create()
            .add_input_text(
                lambda i: i.with_id("txt1")
                .with_label("Name")
                .with_placeholder("Enter name")
                .with_value("John")
                .with_max_length(100)
                .with_is_multiline(True)
                .with_style(TextInputStyle.Email)
                .with_regex(r"^[a-zA-Z]+$")
                .with_is_required(True)
                .with_error_message("Required field")
            )
            .build()
        )
        inp = card["body"][0]
        assert inp["type"] == "Input.Text"
        assert inp["id"] == "txt1"
        assert inp["label"] == "Name"
        assert inp["placeholder"] == "Enter name"
        assert inp["value"] == "John"
        assert inp["maxLength"] == 100
        assert inp["isMultiline"] is True
        assert inp["style"] == "email"
        assert inp["regex"] == r"^[a-zA-Z]+$"
        assert inp["isRequired"] is True
        assert inp["errorMessage"] == "Required field"

    def test_input_number_all_properties(self):
        card = (
            AdaptiveCardBuilder.create()
            .add_input_number(
                lambda i: i.with_id("num1")
                .with_label("Quantity")
                .with_placeholder("0-100")
                .with_value(50)
                .with_min(0)
                .with_max(100)
                .with_is_required(True)
                .with_error_message("Out of range")
            )
            .build()
        )
        inp = card["body"][0]
        assert inp["type"] == "Input.Number"
        assert inp["id"] == "num1"
        assert inp["min"] == 0
        assert inp["max"] == 100
        assert inp["value"] == 50

    def test_input_date_all_properties(self):
        card = (
            AdaptiveCardBuilder.create()
            .add_input_date(
                lambda i: i.with_id("date1")
                .with_label("Date")
                .with_placeholder("Select date")
                .with_value("2024-01-15")
                .with_min("2024-01-01")
                .with_max("2024-12-31")
                .with_is_required(True)
                .with_error_message("Invalid date")
            )
            .build()
        )
        inp = card["body"][0]
        assert inp["type"] == "Input.Date"
        assert inp["id"] == "date1"
        assert inp["value"] == "2024-01-15"
        assert inp["min"] == "2024-01-01"
        assert inp["max"] == "2024-12-31"

    def test_input_time_all_properties(self):
        card = (
            AdaptiveCardBuilder.create()
            .add_input_time(
                lambda i: i.with_id("time1")
                .with_label("Time")
                .with_placeholder("Select time")
                .with_value("09:30")
                .with_min("08:00")
                .with_max("17:00")
                .with_is_required(True)
                .with_error_message("Invalid time")
            )
            .build()
        )
        inp = card["body"][0]
        assert inp["type"] == "Input.Time"
        assert inp["id"] == "time1"
        assert inp["value"] == "09:30"
        assert inp["min"] == "08:00"
        assert inp["max"] == "17:00"

    def test_input_toggle_all_properties(self):
        card = (
            AdaptiveCardBuilder.create()
            .add_input_toggle(
                lambda i: i.with_id("toggle1")
                .with_title("Accept terms")
                .with_label("Terms")
                .with_value("true")
                .with_value_on("true")
                .with_value_off("false")
                .with_wrap(True)
                .with_is_required(True)
                .with_error_message("Must accept")
            )
            .build()
        )
        inp = card["body"][0]
        assert inp["type"] == "Input.Toggle"
        assert inp["id"] == "toggle1"
        assert inp["title"] == "Accept terms"
        assert inp["value"] == "true"
        assert inp["valueOn"] == "true"
        assert inp["valueOff"] == "false"
        assert inp["wrap"] is True

    def test_input_choice_set_all_properties(self):
        card = (
            AdaptiveCardBuilder.create()
            .add_input_choice_set(
                lambda i: i.with_id("choice1")
                .with_label("Color")
                .with_placeholder("Pick a color")
                .with_value("red")
                .with_style(ChoiceInputStyle.Expanded)
                .is_multi_select(True)
                .with_wrap(True)
                .add_choice("Red", "red")
                .add_choice("Blue", "blue")
                .add_choice("Green", "green")
                .with_is_required(True)
                .with_error_message("Select a color")
            )
            .build()
        )
        inp = card["body"][0]
        assert inp["type"] == "Input.ChoiceSet"
        assert inp["id"] == "choice1"
        assert inp["style"] == "expanded"
        assert inp["isMultiSelect"] is True
        assert len(inp["choices"]) == 3
        assert inp["choices"][0] == {"title": "Red", "value": "red"}


class TestMediaSchemaConformance:
    def test_media_all_properties(self):
        m = (
            MediaBuilder()
            .with_id("media1")
            .with_poster("https://example.com/poster.png")
            .with_alt_text("A video")
            .add_source("https://example.com/video.mp4", "video/mp4")
            .add_caption_source(
                "https://example.com/video-en.vtt",
                "text/vtt",
                "English"
            )
            .with_spacing(Spacing.Large)
            .with_separator(True)
            .with_is_visible(True)
            .build()
        )
        assert m["type"] == "Media"
        assert m["id"] == "media1"
        assert m["poster"] == "https://example.com/poster.png"
        assert m["altText"] == "A video"
        assert len(m["sources"]) == 1
        assert m["sources"][0]["mimeType"] == "video/mp4"
        assert m["sources"][0]["url"] == "https://example.com/video.mp4"
        assert len(m["captionSources"]) == 1
        assert m["captionSources"][0]["mimeType"] == "text/vtt"
        assert m["captionSources"][0]["url"] == "https://example.com/video-en.vtt"
        assert m["captionSources"][0]["label"] == "English"
        assert m["captionSources"][0]["type"] == "CaptionSource"
        assert m["spacing"] == "large"
        assert m["separator"] is True
        assert m["isVisible"] is True


class TestImageSetSchemaConformance:
    def test_image_set_all_properties(self):
        iset = (
            ImageSetBuilder()
            .with_id("imgset1")
            .with_image_size(ImageSize.Medium)
            .add_image(lambda img: img.with_url("https://example.com/1.png"))
            .add_image(lambda img: img.with_url("https://example.com/2.png"))
            .with_spacing(Spacing.Small)
            .with_separator(True)
            .with_is_visible(True)
            .build()
        )
        assert iset["type"] == "ImageSet"
        assert iset["id"] == "imgset1"
        assert iset["imageSize"] == "medium"
        assert len(iset["images"]) == 2
        assert iset["images"][0]["url"] == "https://example.com/1.png"
        assert iset["spacing"] == "small"
        assert iset["separator"] is True
        assert iset["isVisible"] is True


class TestColumnSchemaConformance:
    def test_column_all_properties(self):
        col = (
            ColumnBuilder()
            .with_id("col1")
            .with_width("2")
            .with_style(ContainerStyle.Emphasis)
            .with_vertical_content_alignment(VerticalAlignment.Center)
            .with_bleed(True)
            .with_min_height("100px")
            .with_background_image(
                lambda bg: bg.with_url("https://example.com/bg.png")
            )
            .with_select_action(lambda a: a.open_url("https://example.com"))
            .with_spacing(Spacing.Medium)
            .with_separator(True)
            .with_is_visible(True)
            .add_text_block(lambda tb: tb.with_text("In column"))
            .build()
        )
        assert col["type"] == "Column"
        assert col["id"] == "col1"
        assert col["width"] == "2"
        assert col["style"] == "emphasis"
        assert col["verticalContentAlignment"] == "center"
        assert col["bleed"] is True
        assert col["minHeight"] == "100px"
        assert col["backgroundImage"]["url"] == "https://example.com/bg.png"
        assert col["selectAction"]["type"] == "Action.OpenUrl"
        assert col["spacing"] == "medium"
        assert col["separator"] is True
        assert col["isVisible"] is True
        assert len(col["items"]) == 1
        assert col["items"][0]["type"] == "TextBlock"


class TestActionSchemaConformance:
    def test_open_url_action_all_properties(self):
        a = (
            ActionBuilder()
            .open_url("https://example.com")
            .with_title("Open Link")
            .with_icon_url("https://example.com/icon.png")
            .with_id("action1")
            .with_style(ActionStyle.Positive)
            .with_tooltip("Opens a link")
            .with_is_enabled(True)
            .with_mode(ActionMode.Primary)
            .build()
        )
        assert a["type"] == "Action.OpenUrl"
        assert a["url"] == "https://example.com"
        assert a["title"] == "Open Link"
        assert a["iconUrl"] == "https://example.com/icon.png"
        assert a["id"] == "action1"
        assert a["style"] == "positive"
        assert a["tooltip"] == "Opens a link"
        assert a["isEnabled"] is True
        assert a["mode"] == "primary"

    def test_submit_action_all_properties(self):
        a = (
            ActionBuilder()
            .submit("Send")
            .with_data({"key": "value"})
            .with_associated_inputs(AssociatedInputs.Auto)
            .with_icon_url("https://example.com/icon.png")
            .with_id("submit1")
            .with_style(ActionStyle.Destructive)
            .with_tooltip("Submits data")
            .with_is_enabled(True)
            .with_mode(ActionMode.Secondary)
            .build()
        )
        assert a["type"] == "Action.Submit"
        assert a["title"] == "Send"
        assert a["data"] == {"key": "value"}
        assert a["associatedInputs"] == "Auto"
        assert a["iconUrl"] == "https://example.com/icon.png"
        assert a["id"] == "submit1"
        assert a["style"] == "destructive"
        assert a["tooltip"] == "Submits data"
        assert a["isEnabled"] is True
        assert a["mode"] == "secondary"

    def test_execute_action_all_properties(self):
        a = (
            ActionBuilder()
            .execute("Run")
            .with_verb("doSomething")
            .with_data({"param": 1})
            .with_associated_inputs(AssociatedInputs.None_)
            .with_icon_url("https://example.com/icon.png")
            .with_id("exec1")
            .with_style(ActionStyle.Positive)
            .with_tooltip("Executes an action")
            .with_is_enabled(True)
            .with_mode(ActionMode.Primary)
            .build()
        )
        assert a["type"] == "Action.Execute"
        assert a["title"] == "Run"
        assert a["verb"] == "doSomething"
        assert a["data"] == {"param": 1}
        assert a["associatedInputs"] == "None"
        assert a["iconUrl"] == "https://example.com/icon.png"
        assert a["id"] == "exec1"
        assert a["style"] == "positive"
        assert a["tooltip"] == "Executes an action"
        assert a["isEnabled"] is True
        assert a["mode"] == "primary"

    def test_show_card_action_all_properties(self):
        inner_card = (
            AdaptiveCardBuilder.create()
            .add_text_block(lambda tb: tb.with_text("Inner card"))
            .build()
        )
        a = (
            ActionBuilder()
            .show_card("Details")
            .with_card(inner_card)
            .with_icon_url("https://example.com/icon.png")
            .with_id("showcard1")
            .with_style(ActionStyle.Default)
            .with_tooltip("Shows a card")
            .with_is_enabled(True)
            .with_mode(ActionMode.Secondary)
            .build()
        )
        assert a["type"] == "Action.ShowCard"
        assert a["title"] == "Details"
        assert a["card"]["type"] == "AdaptiveCard"
        assert a["iconUrl"] == "https://example.com/icon.png"
        assert a["id"] == "showcard1"
        assert a["style"] == "default"
        assert a["tooltip"] == "Shows a card"
        assert a["isEnabled"] is True
        assert a["mode"] == "secondary"

    def test_toggle_visibility_action_all_properties(self):
        a = (
            ActionBuilder()
            .toggle_visibility("Toggle")
            .add_target_element("elem1", True)
            .add_target_element("elem2", False)
            .with_icon_url("https://example.com/icon.png")
            .with_id("toggle1")
            .with_style(ActionStyle.Positive)
            .with_tooltip("Toggles visibility")
            .with_is_enabled(True)
            .with_mode(ActionMode.Primary)
            .build()
        )
        assert a["type"] == "Action.ToggleVisibility"
        assert a["title"] == "Toggle"
        assert len(a["targetElements"]) == 2
        assert a["iconUrl"] == "https://example.com/icon.png"
        assert a["id"] == "toggle1"
        assert a["style"] == "positive"
        assert a["tooltip"] == "Toggles visibility"
        assert a["isEnabled"] is True
        assert a["mode"] == "primary"


class TestEnumSchemaConformance:
    def test_text_size_enum(self):
        values = [m.value for m in TextSize]
        assert "small" in values
        assert "default" in values
        assert "medium" in values
        assert "large" in values
        assert "extraLarge" in values
        assert len(TextSize) == 5

    def test_text_weight_enum(self):
        values = [m.value for m in TextWeight]
        assert "lighter" in values
        assert "default" in values
        assert "bolder" in values
        assert len(TextWeight) == 3

    def test_text_color_enum(self):
        values = [m.value for m in TextColor]
        assert "default" in values
        assert "dark" in values
        assert "light" in values
        assert "accent" in values
        assert "good" in values
        assert "warning" in values
        assert "attention" in values
        assert len(TextColor) == 7

    def test_font_type_enum(self):
        values = [m.value for m in FontType]
        assert "default" in values
        assert "monospace" in values
        assert len(FontType) == 2

    def test_text_block_style_enum(self):
        values = [m.value for m in TextBlockStyle]
        assert "default" in values
        assert "heading" in values
        assert len(TextBlockStyle) == 2

    def test_horizontal_alignment_enum(self):
        values = [m.value for m in HorizontalAlignment]
        assert "left" in values
        assert "center" in values
        assert "right" in values
        assert len(HorizontalAlignment) == 3

    def test_vertical_alignment_enum(self):
        values = [m.value for m in VerticalAlignment]
        assert "top" in values
        assert "center" in values
        assert "bottom" in values
        assert len(VerticalAlignment) == 3

    def test_image_size_enum(self):
        values = [m.value for m in ImageSize]
        assert "auto" in values
        assert "stretch" in values
        assert "small" in values
        assert "medium" in values
        assert "large" in values
        assert len(ImageSize) == 5

    def test_image_style_enum(self):
        values = [m.value for m in ImageStyle]
        assert "default" in values
        assert "person" in values
        assert len(ImageStyle) == 2

    def test_container_style_enum(self):
        values = [m.value for m in ContainerStyle]
        assert "default" in values
        assert "emphasis" in values
        assert "good" in values
        assert "attention" in values
        assert "warning" in values
        assert "accent" in values
        assert len(ContainerStyle) == 6

    def test_spacing_enum(self):
        values = [m.value for m in Spacing]
        assert "default" in values
        assert "none" in values
        assert "small" in values
        assert "medium" in values
        assert "large" in values
        assert "extraLarge" in values
        assert "padding" in values
        assert len(Spacing) == 7

    def test_action_style_enum(self):
        values = [m.value for m in ActionStyle]
        assert "default" in values
        assert "positive" in values
        assert "destructive" in values
        assert len(ActionStyle) == 3

    def test_action_mode_enum(self):
        values = [m.value for m in ActionMode]
        assert "primary" in values
        assert "secondary" in values
        assert len(ActionMode) == 2

    def test_associated_inputs_enum(self):
        values = [m.value for m in AssociatedInputs]
        assert "Auto" in values
        assert "None" in values
        assert len(AssociatedInputs) == 2

    def test_choice_input_style_enum(self):
        values = [m.value for m in ChoiceInputStyle]
        assert "compact" in values
        assert "expanded" in values
        assert "filtered" in values
        assert len(ChoiceInputStyle) == 3

    def test_text_input_style_enum(self):
        values = [m.value for m in TextInputStyle]
        assert "text" in values
        assert "tel" in values
        assert "url" in values
        assert "email" in values
        assert "password" in values
        assert len(TextInputStyle) == 5

    def test_input_label_position_enum(self):
        values = [m.value for m in InputLabelPosition]
        assert "inline" in values
        assert "above" in values
        assert len(InputLabelPosition) == 2

    def test_background_image_fill_mode_enum(self):
        values = [m.value for m in BackgroundImageFillMode]
        assert "cover" in values
        assert "repeatHorizontally" in values
        assert "repeatVertically" in values
        assert "repeat" in values
        assert len(BackgroundImageFillMode) == 4

    def test_input_style_enum(self):
        values = [m.value for m in InputStyle]
        assert "default" in values
        assert "revealOnHover" in values
        assert len(InputStyle) == 2

    def test_block_element_height_enum(self):
        values = [m.value for m in BlockElementHeight]
        assert "auto" in values
        assert "stretch" in values
        assert len(BlockElementHeight) == 2


class TestAdvancedFeaturesSchemaConformance:
    def test_refresh_all_properties(self):
        card = (
            AdaptiveCardBuilder.create()
            .with_refresh(
                lambda r: r.with_action(
                    lambda a: a.execute("Refresh").with_verb("refresh")
                )
                .add_user_id("user1")
                .add_user_id("user2")
                .with_expires("2025-12-31T23:59:59Z")
            )
            .add_text_block(lambda tb: tb.with_text("Refreshable"))
            .build()
        )
        refresh = card["refresh"]
        assert refresh["action"]["type"] == "Action.Execute"
        assert refresh["action"]["verb"] == "refresh"
        assert "user1" in refresh["userIds"]
        assert "user2" in refresh["userIds"]
        assert refresh["expires"] == "2025-12-31T23:59:59Z"

    def test_authentication_all_properties(self):
        card = (
            AdaptiveCardBuilder.create()
            .with_authentication(
                lambda auth: auth.with_text("Please sign in")
                .with_connection_name("myConnection")
                .with_token_exchange_resource(
                    {"id": "ter1", "uri": "https://example.com/token", "providerId": "provider1"}
                )
                .add_button({"type": "signin", "title": "Sign In", "value": "https://example.com/signin"})
            )
            .add_text_block(lambda tb: tb.with_text("Auth card"))
            .build()
        )
        auth = card["authentication"]
        assert auth["text"] == "Please sign in"
        assert auth["connectionName"] == "myConnection"
        assert auth["tokenExchangeResource"]["id"] == "ter1"
        assert auth["tokenExchangeResource"]["uri"] == "https://example.com/token"
        assert auth["tokenExchangeResource"]["providerId"] == "provider1"
        assert len(auth["buttons"]) == 1
        assert auth["buttons"][0]["title"] == "Sign In"

    def test_metadata_web_url(self):
        card = (
            AdaptiveCardBuilder.create()
            .with_metadata("https://example.com/card")
            .build()
        )
        assert card["metadata"] == {"webUrl": "https://example.com/card"}

    def test_background_image_all_properties(self):
        card = (
            AdaptiveCardBuilder.create()
            .with_background_image(
                lambda bg: bg.with_url("https://example.com/bg.png")
                .with_fill_mode(BackgroundImageFillMode.Cover)
                .with_horizontal_alignment(HorizontalAlignment.Right)
                .with_vertical_alignment(VerticalAlignment.Bottom)
            )
            .build()
        )
        bg = card["backgroundImage"]
        assert bg["url"] == "https://example.com/bg.png"
        assert bg["fillMode"] == "cover"
        assert bg["horizontalAlignment"] == "right"
        assert bg["verticalAlignment"] == "bottom"

    def test_data_query_choices_data(self):
        card = (
            AdaptiveCardBuilder.create()
            .add_input_choice_set(
                lambda i: i.with_id("people1")
                .with_label("Select person")
                .with_choices_data("graph.microsoft.com/users")
            )
            .build()
        )
        inp = card["body"][0]
        assert inp["type"] == "Input.ChoiceSet"
        assert inp["choices.data"]["dataset"] == "graph.microsoft.com/users"

    def test_adaptive_card_all_top_level_properties(self):
        card = (
            AdaptiveCardBuilder.create()
            .with_version("1.6")
            .with_fallback_text("Cannot render this card")
            .with_speak("This card says hello")
            .with_lang("en-US")
            .with_rtl(True)
            .with_min_height("300px")
            .with_metadata("https://example.com/card")
            .with_vertical_content_alignment(VerticalAlignment.Center)
            .with_background_image(
                lambda bg: bg.with_url("https://example.com/bg.png")
            )
            .with_select_action({"type": "Action.OpenUrl", "url": "https://example.com"})
            .with_refresh(
                lambda r: r.with_action(lambda a: a.execute("Refresh").with_verb("refresh"))
            )
            .with_authentication(
                lambda auth: auth.with_text("Sign in")
                .with_connection_name("conn1")
            )
            .add_text_block(lambda tb: tb.with_text("Body content"))
            .build()
        )
        assert card["type"] == "AdaptiveCard"
        assert card["version"] == "1.6"
        assert "$schema" in card
        assert card["fallbackText"] == "Cannot render this card"
        assert card["speak"] == "This card says hello"
        assert card["lang"] == "en-US"
        assert card["rtl"] is True
        assert card["minHeight"] == "300px"
        assert card["metadata"]["webUrl"] == "https://example.com/card"
        assert card["verticalContentAlignment"] == "center"
        assert card["backgroundImage"]["url"] == "https://example.com/bg.png"
        assert card["selectAction"]["type"] == "Action.OpenUrl"
        assert card["refresh"]["action"]["type"] == "Action.Execute"
        assert card["authentication"]["text"] == "Sign in"
        assert len(card["body"]) == 1
        assert card["body"][0]["type"] == "TextBlock"

    def test_input_text_with_inline_action_and_label_props(self):
        card = (
            AdaptiveCardBuilder.create()
            .add_input_text(
                lambda i: i.with_id("search")
                .with_label("Search")
                .with_placeholder("Type here")
                .with_label_position(InputLabelPosition.Above)
                .with_label_width("200px")
                .with_input_style(InputStyle.RevealOnHover)
                .with_inline_action(
                    lambda a: a.submit("Go").with_icon_url("https://example.com/go.png")
                )
            )
            .build()
        )
        inp = card["body"][0]
        assert inp["type"] == "Input.Text"
        assert inp["id"] == "search"
        assert inp["label"] == "Search"
        assert inp["labelPosition"] == "above"
        assert inp["labelWidth"] == "200px"
        assert inp["inputStyle"] == "revealOnHover"
        assert inp["inlineAction"]["type"] == "Action.Submit"
        assert inp["inlineAction"]["iconUrl"] == "https://example.com/go.png"

    def test_input_number_with_label_props(self):
        card = (
            AdaptiveCardBuilder.create()
            .add_input_number(
                lambda i: i.with_id("num2")
                .with_label("Amount")
                .with_label_position(InputLabelPosition.Inline)
                .with_label_width("150px")
                .with_input_style(InputStyle.Default)
            )
            .build()
        )
        inp = card["body"][0]
        assert inp["type"] == "Input.Number"
        assert inp["labelPosition"] == "inline"
        assert inp["labelWidth"] == "150px"
        assert inp["inputStyle"] == "default"
