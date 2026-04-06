import type { Container, AdaptiveElement } from '../models.js';
import { ContainerStyle, VerticalAlignment, Spacing } from '../enums.js';
import { TextBlockBuilder } from './TextBlockBuilder.js';
import { ImageBuilder } from './ImageBuilder.js';
import { ActionBuilder } from './ActionBuilder.js';
import { BackgroundImageBuilder } from './BackgroundImageBuilder.js';
import { ColumnSetBuilder } from './ColumnSetBuilder.js';
import { FactSetBuilder } from './FactSetBuilder.js';
import { RichTextBlockBuilder } from './RichTextBlockBuilder.js';
import { ActionSetBuilder } from './ActionSetBuilder.js';

/** Fluent builder for {@link Container} elements. */
export class ContainerBuilder {
  private readonly container: Container = { type: 'Container', items: [] };

  withId(id: string): this { this.container.id = id; return this; }
  withStyle(style: ContainerStyle): this { this.container.style = style; return this; }
  withVerticalContentAlignment(alignment: VerticalAlignment): this { this.container.verticalContentAlignment = alignment; return this; }
  withBleed(bleed = true): this { this.container.bleed = bleed; return this; }
  withMinHeight(minHeight: string): this { this.container.minHeight = minHeight; return this; }
  withSpacing(spacing: Spacing): this { this.container.spacing = spacing; return this; }
  withSeparator(separator: boolean): this { this.container.separator = separator; return this; }
  withIsVisible(isVisible: boolean): this { this.container.isVisible = isVisible; return this; }

  withBackgroundImage(configure: (b: BackgroundImageBuilder) => void): this {
    const b = new BackgroundImageBuilder();
    configure(b);
    this.container.backgroundImage = b.build();
    return this;
  }

  withSelectAction(configure: (b: ActionBuilder) => void): this {
    const b = new ActionBuilder();
    configure(b);
    this.container.selectAction = b.build();
    return this;
  }

  addTextBlock(configure: (b: TextBlockBuilder) => void): this {
    const b = new TextBlockBuilder();
    configure(b);
    this.container.items!.push(b.build());
    return this;
  }

  addImage(configure: (b: ImageBuilder) => void): this {
    const b = new ImageBuilder();
    configure(b);
    this.container.items!.push(b.build());
    return this;
  }

  addContainer(configure: (b: ContainerBuilder) => void): this {
    const b = new ContainerBuilder();
    configure(b);
    this.container.items!.push(b.build());
    return this;
  }

  addColumnSet(configure: (b: ColumnSetBuilder) => void): this {
    const b = new ColumnSetBuilder();
    configure(b);
    this.container.items!.push(b.build());
    return this;
  }

  addFactSet(configure: (b: FactSetBuilder) => void): this {
    const b = new FactSetBuilder();
    configure(b);
    this.container.items!.push(b.build());
    return this;
  }

  addRichTextBlock(configure: (b: RichTextBlockBuilder) => void): this {
    const b = new RichTextBlockBuilder();
    configure(b);
    this.container.items!.push(b.build());
    return this;
  }

  addActionSet(configure: (b: ActionSetBuilder) => void): this {
    const b = new ActionSetBuilder();
    configure(b);
    this.container.items!.push(b.build());
    return this;
  }

  /** Add a pre-built element directly. */
  addElement(element: AdaptiveElement): this {
    this.container.items!.push(element);
    return this;
  }

  build(): Container { return this.container; }
}
