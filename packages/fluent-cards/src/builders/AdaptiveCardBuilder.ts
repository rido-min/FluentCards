import type {
  AdaptiveCard,
  AdaptiveAction,
  AdaptiveElement,
  AuthenticationConfiguration,
  BackgroundImage,
  CardMetadata,
  Column,
  RefreshConfiguration,
} from '../models.js';
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
  private readonly card: AdaptiveCard = {
    type: 'AdaptiveCard',
    version: '1.5',
    '$schema': 'http://adaptivecards.io/schemas/adaptive-card.json',
  };

  static create(): AdaptiveCardBuilder {
    return new AdaptiveCardBuilder();
  }

  withVersion(version: string): this {
    this.card.version = version;
    return this;
  }

  withSchema(schema: string | undefined): this {
    this.card['$schema'] = schema;
    return this;
  }

  withMetadata(webUrl: string): this {
    this.card.metadata = { webUrl };
    return this;
  }

  // ─── Body elements ────────────────────────────────────────────────────────

  addTextBlock(configure: (b: TextBlockBuilder) => void): this {
    const b = new TextBlockBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  addImage(configure: (b: ImageBuilder) => void): this {
    const b = new ImageBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  addContainer(configure: (b: ContainerBuilder) => void): this {
    const b = new ContainerBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  addColumnSet(configure: (b: ColumnSetBuilder) => void): this {
    const b = new ColumnSetBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  addFactSet(configure: (b: FactSetBuilder) => void): this {
    const b = new FactSetBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  addRichTextBlock(configure: (b: RichTextBlockBuilder) => void): this {
    const b = new RichTextBlockBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  addActionSet(configure: (b: ActionSetBuilder) => void): this {
    const b = new ActionSetBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  addMedia(configure: (b: MediaBuilder) => void): this {
    const b = new MediaBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  addImageSet(configure: (b: ImageSetBuilder) => void): this {
    const b = new ImageSetBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  addTable(configure: (b: TableBuilder) => void): this {
    const b = new TableBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  // ─── Input elements ───────────────────────────────────────────────────────

  addInputText(configure: (b: InputTextBuilder) => void): this {
    const b = new InputTextBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  addInputNumber(configure: (b: InputNumberBuilder) => void): this {
    const b = new InputNumberBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  addInputDate(configure: (b: InputDateBuilder) => void): this {
    const b = new InputDateBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  addInputTime(configure: (b: InputTimeBuilder) => void): this {
    const b = new InputTimeBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  addInputToggle(configure: (b: InputToggleBuilder) => void): this {
    const b = new InputToggleBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  addInputChoiceSet(configure: (b: InputChoiceSetBuilder) => void): this {
    const b = new InputChoiceSetBuilder();
    configure(b);
    this.pushBody(b.build());
    return this;
  }

  // ─── Actions ─────────────────────────────────────────────────────────────

  addAction(configure: (b: ActionBuilder) => void): this {
    const b = new ActionBuilder();
    configure(b);
    (this.card.actions ??= []).push(b.build());
    return this;
  }

  // ─── Advanced configuration ───────────────────────────────────────────────

  withRefresh(configure: (b: RefreshBuilder) => void): this {
    const b = new RefreshBuilder();
    configure(b);
    this.card.refresh = b.build();
    return this;
  }

  withAuthentication(configure: (b: AuthenticationBuilder) => void): this {
    const b = new AuthenticationBuilder();
    configure(b);
    this.card.authentication = b.build();
    return this;
  }

  build(): AdaptiveCard {
    return this.card;
  }

  private pushBody(element: AdaptiveElement): void {
    (this.card.body ??= []).push(element);
  }
}
