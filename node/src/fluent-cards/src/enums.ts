/** Controls the size of text in a TextBlock. */
export enum TextSize {
  Small = 'small',
  Default = 'default',
  Medium = 'medium',
  Large = 'large',
  ExtraLarge = 'extraLarge',
}

/** Controls the weight (boldness) of text in a TextBlock. */
export enum TextWeight {
  Lighter = 'lighter',
  Default = 'default',
  Bolder = 'bolder',
}

/** Controls the color of text in a TextBlock. */
export enum TextColor {
  Default = 'default',
  Dark = 'dark',
  Light = 'light',
  Accent = 'accent',
  Good = 'good',
  Attention = 'attention',
  Warning = 'warning',
}

/** Specifies horizontal alignment of an element. */
export enum HorizontalAlignment {
  Left = 'left',
  Center = 'center',
  Right = 'right',
}

/** Specifies vertical alignment of an element. */
export enum VerticalAlignment {
  Top = 'top',
  Center = 'center',
  Bottom = 'bottom',
}

/** Specifies the size of images in an ImageSet. */
export enum ImageSize {
  Auto = 'auto',
  Stretch = 'stretch',
  Small = 'small',
  Medium = 'medium',
  Large = 'large',
}

/** Specifies the style of an image. */
export enum ImageStyle {
  Default = 'default',
  Person = 'person',
}

/** Controls the style of an action button. */
export enum ActionStyle {
  Default = 'default',
  Positive = 'positive',
  Destructive = 'destructive',
}

/** Specifies the style of a container. */
export enum ContainerStyle {
  Default = 'default',
  Emphasis = 'emphasis',
  Good = 'good',
  Attention = 'attention',
  Warning = 'warning',
  Accent = 'accent',
}

/** Defines the style of a text input. */
export enum TextInputStyle {
  Text = 'text',
  Tel = 'tel',
  Url = 'url',
  Email = 'email',
  Password = 'password',
}

/** Defines the style for rendering a choice set. */
export enum ChoiceInputStyle {
  Compact = 'compact',
  Expanded = 'expanded',
  Filtered = 'filtered',
}

/** Controls which inputs are associated with a submit or execute action. */
export enum AssociatedInputs {
  Auto = 'auto',
  None = 'none',
}

/** Specifies the fill mode for a background image. */
export enum BackgroundImageFillMode {
  Cover = 'cover',
  RepeatHorizontally = 'repeatHorizontally',
  RepeatVertically = 'repeatVertically',
  Repeat = 'repeat',
}

/** Specifies the font type for text rendering. */
export enum FontType {
  Default = 'default',
  Monospace = 'monospace',
}

/** Controls the amount of space between elements. */
export enum Spacing {
  Default = 'default',
  None = 'none',
  Small = 'small',
  Medium = 'medium',
  Large = 'large',
  ExtraLarge = 'extraLarge',
  Padding = 'padding',
}

/** Controls whether an action appears as primary or secondary. */
export enum ActionMode {
  Primary = 'primary',
  Secondary = 'secondary',
}

/** Controls where an input label is placed relative to the input (v1.6+). */
export enum InputLabelPosition {
  Inline = 'inline',
  Above = 'above',
}

/** Controls the visual style of an input (v1.6+). */
export enum InputStyle {
  Default = 'default',
  RevealOnHover = 'revealOnHover',
}

/** Controls the semantic style of a TextBlock (v1.5+). */
export enum TextBlockStyle {
  Default = 'default',
  Heading = 'heading',
}

/** Specifies the severity of a validation issue. */
export enum ValidationSeverity {
  Info = 'info',
  Warning = 'warning',
  Error = 'error',
}
