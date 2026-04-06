// ─── Models & types ───────────────────────────────────────────────────────────
export type {
  AdaptiveCard,
  AdaptiveElement,
  AdaptiveElementBase,
  AdaptiveAction,
  AdaptiveActionBase,
  TextBlock,
  Image,
  Container,
  ColumnSet,
  Column,
  FactSet,
  Fact,
  RichTextBlock,
  TextRun,
  ActionSet,
  Media,
  MediaSource,
  ImageSet,
  Table,
  TableRow,
  TableCell,
  TableColumnDefinition,
  InputElementBase,
  InputText,
  InputNumber,
  InputDate,
  InputTime,
  InputToggle,
  InputChoiceSet,
  Choice,
  OpenUrlAction,
  SubmitAction,
  ShowCardAction,
  ToggleVisibilityAction,
  ExecuteAction,
  TargetElement,
  BackgroundImage,
  RefreshConfiguration,
  AuthenticationConfiguration,
  TokenExchangeResource,
  AuthCardButton,
  CardMetadata,
} from './models.js';

// ─── Enums ────────────────────────────────────────────────────────────────────
export {
  TextSize,
  TextWeight,
  TextColor,
  HorizontalAlignment,
  VerticalAlignment,
  ImageSize,
  ImageStyle,
  ActionStyle,
  ActionMode,
  ContainerStyle,
  FontType,
  TextBlockStyle,
  TextInputStyle,
  ChoiceInputStyle,
  AssociatedInputs,
  BackgroundImageFillMode,
  Spacing,
  InputLabelPosition,
  InputStyle,
  ValidationSeverity,
} from './enums.js';

// ─── Builders ─────────────────────────────────────────────────────────────────
export { AdaptiveCardBuilder } from './builders/AdaptiveCardBuilder.js';
export { TextBlockBuilder } from './builders/TextBlockBuilder.js';
export { ImageBuilder } from './builders/ImageBuilder.js';
export { ContainerBuilder } from './builders/ContainerBuilder.js';
export { ColumnSetBuilder } from './builders/ColumnSetBuilder.js';
export { ColumnBuilder } from './builders/ColumnBuilder.js';
export { FactSetBuilder } from './builders/FactSetBuilder.js';
export { RichTextBlockBuilder } from './builders/RichTextBlockBuilder.js';
export { TextRunBuilder } from './builders/TextRunBuilder.js';
export { ActionSetBuilder } from './builders/ActionSetBuilder.js';
export { MediaBuilder } from './builders/MediaBuilder.js';
export { ImageSetBuilder } from './builders/ImageSetBuilder.js';
export { TableBuilder } from './builders/TableBuilder.js';
export { ActionBuilder } from './builders/ActionBuilder.js';
export { BackgroundImageBuilder } from './builders/BackgroundImageBuilder.js';
export { RefreshBuilder } from './builders/RefreshBuilder.js';
export { AuthenticationBuilder } from './builders/AuthenticationBuilder.js';
export { InputTextBuilder } from './builders/inputs/InputTextBuilder.js';
export { InputNumberBuilder } from './builders/inputs/InputNumberBuilder.js';
export { InputDateBuilder } from './builders/inputs/InputDateBuilder.js';
export { InputTimeBuilder } from './builders/inputs/InputTimeBuilder.js';
export { InputToggleBuilder } from './builders/inputs/InputToggleBuilder.js';
export { InputChoiceSetBuilder } from './builders/inputs/InputChoiceSetBuilder.js';

// ─── Serialization ────────────────────────────────────────────────────────────
export { toJson, fromJson } from './serialization.js';

// ─── Validation ───────────────────────────────────────────────────────────────
export {
  validate,
  validateAndThrow,
  AdaptiveCardValidationError,
} from './validation.js';
export type { ValidationIssue } from './validation.js';

// ─── Teams helpers ────────────────────────────────────────────────────────────
export { TeamsAdaptiveCards } from './TeamsAdaptiveCards.js';
export type {
  ApprovalCardInput,
  StatusUpdateCardInput,
  TaskUpdateCardInput,
  MeetingReminderCardInput,
  ExpenseReportCardInput,
} from './TeamsAdaptiveCards.js';
