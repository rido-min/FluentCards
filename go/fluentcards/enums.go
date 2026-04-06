package fluentcards

// TextSize controls the font size of text in TextBlock and TextRun elements.
type TextSize string

const (
	TextSizeSmall      TextSize = "small"
	TextSizeDefault    TextSize = "default"
	TextSizeMedium     TextSize = "medium"
	TextSizeLarge      TextSize = "large"
	TextSizeExtraLarge TextSize = "extraLarge"
)

// TextWeight controls the font weight of text in TextBlock and TextRun elements.
type TextWeight string

const (
	TextWeightLighter TextWeight = "lighter"
	TextWeightDefault TextWeight = "default"
	TextWeightBolder  TextWeight = "bolder"
)

// TextColor controls the color of text in TextBlock and TextRun elements.
type TextColor string

const (
	TextColorDefault   TextColor = "default"
	TextColorDark      TextColor = "dark"
	TextColorLight     TextColor = "light"
	TextColorAccent    TextColor = "accent"
	TextColorGood      TextColor = "good"
	TextColorAttention TextColor = "attention"
	TextColorWarning   TextColor = "warning"
	TextColorWhite     TextColor = "white"
)

// FontType controls the font family used for text rendering.
type FontType string

const (
	FontTypeDefault   FontType = "default"
	FontTypeMonospace FontType = "monospace"
)

// TextBlockStyle controls the visual style of a TextBlock.
type TextBlockStyle string

const (
	TextBlockStyleDefault TextBlockStyle = "default"
	TextBlockStyleHeading TextBlockStyle = "heading"
)

// HorizontalAlignment controls horizontal alignment of elements.
type HorizontalAlignment string

const (
	HorizontalAlignmentLeft   HorizontalAlignment = "left"
	HorizontalAlignmentCenter HorizontalAlignment = "center"
	HorizontalAlignmentRight  HorizontalAlignment = "right"
)

// VerticalAlignment controls vertical alignment of content within containers.
type VerticalAlignment string

const (
	VerticalAlignmentTop    VerticalAlignment = "top"
	VerticalAlignmentCenter VerticalAlignment = "center"
	VerticalAlignmentBottom VerticalAlignment = "bottom"
)

// Spacing controls the spacing before an element.
type Spacing string

const (
	SpacingDefault    Spacing = "default"
	SpacingNone       Spacing = "none"
	SpacingSmall      Spacing = "small"
	SpacingMedium     Spacing = "medium"
	SpacingLarge      Spacing = "large"
	SpacingExtraLarge Spacing = "extraLarge"
	SpacingPadding    Spacing = "padding"
)

// ContainerStyle controls the visual style of Container and Column elements.
type ContainerStyle string

const (
	ContainerStyleDefault   ContainerStyle = "default"
	ContainerStyleEmphasis  ContainerStyle = "emphasis"
	ContainerStyleGood      ContainerStyle = "good"
	ContainerStyleAttention ContainerStyle = "attention"
	ContainerStyleWarning   ContainerStyle = "warning"
	ContainerStyleAccent    ContainerStyle = "accent"
)

// ImageSize controls the display size of Image elements.
type ImageSize string

const (
	ImageSizeAuto    ImageSize = "auto"
	ImageSizeStretch ImageSize = "stretch"
	ImageSizeSmall   ImageSize = "small"
	ImageSizeMedium  ImageSize = "medium"
	ImageSizeLarge   ImageSize = "large"
)

// ImageStyle controls the shape/style of Image elements.
type ImageStyle string

const (
	ImageStyleDefault ImageStyle = "default"
	ImageStylePerson  ImageStyle = "person"
)

// ActionStyle controls the visual style of action buttons.
type ActionStyle string

const (
	ActionStyleDefault     ActionStyle = "default"
	ActionStylePositive    ActionStyle = "positive"
	ActionStyleDestructive ActionStyle = "destructive"
)

// ActionMode controls whether an action appears in the primary or overflow menu.
type ActionMode string

const (
	ActionModePrimary   ActionMode = "primary"
	ActionModeSecondary ActionMode = "secondary"
)

// TextInputStyle controls the keyboard type shown for Input.Text on mobile devices.
type TextInputStyle string

const (
	TextInputStyleText     TextInputStyle = "text"
	TextInputStyleTel      TextInputStyle = "tel"
	TextInputStyleUrl      TextInputStyle = "url"
	TextInputStyleEmail    TextInputStyle = "email"
	TextInputStylePassword TextInputStyle = "password"
)

// ChoiceInputStyle controls the display style of Input.ChoiceSet.
type ChoiceInputStyle string

const (
	ChoiceInputStyleCompact  ChoiceInputStyle = "compact"
	ChoiceInputStyleExpanded ChoiceInputStyle = "expanded"
	ChoiceInputStyleFiltered ChoiceInputStyle = "filtered"
)

// InputLabelPosition controls where the label is rendered relative to the input.
type InputLabelPosition string

const (
	InputLabelPositionInline InputLabelPosition = "inline"
	InputLabelPositionAbove  InputLabelPosition = "above"
)

// InputStyle controls how password-style inputs reveal their content.
type InputStyle string

const (
	InputStyleDefault       InputStyle = "default"
	InputStyleRevealOnHover InputStyle = "revealOnHover"
)

// AssociatedInputs controls which inputs are submitted with an action.
type AssociatedInputs string

const (
	AssociatedInputsAuto AssociatedInputs = "auto"
	AssociatedInputsNone AssociatedInputs = "none"
)

// BackgroundImageFillMode controls how a background image fills its container.
type BackgroundImageFillMode string

const (
	BackgroundImageFillModeCover             BackgroundImageFillMode = "cover"
	BackgroundImageFillModeRepeatHorizontally BackgroundImageFillMode = "repeatHorizontally"
	BackgroundImageFillModeRepeatVertically   BackgroundImageFillMode = "repeatVertically"
	BackgroundImageFillModeRepeat            BackgroundImageFillMode = "repeat"
)

// ValidationSeverity indicates the severity level of a validation issue.
type ValidationSeverity string

const (
	ValidationSeverityInfo    ValidationSeverity = "info"
	ValidationSeverityWarning ValidationSeverity = "warning"
	ValidationSeverityError   ValidationSeverity = "error"
)
