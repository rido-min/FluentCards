import {
  ActionMode,
  ActionStyle,
  AssociatedInputs,
  BackgroundImageFillMode,
  ChoiceInputStyle,
  ContainerStyle,
  FontType,
  HorizontalAlignment,
  ImageSize,
  ImageStyle,
  InputLabelPosition,
  InputStyle,
  Spacing,
  TextBlockStyle,
  TextColor,
  TextInputStyle,
  TextSize,
  TextWeight,
  VerticalAlignment,
} from './enums.js';

// ─── Shared base ─────────────────────────────────────────────────────────────

/** Base properties shared by all Adaptive Card body elements. */
export interface AdaptiveElementBase {
  id?: string;
  isVisible?: boolean;
  spacing?: Spacing;
  separator?: boolean;
  /** "auto" | "stretch" */
  height?: string;
  /** "drop" or a fallback AdaptiveElement */
  fallback?: 'drop' | AdaptiveElement;
  requires?: Record<string, string>;
  rtl?: boolean;
}

// ─── Concrete element models ─────────────────────────────────────────────────

/** A text element that displays formatted text. */
export interface TextBlock extends AdaptiveElementBase {
  type: 'TextBlock';
  text: string;
  size?: TextSize;
  weight?: TextWeight;
  color?: TextColor;
  isSubtle?: boolean;
  wrap?: boolean;
  maxLines?: number;
  horizontalAlignment?: HorizontalAlignment;
  fontType?: FontType;
  style?: TextBlockStyle;
  selectAction?: AdaptiveAction;
}

/** An image element. */
export interface Image extends AdaptiveElementBase {
  type: 'Image';
  url?: string;
  altText?: string;
  size?: ImageSize;
  style?: ImageStyle;
  horizontalAlignment?: HorizontalAlignment;
  backgroundColor?: string;
  width?: string;
  /** overrides base height */
  height?: string;
  selectAction?: AdaptiveAction;
}

/** A container that groups body elements together. */
export interface Container extends AdaptiveElementBase {
  type: 'Container';
  items?: AdaptiveElement[];
  style?: ContainerStyle;
  verticalContentAlignment?: VerticalAlignment;
  bleed?: boolean;
  minHeight?: string;
  backgroundImage?: BackgroundImage;
  selectAction?: AdaptiveAction;
}

/** A set of columns that divide horizontal space. */
export interface ColumnSet extends AdaptiveElementBase {
  type: 'ColumnSet';
  columns?: Column[];
  style?: ContainerStyle;
  bleed?: boolean;
  minHeight?: string;
  horizontalAlignment?: HorizontalAlignment;
  selectAction?: AdaptiveAction;
}

/** A single column within a {@link ColumnSet}. */
export interface Column {
  type: 'Column';
  items?: AdaptiveElement[];
  width?: string;
  id?: string;
  style?: ContainerStyle;
  verticalContentAlignment?: VerticalAlignment;
  bleed?: boolean;
  minHeight?: string;
  backgroundImage?: BackgroundImage;
  selectAction?: AdaptiveAction;
}

/** A set of name/value pairs displayed in a table layout. */
export interface FactSet extends AdaptiveElementBase {
  type: 'FactSet';
  facts?: Fact[];
}

/** A single name/value pair within a {@link FactSet}. */
export interface Fact {
  title?: string;
  value?: string;
}

/** A block of richly formatted text composed of inline {@link TextRun} elements. */
export interface RichTextBlock extends AdaptiveElementBase {
  type: 'RichTextBlock';
  inlines?: (string | TextRun)[];
  horizontalAlignment?: HorizontalAlignment;
}

/** An inline text run with optional formatting, used inside {@link RichTextBlock}. */
export interface TextRun {
  type: 'TextRun';
  text?: string;
  size?: TextSize;
  weight?: TextWeight;
  color?: TextColor;
  isSubtle?: boolean;
  italic?: boolean;
  strikethrough?: boolean;
  underline?: boolean;
  highlight?: boolean;
  selectAction?: AdaptiveAction;
}

