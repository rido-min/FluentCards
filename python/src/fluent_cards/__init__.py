"""
fluent_cards - Fluent builder API for Adaptive Cards
"""

from .enums import (
    TextSize,
    TextWeight,
    TextColor,
    HorizontalAlignment,
    VerticalAlignment,
    ImageSize,
    ImageStyle,
    ActionStyle,
    ContainerStyle,
    TextInputStyle,
    ChoiceInputStyle,
    AssociatedInputs,
    BackgroundImageFillMode,
    FontType,
    Spacing,
    ActionMode,
    InputLabelPosition,
    InputStyle,
    TextBlockStyle,
    ValidationSeverity,
)

from .serialization import to_json, from_json

from .validation import (
    ValidationIssue,
    AdaptiveCardValidationError,
    validate,
    validate_and_throw,
)

from .builders import (
    AdaptiveCardBuilder,
    TextBlockBuilder,
    ImageBuilder,
    ContainerBuilder,
    ColumnSetBuilder,
    ColumnBuilder,
    FactSetBuilder,
    RichTextBlockBuilder,
    TextRunBuilder,
    ActionSetBuilder,
    MediaBuilder,
    ImageSetBuilder,
    TableBuilder,
    ActionBuilder,
    RefreshBuilder,
    AuthenticationBuilder,
    BackgroundImageBuilder,
    InputTextBuilder,
    InputNumberBuilder,
    InputDateBuilder,
    InputTimeBuilder,
    InputToggleBuilder,
    InputChoiceSetBuilder,
)

from .teams import TeamsAdaptiveCards

__all__ = [
    # Enums
    'TextSize',
    'TextWeight',
    'TextColor',
    'HorizontalAlignment',
    'VerticalAlignment',
    'ImageSize',
    'ImageStyle',
    'ActionStyle',
    'ContainerStyle',
    'TextInputStyle',
    'ChoiceInputStyle',
    'AssociatedInputs',
    'BackgroundImageFillMode',
    'FontType',
    'Spacing',
    'ActionMode',
    'InputLabelPosition',
    'InputStyle',
    'TextBlockStyle',
    'ValidationSeverity',
    # Serialization
    'to_json',
    'from_json',
    # Validation
    'ValidationIssue',
    'AdaptiveCardValidationError',
    'validate',
    'validate_and_throw',
    # Builders
    'AdaptiveCardBuilder',
    'TextBlockBuilder',
    'ImageBuilder',
    'ContainerBuilder',
    'ColumnSetBuilder',
    'ColumnBuilder',
    'FactSetBuilder',
    'RichTextBlockBuilder',
    'TextRunBuilder',
    'ActionSetBuilder',
    'MediaBuilder',
    'ImageSetBuilder',
    'TableBuilder',
    'ActionBuilder',
    'RefreshBuilder',
    'AuthenticationBuilder',
    'BackgroundImageBuilder',
    'InputTextBuilder',
    'InputNumberBuilder',
    'InputDateBuilder',
    'InputTimeBuilder',
    'InputToggleBuilder',
    'InputChoiceSetBuilder',
    # Teams
    'TeamsAdaptiveCards',
]
