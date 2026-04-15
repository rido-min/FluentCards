"""Deep tests for all input builder types."""

import pytest
from fluent_cards import (
    AdaptiveCardBuilder,
    InputTextBuilder,
    InputNumberBuilder,
    InputDateBuilder,
    InputTimeBuilder,
    InputToggleBuilder,
    InputChoiceSetBuilder,
    TextInputStyle,
    ChoiceInputStyle,
    Spacing,
)


class TestInputTextDeep:
    def test_multiline(self):
        inp = InputTextBuilder().with_id("t1").with_is_multiline(True).build()
        assert inp["isMultiline"] is True

    def test_max_length(self):
        inp = InputTextBuilder().with_id("t1").with_max_length(256).build()
        assert inp["maxLength"] == 256

    def test_regex(self):
        inp = InputTextBuilder().with_id("t1").with_regex(r"^\d{3}-\d{4}$").build()
        assert inp["regex"] == r"^\d{3}-\d{4}$"

    def test_placeholder(self):
        inp = InputTextBuilder().with_id("t1").with_placeholder("Type here...").build()
        assert inp["placeholder"] == "Type here..."

    def test_inline_action(self):
        inp = (
            InputTextBuilder()
            .with_id("t1")
            .with_inline_action(lambda a: a.submit("Go"))
            .build()
        )
        assert inp["inlineAction"]["type"] == "Action.Submit"
        assert inp["inlineAction"]["title"] == "Go"

    def test_style_email(self):
        inp = InputTextBuilder().with_id("t1").with_style(TextInputStyle.Email).build()
        assert inp["style"] == "email"

    def test_style_tel(self):
        inp = InputTextBuilder().with_id("t1").with_style(TextInputStyle.Tel).build()
        assert inp["style"] == "tel"

    def test_style_url(self):
        inp = InputTextBuilder().with_id("t1").with_style(TextInputStyle.Url).build()
        assert inp["style"] == "url"

    def test_style_password(self):
        inp = InputTextBuilder().with_id("t1").with_style(TextInputStyle.Password).build()
        assert inp["style"] == "password"

    def test_all_properties_combined(self):
        inp = (
            InputTextBuilder()
            .with_id("full")
            .with_label("Full Name")
            .with_placeholder("Enter full name")
            .with_value("Default")
            .with_max_length(200)
            .with_is_multiline(True)
            .with_style(TextInputStyle.Text)
            .with_regex(r".+")
            .with_is_required(True)
            .with_error_message("Name is required")
            .with_spacing(Spacing.Large)
            .build()
        )
        assert inp["type"] == "Input.Text"
        assert inp["id"] == "full"
        assert inp["label"] == "Full Name"
        assert inp["placeholder"] == "Enter full name"
        assert inp["value"] == "Default"
        assert inp["maxLength"] == 200
        assert inp["isMultiline"] is True
        assert inp["style"] == "text"
        assert inp["regex"] == r".+"
        assert inp["isRequired"] is True
        assert inp["errorMessage"] == "Name is required"
        assert inp["spacing"] == "large"

    def test_unset_optionals_absent(self):
        inp = InputTextBuilder().with_id("t1").build()
        for key in ("label", "placeholder", "value", "maxLength", "isMultiline",
                     "style", "regex", "isRequired", "errorMessage", "inlineAction"):
            assert key not in inp


class TestInputNumberDeep:
    def test_min_max(self):
        inp = InputNumberBuilder().with_id("n1").with_min(0).with_max(100).build()
        assert inp["min"] == 0
        assert inp["max"] == 100

    def test_placeholder(self):
        inp = InputNumberBuilder().with_id("n1").with_placeholder("0-100").build()
        assert inp["placeholder"] == "0-100"

    def test_value(self):
        inp = InputNumberBuilder().with_id("n1").with_value(42).build()
        assert inp["value"] == 42

    def test_negative_min(self):
        inp = InputNumberBuilder().with_id("n1").with_min(-50).with_max(50).build()
        assert inp["min"] == -50

    def test_all_properties_combined(self):
        inp = (
            InputNumberBuilder()
            .with_id("num")
            .with_label("Quantity")
            .with_placeholder("Enter amount")
            .with_value(10)
            .with_min(1)
            .with_max(999)
            .with_is_required(True)
            .with_error_message("Invalid quantity")
            .with_spacing(Spacing.Medium)
            .build()
        )
        assert inp["type"] == "Input.Number"
        assert inp["id"] == "num"
        assert inp["label"] == "Quantity"
        assert inp["value"] == 10
        assert inp["min"] == 1
        assert inp["max"] == 999
        assert inp["isRequired"] is True
        assert inp["errorMessage"] == "Invalid quantity"

    def test_unset_optionals_absent(self):
        inp = InputNumberBuilder().with_id("n1").build()
        for key in ("label", "placeholder", "value", "min", "max",
                     "isRequired", "errorMessage"):
            assert key not in inp


