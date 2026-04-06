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

  /** Sets the unique identifier. @param id The unique identifier. @returns The builder instance for method chaining. */
  withId(id: string): this { this.container.id = id; return this; }
  /** Sets the container style. @param style The container style. @returns The builder instance for method chaining. */
  withStyle(style: ContainerStyle): this { this.container.style = style; return this; }
  /** Sets the vertical alignment of the container content. @param alignment The vertical alignment. @returns The builder instance for method chaining. */
  withVerticalContentAlignment(alignment: VerticalAlignment): this { this.container.verticalContentAlignment = alignment; return this; }
  /** Sets whether the container bleeds into the parent container. @param bleed True to enable bleed. @returns The builder instance for method chaining. */
  withBleed(bleed = true): this { this.container.bleed = bleed; return this; }
  /** Sets the minimum height of the container. @param minHeight The minimum height (e.g. `'100px'`). @returns The builder instance for method chaining. */
  withMinHeight(minHeight: string): this { this.container.minHeight = minHeight; return this; }
  /** Sets the spacing above the element. @param spacing The spacing value. @returns The builder instance for method chaining. */
  withSpacing(spacing: Spacing): this { this.container.spacing = spacing; return this; }
  /** Sets whether a separator line is displayed above the element. @param separator True to show a separator. @returns The builder instance for method chaining. */
  withSeparator(separator: boolean): this { this.container.separator = separator; return this; }
  /** Sets whether the element is visible. @param isVisible True to show the element. @returns The builder instance for method chaining. */
  withIsVisible(isVisible: boolean): this { this.container.isVisible = isVisible; return this; }

  /** Sets the background image of the container. @param configure A callback to configure the BackgroundImageBuilder. @returns The builder instance for method chaining. */
  withBackgroundImage(configure: (b: BackgroundImageBuilder) => void): this {
    const b = new BackgroundImageBuilder();
    configure(b);
    this.container.backgroundImage = b.build();
    return this;
  }

  /** Sets the action to invoke when the container is selected. @param configure A callback to configure the ActionBuilder. @returns The builder instance for method chaining. */
  withSelectAction(configure: (b: ActionBuilder) => void): this {
    const b = new ActionBuilder();
    configure(b);
    this.container.selectAction = b.build();
    return this;
  }

  /** Adds a TextBlock element. @param configure A callback to configure the TextBlockBuilder. @returns The builder instance for method chaining. */
  addTextBlock(configure: (b: TextBlockBuilder) => void): this {
    const b = new TextBlockBuilder();
    configure(b);
    this.container.items!.push(b.build());
    return this;
  }

  /** Adds an Image element. @param configure A callback to configure the ImageBuilder. @returns The builder instance for method chaining. */
  addImage(configure: (b: ImageBuilder) => void): this {
    const b = new ImageBuilder();
    configure(b);
    this.container.items!.push(b.build());
    return this;
  }

  /** Adds a nested Container element. @param configure A callback to configure the ContainerBuilder. @returns The builder instance for method chaining. */
  addContainer(configure: (b: ContainerBuilder) => void): this {
    const b = new ContainerBuilder();
    configure(b);
    this.container.items!.push(b.build());
    return this;
  }

  /** Adds a ColumnSet element. @param configure A callback to configure the ColumnSetBuilder. @returns The builder instance for method chaining. */
  addColumnSet(configure: (b: ColumnSetBuilder) => void): this {
    const b = new ColumnSetBuilder();
    configure(b);
    this.container.items!.push(b.build());
    return this;
  }

  /** Adds a FactSet element. @param configure A callback to configure the FactSetBuilder. @returns The builder instance for method chaining. */
  addFactSet(configure: (b: FactSetBuilder) => void): this {
    const b = new FactSetBuilder();
    configure(b);
    this.container.items!.push(b.build());
    return this;
  }

  /** Adds a RichTextBlock element. @param configure A callback to configure the RichTextBlockBuilder. @returns The builder instance for method chaining. */
  addRichTextBlock(configure: (b: RichTextBlockBuilder) => void): this {
    const b = new RichTextBlockBuilder();
    configure(b);
    this.container.items!.push(b.build());
    return this;
  }

  /** Adds an ActionSet element. @param configure A callback to configure the ActionSetBuilder. @returns The builder instance for method chaining. */
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

  /** Builds and returns the configured Container. @returns The configured Container instance. */
  build(): Container { return this.container; }
}
