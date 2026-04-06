use std::fmt;

macro_rules! string_enum {
    (
        $(#[$meta:meta])*
        $name:ident {
            $( $(#[$vmeta:meta])* $variant:ident => $str:literal ),+ $(,)?
        }
    ) => {
        $(#[$meta])*
        #[derive(Debug, Clone, Copy, PartialEq, Eq, Hash)]
        pub enum $name {
            $( $(#[$vmeta])* $variant ),+
        }

        impl $name {
            /// Returns the string representation used in Adaptive Cards JSON.
            pub fn as_str(&self) -> &'static str {
                match self {
                    $( $name::$variant => $str ),+
                }
            }
        }

        impl AsRef<str> for $name {
            fn as_ref(&self) -> &str {
                self.as_str()
            }
        }

        impl fmt::Display for $name {
            fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
                f.write_str(self.as_str())
            }
        }

        impl From<$name> for serde_json::Value {
            fn from(val: $name) -> serde_json::Value {
                serde_json::Value::String(val.as_str().to_string())
            }
        }
    };
}

string_enum! {
    /// Controls the font size of text in TextBlock and TextRun elements.
    TextSize {
        Small => "small",
        Default => "default",
        Medium => "medium",
        Large => "large",
        ExtraLarge => "extraLarge",
    }
}

string_enum! {
    /// Controls the font weight of text in TextBlock and TextRun elements.
    TextWeight {
        Lighter => "lighter",
        Default => "default",
        Bolder => "bolder",
    }
}

string_enum! {
    /// Controls the color of text in TextBlock and TextRun elements.
    TextColor {
        Default => "default",
        Dark => "dark",
        Light => "light",
        Accent => "accent",
        Good => "good",
        Attention => "attention",
        Warning => "warning",
        White => "white",
    }
}

string_enum! {
    /// Controls the font family used for text rendering.
    FontType {
        Default => "default",
        Monospace => "monospace",
    }
}

string_enum! {
    /// Controls the visual style of a TextBlock.
    TextBlockStyle {
        Default => "default",
        Heading => "heading",
    }
}

string_enum! {
    /// Controls horizontal alignment of elements.
    HorizontalAlignment {
        Left => "left",
        Center => "center",
        Right => "right",
    }
}

string_enum! {
    /// Controls vertical alignment of content within containers.
    VerticalAlignment {
        Top => "top",
        Center => "center",
        Bottom => "bottom",
    }
}

string_enum! {
    /// Controls the spacing before an element.
    Spacing {
        Default => "default",
        None => "none",
        Small => "small",
        Medium => "medium",
        Large => "large",
        ExtraLarge => "extraLarge",
        Padding => "padding",
    }
}

string_enum! {
    /// Controls the visual style of Container and Column elements.
    ContainerStyle {
        Default => "default",
        Emphasis => "emphasis",
        Good => "good",
        Attention => "attention",
        Warning => "warning",
        Accent => "accent",
    }
}

string_enum! {
    /// Controls the display size of Image elements.
    ImageSize {
        Auto => "auto",
        Stretch => "stretch",
        Small => "small",
        Medium => "medium",
        Large => "large",
    }
}

string_enum! {
    /// Controls the shape/style of Image elements.
    ImageStyle {
        Default => "default",
        Person => "person",
    }
}

string_enum! {
    /// Controls the visual style of action buttons.
    ActionStyle {
        Default => "default",
        Positive => "positive",
        Destructive => "destructive",
    }
}

string_enum! {
    /// Controls whether an action appears in the primary or overflow menu.
    ActionMode {
        Primary => "primary",
        Secondary => "secondary",
    }
}

string_enum! {
    /// Controls the keyboard type shown for Input.Text on mobile devices.
    TextInputStyle {
        Text => "text",
        Tel => "tel",
        Url => "url",
        Email => "email",
        Password => "password",
    }
}

string_enum! {
    /// Controls the display style of Input.ChoiceSet.
    ChoiceInputStyle {
        Compact => "compact",
        Expanded => "expanded",
        Filtered => "filtered",
    }
}

string_enum! {
    /// Controls where the label is rendered relative to the input.
    InputLabelPosition {
        Inline => "inline",
        Above => "above",
    }
}

string_enum! {
    /// Controls how password-style inputs reveal their content.
    InputStyle {
        Default => "default",
        RevealOnHover => "revealOnHover",
    }
}

string_enum! {
    /// Controls which inputs are submitted with an action.
    AssociatedInputs {
        Auto => "auto",
        None => "none",
    }
}

string_enum! {
    /// Controls how a background image fills its container.
    BackgroundImageFillMode {
        Cover => "cover",
        RepeatHorizontally => "repeatHorizontally",
        RepeatVertically => "repeatVertically",
        Repeat => "repeat",
    }
}

string_enum! {
    /// Indicates the severity level of a validation issue.
    ValidationSeverity {
        Info => "info",
        Warning => "warning",
        Error => "error",
    }
}