class TestInputDateDeep:
    def test_min_max(self):
        inp = InputDateBuilder().with_id("d1").with_min("2020-01-01").with_max("2025-12-31").build()
        assert inp["min"] == "2020-01-01"
        assert inp["max"] == "2025-12-31"

    def test_placeholder(self):
        inp = InputDateBuilder().with_id("d1").with_placeholder("YYYY-MM-DD").build()
        assert inp["placeholder"] == "YYYY-MM-DD"

    def test_value(self):
        inp = InputDateBuilder().with_id("d1").with_value("2024-06-15").build()
        assert inp["value"] == "2024-06-15"

    def test_all_properties_combined(self):
        inp = (
            InputDateBuilder()
            .with_id("date")
            .with_label("Start date")
            .with_placeholder("Pick a date")
            .with_value("2024-03-01")
            .with_min("2024-01-01")
            .with_max("2024-12-31")
            .with_is_required(True)
            .with_error_message("Date required")
            .build()
        )
        assert inp["type"] == "Input.Date"
        assert inp["id"] == "date"
        assert inp["label"] == "Start date"
        assert inp["value"] == "2024-03-01"
        assert inp["isRequired"] is True

    def test_unset_optionals_absent(self):
        inp = InputDateBuilder().with_id("d1").build()
        for key in ("label", "placeholder", "value", "min", "max",
                     "isRequired", "errorMessage"):
            assert key not in inp


class TestInputTimeDeep:
    def test_min_max(self):
        inp = InputTimeBuilder().with_id("t1").with_min("08:00").with_max("17:00").build()
        assert inp["min"] == "08:00"
        assert inp["max"] == "17:00"

    def test_placeholder(self):
        inp = InputTimeBuilder().with_id("t1").with_placeholder("HH:MM").build()
        assert inp["placeholder"] == "HH:MM"

    def test_value(self):
        inp = InputTimeBuilder().with_id("t1").with_value("12:30").build()
        assert inp["value"] == "12:30"

    def test_all_properties_combined(self):
        inp = (
            InputTimeBuilder()
            .with_id("time")
            .with_label("Meeting time")
            .with_placeholder("Select time")
            .with_value("14:00")
            .with_min("09:00")
            .with_max("18:00")
            .with_is_required(True)
            .with_error_message("Time required")
            .build()
        )
        assert inp["type"] == "Input.Time"
        assert inp["id"] == "time"
        assert inp["label"] == "Meeting time"
        assert inp["value"] == "14:00"
        assert inp["isRequired"] is True

    def test_unset_optionals_absent(self):
        inp = InputTimeBuilder().with_id("t1").build()
        for key in ("label", "placeholder", "value", "min", "max",
                     "isRequired", "errorMessage"):
            assert key not in inp


class TestInputToggleDeep:
    def test_title(self):
        inp = InputToggleBuilder().with_id("tg1").with_title("Enable feature").build()
        assert inp["title"] == "Enable feature"

    def test_value_on_off(self):
        inp = (
            InputToggleBuilder()
            .with_id("tg1")
            .with_title("Toggle")
            .with_value_on("yes")
            .with_value_off("no")
            .build()
        )
        assert inp["valueOn"] == "yes"
        assert inp["valueOff"] == "no"

    def test_value(self):
        inp = InputToggleBuilder().with_id("tg1").with_title("T").with_value("true").build()
        assert inp["value"] == "true"

    def test_wrap(self):
        inp = InputToggleBuilder().with_id("tg1").with_title("T").with_wrap(True).build()
        assert inp["wrap"] is True

    def test_all_properties_combined(self):
        inp = (
            InputToggleBuilder()
            .with_id("toggle")
            .with_title("I agree")
            .with_label("Agreement")
            .with_value("true")
            .with_value_on("true")
            .with_value_off("false")
            .with_wrap(True)
            .with_is_required(True)
            .with_error_message("Must agree")
            .with_spacing(Spacing.Small)
            .build()
        )
        assert inp["type"] == "Input.Toggle"
        assert inp["id"] == "toggle"
        assert inp["title"] == "I agree"
        assert inp["label"] == "Agreement"
        assert inp["value"] == "true"
        assert inp["valueOn"] == "true"
        assert inp["valueOff"] == "false"
        assert inp["wrap"] is True
        assert inp["isRequired"] is True
        assert inp["errorMessage"] == "Must agree"
        assert inp["spacing"] == "small"


