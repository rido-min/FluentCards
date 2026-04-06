from enum import Enum


class TextSize(str, Enum):
    Small = "small"
    Default = "default"
    Medium = "medium"
    Large = "large"
    ExtraLarge = "extraLarge"


class TextWeight(str, Enum):
    Lighter = "lighter"
    Default = "default"
    Bolder = "bolder"


class TextColor(str, Enum):
    Default = "default"
    Dark = "dark"
    Light = "light"
    Accent = "accent"
    Good = "good"
    Attention = "attention"
    Warning = "warning"


class HorizontalAlignment(str, Enum):
    Left = "left"
    Center = "center"
    Right = "right"


class VerticalAlignment(str, Enum):
    Top = "top"
    Center = "center"
    Bottom = "bottom"


class ImageSize(str, Enum):
    Auto = "auto"
    Stretch = "stretch"
    Small = "small"
    Medium = "medium"
    Large = "large"


class ImageStyle(str, Enum):
    Default = "default"
    Person = "person"


class ActionStyle(str, Enum):
    Default = "default"
    Positive = "positive"
    Destructive = "destructive"


class ContainerStyle(str, Enum):
    Default = "default"
    Emphasis = "emphasis"
    Good = "good"
    Attention = "attention"
    Warning = "warning"
    Accent = "accent"


class TextInputStyle(str, Enum):
    Text = "text"
    Tel = "tel"
    Url = "url"
    Email = "email"
    Password = "password"


class ChoiceInputStyle(str, Enum):
    Compact = "compact"
    Expanded = "expanded"
    Filtered = "filtered"


class AssociatedInputs(str, Enum):
    Auto = "auto"
    None_ = "none"


class BackgroundImageFillMode(str, Enum):
    Cover = "cover"
    RepeatHorizontally = "repeatHorizontally"
    RepeatVertically = "repeatVertically"
    Repeat = "repeat"


class FontType(str, Enum):
    Default = "default"
    Monospace = "monospace"


class Spacing(str, Enum):
    Default = "default"
    None_ = "none"
    Small = "small"
    Medium = "medium"
    Large = "large"
    ExtraLarge = "extraLarge"
    Padding = "padding"


class ActionMode(str, Enum):
    Primary = "primary"
    Secondary = "secondary"


class InputLabelPosition(str, Enum):
    Inline = "inline"
    Above = "above"


class InputStyle(str, Enum):
    Default = "default"
    RevealOnHover = "revealOnHover"


class TextBlockStyle(str, Enum):
    Default = "default"
    Heading = "heading"


class ValidationSeverity(str, Enum):
    Info = "info"
    Warning = "warning"
    Error = "error"
