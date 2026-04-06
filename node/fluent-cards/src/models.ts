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

export interface ColumnSet extends AdaptiveElementBase {
  type: 'ColumnSet';
  columns?: Column[];
  style?: ContainerStyle;
  bleed?: boolean;
  minHeight?: string;
  horizontalAlignment?: HorizontalAlignment;
  selectAction?: AdaptiveAction;
}

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

export interface FactSet extends AdaptiveElementBase {
  type: 'FactSet';
  facts?: Fact[];
}

export interface Fact {
  title?: string;
  value?: string;
}

export interface RichTextBlock extends AdaptiveElementBase {
  type: 'RichTextBlock';
  inlines?: (string | TextRun)[];
  horizontalAlignment?: HorizontalAlignment;
}

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

export interface ActionSet extends AdaptiveElementBase {
  type: 'ActionSet';
  actions?: AdaptiveAction[];
}

export interface Media extends AdaptiveElementBase {
  type: 'Media';
  sources?: MediaSource[];
  poster?: string;
  altText?: string;
}

export interface MediaSource {
  mimeType?: string;
  url?: string;
}

export interface ImageSet extends AdaptiveElementBase {
  type: 'ImageSet';
  images?: Image[];
  imageSize?: ImageSize;
}

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

export interface TableRow {
  type: 'TableRow';
  cells?: TableCell[];
  style?: ContainerStyle;
  horizontalCellContentAlignment?: HorizontalAlignment;
  verticalCellContentAlignment?: VerticalAlignment;
}

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

export interface TableColumnDefinition {
  width?: string;
  horizontalCellContentAlignment?: HorizontalAlignment;
  verticalCellContentAlignment?: VerticalAlignment;
}

// ─── Input elements ──────────────────────────────────────────────────────────

export interface InputElementBase extends AdaptiveElementBase {
  id: string;
  label?: string;
  isRequired?: boolean;
  errorMessage?: string;
  labelPosition?: InputLabelPosition;
  inputStyle?: InputStyle;
}

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

export interface InputNumber extends InputElementBase {
  type: 'Input.Number';
  min?: number;
  max?: number;
  placeholder?: string;
  value?: number;
}

export interface InputDate extends InputElementBase {
  type: 'Input.Date';
  min?: string;
  max?: string;
  placeholder?: string;
  value?: string;
}

export interface InputTime extends InputElementBase {
  type: 'Input.Time';
  min?: string;
  max?: string;
  placeholder?: string;
  value?: string;
}

export interface InputToggle extends InputElementBase {
  type: 'Input.Toggle';
  title: string;
  value?: string;
  valueOn?: string;
  valueOff?: string;
  wrap?: boolean;
}

export interface InputChoiceSet extends InputElementBase {
  type: 'Input.ChoiceSet';
  choices?: Choice[];
  isMultiSelect?: boolean;
  style?: ChoiceInputStyle;
  value?: string;
  placeholder?: string;
  wrap?: boolean;
}

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

export interface AdaptiveActionBase {
  id?: string;
  title?: string;
  iconUrl?: string;
  style?: ActionStyle;
  mode?: ActionMode;
  isEnabled?: boolean;
  tooltip?: string;
}

export interface OpenUrlAction extends AdaptiveActionBase {
  type: 'Action.OpenUrl';
  url: string;
}

export interface SubmitAction extends AdaptiveActionBase {
  type: 'Action.Submit';
  data?: unknown;
  associatedInputs?: AssociatedInputs;
}

export interface ShowCardAction extends AdaptiveActionBase {
  type: 'Action.ShowCard';
  card?: AdaptiveCard;
}

export interface ToggleVisibilityAction extends AdaptiveActionBase {
  type: 'Action.ToggleVisibility';
  targetElements?: (string | TargetElement)[];
}

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

export interface TargetElement {
  elementId: string;
  isVisible?: boolean;
}

// ─── Advanced configuration models ───────────────────────────────────────────

export interface BackgroundImage {
  url?: string;
  fillMode?: BackgroundImageFillMode;
  horizontalAlignment?: HorizontalAlignment;
  verticalAlignment?: VerticalAlignment;
}

export interface RefreshConfiguration {
  action?: AdaptiveAction;
  userIds?: string[];
  expires?: string;
}

export interface AuthenticationConfiguration {
  text?: string;
  connectionName?: string;
  tokenExchangeResource?: TokenExchangeResource;
  buttons?: AuthCardButton[];
}

export interface TokenExchangeResource {
  id?: string;
  uri?: string;
  providerId?: string;
}

export interface AuthCardButton {
  type: string;
  title?: string;
  image?: string;
  value?: string;
}

export interface CardMetadata {
  webUrl?: string;
}

// ─── Root card ───────────────────────────────────────────────────────────────

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