/** A set of actions displayed as a row of buttons within the card body. */
export interface ActionSet extends AdaptiveElementBase {
  type: 'ActionSet';
  actions?: AdaptiveAction[];
}

/** A media element that displays video or audio. */
export interface Media extends AdaptiveElementBase {
  type: 'Media';
  sources?: MediaSource[];
  poster?: string;
  altText?: string;
}

/** A single media source (URL + MIME type) within a {@link Media} element. */
export interface MediaSource {
  mimeType?: string;
  url?: string;
}

/** A collection of images displayed as a gallery. */
export interface ImageSet extends AdaptiveElementBase {
  type: 'ImageSet';
  images?: Image[];
  imageSize?: ImageSize;
}

/** A table element with rows and columns. */
export interface Table extends AdaptiveElementBase {
  type: 'Table';
  columns?: TableColumnDefinition[];
  rows?: TableRow[];
  firstRowAsHeader?: boolean;
  showGridLines?: boolean;
  gridStyle?: ContainerStyle;
  horizontalCellContentAlignment?: HorizontalAlignment;
  verticalCellContentAlignment?: VerticalAlignment;
}

/** A single row within a {@link Table}. */
export interface TableRow {
  type: 'TableRow';
  cells?: TableCell[];
  style?: ContainerStyle;
  horizontalCellContentAlignment?: HorizontalAlignment;
  verticalCellContentAlignment?: VerticalAlignment;
}

/** A single cell within a {@link TableRow}. */
export interface TableCell {
  type: 'TableCell';
  items?: AdaptiveElement[];
  selectAction?: AdaptiveAction;
  style?: ContainerStyle;
  verticalContentAlignment?: VerticalAlignment;
  bleed?: boolean;
  backgroundImage?: BackgroundImage;
  minHeight?: string;
  rtl?: boolean;
}

/** Defines the width and alignment for a single column in a {@link Table}. */
export interface TableColumnDefinition {
  width?: string;
  horizontalCellContentAlignment?: HorizontalAlignment;
  verticalCellContentAlignment?: VerticalAlignment;
}

// ─── Input elements ──────────────────────────────────────────────────────────

/** Base properties shared by all Adaptive Card input elements. */
export interface InputElementBase extends AdaptiveElementBase {
  id: string;
  label?: string;
  isRequired?: boolean;
  errorMessage?: string;
  labelPosition?: InputLabelPosition;
  inputStyle?: InputStyle;
}

/** A single-line or multi-line text input. */
export interface InputText extends InputElementBase {
  type: 'Input.Text';
  isMultiline?: boolean;
  maxLength?: number;
  placeholder?: string;
  value?: string;
  style?: TextInputStyle;
  regex?: string;
  inlineAction?: AdaptiveAction;
}

/** A numeric input field. */
export interface InputNumber extends InputElementBase {
  type: 'Input.Number';
  min?: number;
  max?: number;
  placeholder?: string;
  value?: number;
}

/** A date picker input. */
export interface InputDate extends InputElementBase {
  type: 'Input.Date';
  min?: string;
  max?: string;
  placeholder?: string;
  value?: string;
}

/** A time picker input. */
export interface InputTime extends InputElementBase {
  type: 'Input.Time';
  min?: string;
  max?: string;
  placeholder?: string;
  value?: string;
}

/** A toggle (checkbox) input. */
export interface InputToggle extends InputElementBase {
  type: 'Input.Toggle';
  title: string;
  value?: string;
  valueOn?: string;
  valueOff?: string;
  wrap?: boolean;
}

/** A dynamic data query for fetching choices from a data source (Adaptive Cards 1.6+). */
export interface DataQuery {
  type: 'Data.Query';
  dataset: string;
  count?: number;
  skip?: number;
}

/** A dropdown or multi-select input with predefined choices. */
export interface InputChoiceSet extends InputElementBase {
  type: 'Input.ChoiceSet';
  choices?: Choice[];
  isMultiSelect?: boolean;
  style?: ChoiceInputStyle;
  value?: string;
  placeholder?: string;
  wrap?: boolean;
  'choices.data'?: DataQuery;
}

