/// TextSize controls the font size of text in TextBlock and TextRun elements.
public enum TextSize: String {
    case small = "small"
    case `default` = "default"
    case medium = "medium"
    case large = "large"
    case extraLarge = "extraLarge"
}

/// TextWeight controls the font weight of text in TextBlock and TextRun elements.
public enum TextWeight: String {
    case lighter = "lighter"
    case `default` = "default"
    case bolder = "bolder"
}

/// TextColor controls the color of text in TextBlock and TextRun elements.
public enum TextColor: String {
    case `default` = "default"
    case dark = "dark"
    case light = "light"
    case accent = "accent"
    case good = "good"
    case attention = "attention"
    case warning = "warning"
    case white = "white"
}

/// FontType controls the font family used for text rendering.
public enum FontType: String {
    case `default` = "default"
    case monospace = "monospace"
}

/// TextBlockStyle controls the visual style of a TextBlock.
public enum TextBlockStyle: String {
    case `default` = "default"
    case heading = "heading"
}

/// HorizontalAlignment controls horizontal alignment of elements.
public enum HorizontalAlignment: String {
    case left = "left"
    case center = "center"
    case right = "right"
}

/// VerticalAlignment controls vertical alignment of content within containers.
public enum VerticalAlignment: String {
    case top = "top"
    case center = "center"
    case bottom = "bottom"
}

/// Spacing controls the spacing before an element.
public enum Spacing: String {
    case `default` = "default"
    case none = "none"
    case small = "small"
    case medium = "medium"
    case large = "large"
    case extraLarge = "extraLarge"
    case padding = "padding"
}

/// ContainerStyle controls the visual style of Container and Column elements.
public enum ContainerStyle: String {
    case `default` = "default"
    case emphasis = "emphasis"
    case good = "good"
    case attention = "attention"
    case warning = "warning"
    case accent = "accent"
}

/// ImageSize controls the display size of Image elements.
public enum ImageSize: String {
    case auto = "auto"
    case stretch = "stretch"
    case small = "small"
    case medium = "medium"
    case large = "large"
}

/// ImageStyle controls the shape/style of Image elements.
public enum ImageStyle: String {
    case `default` = "default"
    case person = "person"
}

/// ActionStyle controls the visual style of action buttons.
public enum ActionStyle: String {
    case `default` = "default"
    case positive = "positive"
    case destructive = "destructive"
}

/// ActionMode controls whether an action appears in the primary or overflow menu.
public enum ActionMode: String {
    case primary = "primary"
    case secondary = "secondary"
}

/// TextInputStyle controls the keyboard type shown for Input.Text on mobile devices.
public enum TextInputStyle: String {
    case text = "text"
    case tel = "tel"
    case url = "url"
    case email = "email"
    case password = "password"
}

/// ChoiceInputStyle controls the display style of Input.ChoiceSet.
public enum ChoiceInputStyle: String {
    case compact = "compact"
    case expanded = "expanded"
    case filtered = "filtered"
}

/// InputLabelPosition controls where the label is rendered relative to the input.
public enum InputLabelPosition: String {
    case inline = "inline"
    case above = "above"
}

/// InputStyle controls how password-style inputs reveal their content.
public enum InputStyle: String {
    case `default` = "default"
    case revealOnHover = "revealOnHover"
}

/// AssociatedInputs controls which inputs are submitted with an action.
public enum AssociatedInputs: String {
    case auto = "auto"
    case none = "none"
}

/// BackgroundImageFillMode controls how a background image fills its container.
public enum BackgroundImageFillMode: String {
    case cover = "cover"
    case repeatHorizontally = "repeatHorizontally"
    case repeatVertically = "repeatVertically"
    case `repeat` = "repeat"
}

/// ValidationSeverity indicates the severity level of a validation issue.
public enum ValidationSeverity: String {
    case info = "info"
    case warning = "warning"
    case error = "error"
}
