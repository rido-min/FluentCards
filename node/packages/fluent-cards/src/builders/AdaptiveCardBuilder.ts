import type {
  AdaptiveCard,
  AdaptiveAction,
  AdaptiveElement,
  BackgroundImage,
  CardMetadata,
  Column,
  RefreshConfiguration,
} from '../models.js';
import { VerticalAlignment } from '../enums.js';
import { TextBlockBuilder } from './TextBlockBuilder.js';
import { ImageBuilder } from './ImageBuilder.js';
import { ContainerBuilder } from './ContainerBuilder.js';
import { ColumnSetBuilder } from './ColumnSetBuilder.js';
import { FactSetBuilder } from './FactSetBuilder.js';
import { RichTextBlockBuilder } from './RichTextBlockBuilder.js';
import { ActionSetBuilder } from './ActionSetBuilder.js';
import { MediaBuilder } from './MediaBuilder.js';
import { ImageSetBuilder } from './ImageSetBuilder.js';
import { TableBuilder } from './TableBuilder.js';
import { ActionBuilder } from './ActionBuilder.js';
import { RefreshBuilder } from './RefreshBuilder.js';
import { AuthenticationBuilder } from './AuthenticationBuilder.js';
import { InputTextBuilder } from './inputs/InputTextBuilder.js';
import { InputNumberBuilder } from './inputs/InputNumberBuilder.js';
import { InputDateBuilder } from './inputs/InputDateBuilder.js';
import { InputTimeBuilder } from './inputs/InputTimeBuilder.js';
import { InputToggleBuilder } from './inputs/InputToggleBuilder.js';
import { InputChoiceSetBuilder } from './inputs/InputChoiceSetBuilder.js';

/** Fluent builder for creating {@link AdaptiveCard} instances. */
export class AdaptiveCardBuilder {
  private static readonly SCHEMA_URLS: Record<string, string> = {
    '1.0': 'https://adaptivecards.io/schemas/1.0.0/adaptive-card.json',
    '1.1': 'https://adaptivecards.io/schemas/1.1.0/adaptive-card.json',
    '1.2': 'https://adaptivecards.io/schemas/1.2.0/adaptive-card.json',
    '1.3': 'https://adaptivecards.io/schemas/1.3.0/adaptive-card.json',
    '1.4': 'https://adaptivecards.io/schemas/1.4.0/adaptive-card.json',
    '1.5': 'https://adaptivecards.io/schemas/1.5.0/adaptive-card.json',
    '1.6': 'https://adaptivecards.io/schemas/1.6.0/adaptive-card.json',
  };

  private readonly card: AdaptiveCard = {
    type: 'AdaptiveCard',
    version: '1.5',
    '$schema': AdaptiveCardBuilder.SCHEMA_URLS['1.5'],
  };

  /** Creates a new AdaptiveCardBuilder instance. @returns A new AdaptiveCardBuilder. */
  static create(): AdaptiveCardBuilder {
    return new AdaptiveCardBuilder();
  }

  /** Sets the Adaptive Cards schema version and updates the schema URL automatically. @param version The version string, e.g. `'1.5'`. @returns The builder instance for method chaining. */
  withVersion(version: string): this {
    this.card.version = version;
    this.card['$schema'] = AdaptiveCardBuilder.SCHEMA_URLS[version]
      ?? 'http://adaptivecards.io/schemas/adaptive-card.json';
    return this;
  }

  /** Overrides the schema URL. @param schema The schema URL, or `undefined` to remove it. @returns The builder instance for method chaining. */
  withSchema(schema: string | undefined): this {
    this.card['$schema'] = schema;
    return this;
  }

  /** Sets the fallback text shown when the card cannot be rendered. @param fallbackText The fallback text. @returns The builder instance for method chaining. */
  withFallbackText(fallbackText: string): this {
    this.card.fallbackText = fallbackText;
    return this;
  }

  /** Sets the text that a screen reader should speak for the card. @param speak The spoken text. @returns The builder instance for method chaining. */
  withSpeak(speak: string): this {
    this.card.speak = speak;
    return this;
  }

  /** Sets the locale for the card (e.g. `'en-US'`). @param lang The BCP-47 language tag. @returns The builder instance for method chaining. */
  withLang(lang: string): this {
    this.card.lang = lang;
    return this;
  }

  /** Sets the right-to-left text direction. @param rtl True to enable RTL. @returns The builder instance for method chaining. */
  withRtl(rtl = true): this {
    this.card.rtl = rtl;
    return this;
  }

  /** Sets the minimum height of the card. @param minHeight The minimum height (e.g. `'100px'`). @returns The builder instance for method chaining. */
  withMinHeight(minHeight: string): this {
    this.card.minHeight = minHeight;
    return this;
  }

  /** Sets the vertical alignment of the card body content. @param alignment The vertical alignment. @returns The builder instance for method chaining. */
  withVerticalContentAlignment(alignment: VerticalAlignment): this {
    this.card.verticalContentAlignment = alignment;
    return this;
  }

  /** Sets the background image of the card. @param backgroundImage The background image configuration. @returns The builder instance for method chaining. */
  withBackgroundImage(backgroundImage: BackgroundImage): this {
    this.card.backgroundImage = backgroundImage;
    return this;
  }

  /** Sets the action to invoke when the card is selected. @param action The select action. @returns The builder instance for method chaining. */
  withSelectAction(action: AdaptiveAction): this {
    this.card.selectAction = action;
    return this;
  }