/** A single choice option within an {@link InputChoiceSet}. */
export interface Choice {
  title: string;
  value: string;
}

/** Union of all body element types. */
export type AdaptiveElement =
  | TextBlock
  | Image
  | Container
  | ColumnSet
  | FactSet
  | RichTextBlock
  | ActionSet
  | Media
  | ImageSet
  | Table
  | InputText
  | InputNumber
  | InputDate
  | InputTime
  | InputToggle
  | InputChoiceSet;

// ─── Action models ────────────────────────────────────────────────────────────

/** Base properties shared by all Adaptive Card action types. */
export interface AdaptiveActionBase {
  id?: string;
  title?: string;
  iconUrl?: string;
  style?: ActionStyle;
  mode?: ActionMode;
  isEnabled?: boolean;
  tooltip?: string;
}

/** An action that opens a URL in a browser. */
export interface OpenUrlAction extends AdaptiveActionBase {
  type: 'Action.OpenUrl';
  url: string;
}

/** An action that submits input data to the host. */
export interface SubmitAction extends AdaptiveActionBase {
  type: 'Action.Submit';
  data?: unknown;
  associatedInputs?: AssociatedInputs;
}

/** An action that reveals an embedded card when clicked. */
export interface ShowCardAction extends AdaptiveActionBase {
  type: 'Action.ShowCard';
  card?: AdaptiveCard;
}

/** An action that toggles the visibility of one or more elements. */
export interface ToggleVisibilityAction extends AdaptiveActionBase {
  type: 'Action.ToggleVisibility';
  targetElements?: (string | TargetElement)[];
}

/** An action that executes a command with a verb on the host (Adaptive Cards Universal Action Model). */
export interface ExecuteAction extends AdaptiveActionBase {
  type: 'Action.Execute';
  verb?: string;
  data?: unknown;
  associatedInputs?: AssociatedInputs;
}

/** Union of all action types. */
export type AdaptiveAction =
  | OpenUrlAction
  | SubmitAction
  | ShowCardAction
  | ToggleVisibilityAction
  | ExecuteAction;

/** Identifies an element whose visibility is toggled by {@link ToggleVisibilityAction}. */
export interface TargetElement {
  elementId: string;
  isVisible?: boolean;
}

// ─── Advanced configuration models ───────────────────────────────────────────

/** Defines a background image with fill mode and alignment options. */
export interface BackgroundImage {
  url?: string;
  fillMode?: BackgroundImageFillMode;
  horizontalAlignment?: HorizontalAlignment;
  verticalAlignment?: VerticalAlignment;
}

/** Configuration for automatic card refresh. */
export interface RefreshConfiguration {
  action?: AdaptiveAction;
  userIds?: string[];
  expires?: string;
}

/** Configuration for OAuth/SSO authentication on the card. */
export interface AuthenticationConfiguration {
  text?: string;
  connectionName?: string;
  tokenExchangeResource?: TokenExchangeResource;
  buttons?: AuthCardButton[];
}

/** Describes the token exchange resource for SSO authentication. */
export interface TokenExchangeResource {
  id?: string;
  uri?: string;
  providerId?: string;
}

/** A button shown in the authentication prompt. */
export interface AuthCardButton {
  type: string;
  title?: string;
  image?: string;
  value?: string;
}

/** Metadata associated with the card, such as the web URL for deep linking. */
export interface CardMetadata {
  webUrl?: string;
}

// ─── Root card ───────────────────────────────────────────────────────────────

/** The root Adaptive Card object. */
export interface AdaptiveCard {
  type: 'AdaptiveCard';
  version: string;
  '$schema'?: string;
  body?: AdaptiveElement[];
  actions?: AdaptiveAction[];
  selectAction?: AdaptiveAction;
  fallbackText?: string;
  speak?: string;
  lang?: string;
  minHeight?: string;
  verticalContentAlignment?: VerticalAlignment;
  backgroundImage?: BackgroundImage;
  rtl?: boolean;
  refresh?: RefreshConfiguration;
  authentication?: AuthenticationConfiguration;
  metadata?: CardMetadata;
}