class TestInputChoiceSetDeep:
    def test_choices(self):
        inp = (
            InputChoiceSetBuilder()
            .with_id("c1")
            .add_choice("Option A", "a")
            .add_choice("Option B", "b")
            .build()
        )
        assert len(inp["choices"]) == 2
        assert inp["choices"][0] == {"title": "Option A", "value": "a"}
        assert inp["choices"][1] == {"title": "Option B", "value": "b"}

    def test_is_multi_select(self):
        inp = InputChoiceSetBuilder().with_id("c1").is_multi_select(True).build()
        assert inp["isMultiSelect"] is True

    def test_style_compact(self):
        inp = InputChoiceSetBuilder().with_id("c1").with_style(ChoiceInputStyle.Compact).build()
        assert inp["style"] == "compact"

    def test_style_expanded(self):
        inp = InputChoiceSetBuilder().with_id("c1").with_style(ChoiceInputStyle.Expanded).build()
        assert inp["style"] == "expanded"

    def test_style_filtered(self):
        inp = InputChoiceSetBuilder().with_id("c1").with_style(ChoiceInputStyle.Filtered).build()
        assert inp["style"] == "filtered"

    def test_placeholder(self):
        inp = InputChoiceSetBuilder().with_id("c1").with_placeholder("Choose...").build()
        assert inp["placeholder"] == "Choose..."

    def test_choices_data(self):
        inp = (
            InputChoiceSetBuilder()
            .with_id("c1")
            .with_choices_data("graph.microsoft.com/users")
            .build()
        )
        assert inp["choices.data"] == {
            "type": "Data.Query",
            "dataset": "graph.microsoft.com/users",
        }

    def test_dict_choice(self):
        inp = (
            InputChoiceSetBuilder()
            .with_id("c1")
            .add_choice({"title": "Custom", "value": "custom"})
            .build()
        )
        assert inp["choices"][0] == {"title": "Custom", "value": "custom"}

    def test_all_properties_combined(self):
        inp = (
            InputChoiceSetBuilder()
            .with_id("full_choice")
            .with_label("Favorite")
            .with_placeholder("Pick one")
            .with_value("a")
            .with_style(ChoiceInputStyle.Expanded)
            .is_multi_select(True)
            .with_wrap(True)
            .add_choice("A", "a")
            .add_choice("B", "b")
            .with_is_required(True)
            .with_error_message("Pick something")
            .with_spacing(Spacing.Large)
            .build()
        )
        assert inp["type"] == "Input.ChoiceSet"
        assert inp["id"] == "full_choice"
        assert inp["label"] == "Favorite"
        assert inp["placeholder"] == "Pick one"
        assert inp["value"] == "a"
        assert inp["style"] == "expanded"
        assert inp["isMultiSelect"] is True
        assert inp["wrap"] is True
        assert len(inp["choices"]) == 2
        assert inp["isRequired"] is True
        assert inp["errorMessage"] == "Pick something"
        assert inp["spacing"] == "large"


class TestAllInputsCommonProperties:
    """Test common properties shared by all input types."""

    @pytest.mark.parametrize(
        "builder_cls,extra_args",
        [
            (InputTextBuilder, {}),
            (InputNumberBuilder, {}),
            (InputDateBuilder, {}),
            (InputTimeBuilder, {}),
            (InputToggleBuilder, {"title": "T"}),
            (InputChoiceSetBuilder, {}),
        ],
    )
    def test_id(self, builder_cls, extra_args):
        b = builder_cls().with_id("input1")
        if "title" in extra_args:
            b = b.with_title(extra_args["title"])
        inp = b.build()
        assert inp["id"] == "input1"

    @pytest.mark.parametrize(
        "builder_cls,extra_args",
        [
            (InputTextBuilder, {}),
            (InputNumberBuilder, {}),
            (InputDateBuilder, {}),
            (InputTimeBuilder, {}),
            (InputToggleBuilder, {"title": "T"}),
            (InputChoiceSetBuilder, {}),
        ],
    )
    def test_label(self, builder_cls, extra_args):
        b = builder_cls().with_id("input1").with_label("My Label")
        if "title" in extra_args:
            b = b.with_title(extra_args["title"])
        inp = b.build()
        assert inp["label"] == "My Label"

    @pytest.mark.parametrize(
        "builder_cls,extra_args",
        [
            (InputTextBuilder, {}),
            (InputNumberBuilder, {}),
            (InputDateBuilder, {}),
            (InputTimeBuilder, {}),
            (InputToggleBuilder, {"title": "T"}),
            (InputChoiceSetBuilder, {}),
        ],
    )
    def test_is_required(self, builder_cls, extra_args):
        b = builder_cls().with_id("input1").with_is_required(True)
        if "title" in extra_args:
            b = b.with_title(extra_args["title"])
        inp = b.build()
        assert inp["isRequired"] is True

    @pytest.mark.parametrize(
        "builder_cls,extra_args",
        [
            (InputTextBuilder, {}),
            (InputNumberBuilder, {}),
            (InputDateBuilder, {}),
            (InputTimeBuilder, {}),
            (InputToggleBuilder, {"title": "T"}),
            (InputChoiceSetBuilder, {}),
        ],
    )
    def test_error_message(self, builder_cls, extra_args):
        b = builder_cls().with_id("input1").with_error_message("Error!")
        if "title" in extra_args:
            b = b.with_title(extra_args["title"])
        inp = b.build()
        assert inp["errorMessage"] == "Error!"
