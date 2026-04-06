from enum import Enum


class TextSize(str, Enum):
    """Controls the size of text in a TextBlock."""

    Small = "small"
    Default = "default"
    Medium = "medium"
    Large = "large"
    ExtraLarge = "extraLarge"


class TextWeight(str, Enum):
    """Controls the weight (boldness) of text in a TextBlock."""

    Lighter = "lighter"
    Default = "default"
    Bolder = "bolder"


class TextColor(str, Enum):
    """Controls the color of text in a TextBlock or TextRun."""

    Default = "default"
    Dark = "dark"
    Light = "light"
    Accent = "accent"
    Good = "good"
    Attention = "attention"
    Warning = "warning"


class HorizontalAlignment(str, Enum):
    """Controls the horizontal alignment of an element."""

    Left = "left"
    Center = "center"
    Right = "right"


class VerticalAlignment(str, Enum):
    """Controls the vertical alignment of content within a container."""

    Top = "top"
    Center = "center"
    Bottom = "bottom"


class ImageSize(str, Enum):
    """Controls the size of an Image element."""

    Auto = "auto"
    Stretch = "stretch"
    Small = "small"
    Medium = "medium"
    Large = "large"


class ImageStyle(str, Enum):
    """Controls the display style of an Image element."""

    Default = "default"
    Person = "person"


class ActionStyle(str, Enum):
    """Controls the visual style of an action button."""

    Default = "default"
    Positive = "positive"
    Destructive = "destructive"


class ContainerStyle(str, Enum):
    """Controls the visual style of a Container or Column."""

    Default = "default"
    Emphasis = "emphasis"
    Good = "good"
    Attention = "attention"
    Warning = "warning"
    Accent = "accent"


class TextInputStyle(str, Enum):
    """Controls the input style hint for an Input.Text element."""

    Text = "text"
    Tel = "tel"
    Url = "url"
    Email = "email"
    Password = "password"


class ChoiceInputStyle(str, Enum):
    """Controls the display style of an Input.ChoiceSet element."""

    Compact = "compact"
    Expanded = "expanded"
    Filtered = "filtered"


class AssociatedInputs(str, Enum):
    """Controls which inputs are submitted when an action is invoked."""

    Auto = "auto"
    None_ = "none"


class BackgroundImageFillMode(str, Enum):
    """Controls how a background image is tiled or scaled."""

    Cover = "cover"
    RepeatHorizontally = "repeatHorizontally"
    RepeatVertically = "repeatVertically"
    Repeat = "repeat"


class FontType(str, Enum):
    """Controls the font type used to display text."""

    Default = "default"
    Monospace = "monospace"


class Spacing(str, Enum):
    """Controls the amount of spacing before an element."""

    Default = "default"
    None_ = "none"
    Small = "small"
    Medium = "medium"
    Large = "large"
    ExtraLarge = "extraLarge"
    Padding = "padding"


class ActionMode(str, Enum):
    """Controls whether an action appears as a primary or secondary action."""

    Primary = "primary"
    Secondary = "secondary"


class InputLabelPosition(str, Enum):
    """Controls the position of the label relative to an input field."""

    Inline = "inline"
    Above = "above"


class InputStyle(str, Enum):
    """Controls the visual style of an input element."""

    Default = "default"
    RevealOnHover = "revealOnHover"


class TextBlockStyle(str, Enum):
    """Controls the display style of a TextBlock element."""

    Default = "default"
    Heading = "heading"


class ValidationSeverity(str, Enum):
    """Indicates the severity level of a validation issue."""

    Info = "info"
    Warning = "warning"
    Error = "error"