  /** Sets the card metadata (e.g. the web URL for deep linking). @param webUrl The web URL for the card. @returns The builder instance for method chaining. */
  withMetadata(webUrl: string): this {
    this.card.metadata = { webUrl };
    return this;
  }

  // ─── Body elements ────────────────────────────────────────────────────────

  /** Adds a TextBlock element. @param configure A callback to configure the TextBlockBuilder. @returns The builder instance for method chaining. */
  addTextBlock(configure: (b: TextBlockBuilder) => void): this {
    const b = new TextBlockBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  /** Adds an Image element. @param configure A callback to configure the ImageBuilder. @returns The builder instance for method chaining. */
  addImage(configure: (b: ImageBuilder) => void): this {
    const b = new ImageBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  /** Adds a Container element. @param configure A callback to configure the ContainerBuilder. @returns The builder instance for method chaining. */
  addContainer(configure: (b: ContainerBuilder) => void): this {
    const b = new ContainerBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  /** Adds a ColumnSet element. @param configure A callback to configure the ColumnSetBuilder. @returns The builder instance for method chaining. */
  addColumnSet(configure: (b: ColumnSetBuilder) => void): this {
    const b = new ColumnSetBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  /** Adds a FactSet element. @param configure A callback to configure the FactSetBuilder. @returns The builder instance for method chaining. */
  addFactSet(configure: (b: FactSetBuilder) => void): this {
    const b = new FactSetBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  /** Adds a RichTextBlock element. @param configure A callback to configure the RichTextBlockBuilder. @returns The builder instance for method chaining. */
  addRichTextBlock(configure: (b: RichTextBlockBuilder) => void): this {
    const b = new RichTextBlockBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  /** Adds an ActionSet element. @param configure A callback to configure the ActionSetBuilder. @returns The builder instance for method chaining. */
  addActionSet(configure: (b: ActionSetBuilder) => void): this {
    const b = new ActionSetBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  /** Adds a Media element. @param configure A callback to configure the MediaBuilder. @returns The builder instance for method chaining. */
  addMedia(configure: (b: MediaBuilder) => void): this {
    const b = new MediaBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  /** Adds an ImageSet element. @param configure A callback to configure the ImageSetBuilder. @returns The builder instance for method chaining. */
  addImageSet(configure: (b: ImageSetBuilder) => void): this {
    const b = new ImageSetBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  /** Adds a Table element. @param configure A callback to configure the TableBuilder. @returns The builder instance for method chaining. */
  addTable(configure: (b: TableBuilder) => void): this {
    const b = new TableBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  // ─── Input elements ───────────────────────────────────────────────────────

  /** Adds an Input.Text element. @param configure A callback to configure the InputTextBuilder. @returns The builder instance for method chaining. */
  addInputText(configure: (b: InputTextBuilder) => void): this {
    const b = new InputTextBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  /** Adds an Input.Number element. @param configure A callback to configure the InputNumberBuilder. @returns The builder instance for method chaining. */
  addInputNumber(configure: (b: InputNumberBuilder) => void): this {
    const b = new InputNumberBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  /** Adds an Input.Date element. @param configure A callback to configure the InputDateBuilder. @returns The builder instance for method chaining. */
  addInputDate(configure: (b: InputDateBuilder) => void): this {
    const b = new InputDateBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  /** Adds an Input.Time element. @param configure A callback to configure the InputTimeBuilder. @returns The builder instance for method chaining. */
  addInputTime(configure: (b: InputTimeBuilder) => void): this {
    const b = new InputTimeBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  /** Adds an Input.Toggle element. @param configure A callback to configure the InputToggleBuilder. @returns The builder instance for method chaining. */
  addInputToggle(configure: (b: InputToggleBuilder) => void): this {
    const b = new InputToggleBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  /** Adds an Input.ChoiceSet element. @param configure A callback to configure the InputChoiceSetBuilder. @returns The builder instance for method chaining. */
  addInputChoiceSet(configure: (b: InputChoiceSetBuilder) => void): this {
    const b = new InputChoiceSetBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  /** Add a pre-built element directly to the card body. */
  addElement(element: AdaptiveElement): this {
    this.pushBody(element);
    return this;
  }

  // ─── Actions ─────────────────────────────────────────────────────────────

  /** Adds an action to the card's action bar. @param configure A callback to configure the ActionBuilder. @returns The builder instance for method chaining. */
  addAction(configure: (b: ActionBuilder) => void): this {
    const b = new ActionBuilder();
    configure(b);
    (this.card.actions ??= []).push(b.build());
    return this;
  }

  // ─── Advanced configuration ───────────────────────────────────────────────

  /** Configures the automatic refresh settings. @param configure A callback to configure the RefreshBuilder. @returns The builder instance for method chaining. */
  withRefresh(configure: (b: RefreshBuilder) => void): this {
    const b = new RefreshBuilder();
    configure(b);
    this.card.refresh = b.build();
    return this;
  }

  /** Configures authentication for the card. @param configure A callback to configure the AuthenticationBuilder. @returns The builder instance for method chaining. */
  withAuthentication(configure: (b: AuthenticationBuilder) => void): this {
    const b = new AuthenticationBuilder();
    configure(b);
    this.card.authentication = b.build();
    return this;
  }

  /** Builds and returns the configured AdaptiveCard. @returns The configured AdaptiveCard instance. */
  build(): AdaptiveCard {
    return this.card;
  }

  private pushBody(element: AdaptiveElement): void {
    (this.card.body ??= []).push(element);
  }
}
