import type { Column, AdaptiveElement } from '../models.js';
import { ContainerStyle, VerticalAlignment } from '../enums.js';
import { TextBlockBuilder } from './TextBlockBuilder.js';
import { ImageBuilder } from './ImageBuilder.js';
import { ContainerBuilder } from './ContainerBuilder.js';
import { ActionBuilder } from './ActionBuilder.js';
import { BackgroundImageBuilder } from './BackgroundImageBuilder.js';

/** Fluent builder for {@link Column} elements. */
export class ColumnBuilder {
  private readonly column: Column = { type: 'Column', items: [] };

  /** Sets the unique identifier. @param id The unique identifier. @returns The builder instance for method chaining. */
  withId(id: string): this { this.column.id = id; return this; }
  /** Sets the column width (e.g. `'auto'`, `'stretch'`, `'50px'`, or a weight like `'1'`). @param width The column width. @returns The builder instance for method chaining. */
  withWidth(width: string): this { this.column.width = width; return this; }
  /** Sets the container style. @param style The container style. @returns The builder instance for method chaining. */
  withStyle(style: ContainerStyle): this { this.column.style = style; return this; }
  /** Sets the vertical alignment of the column content. @param alignment The vertical alignment. @returns The builder instance for method chaining. */
  withVerticalContentAlignment(alignment: VerticalAlignment): this { this.column.verticalContentAlignment = alignment; return this; }
  /** Sets whether the column bleeds into the parent container. @param bleed True to enable bleed. @returns The builder instance for method chaining. */
  withBleed(bleed = true): this { this.column.bleed = bleed; return this; }
  /** Sets the minimum height of the column. @param minHeight The minimum height (e.g. `'100px'`). @returns The builder instance for method chaining. */
  withMinHeight(minHeight: string): this { this.column.minHeight = minHeight; return this; }

  /** Sets the background image of the column. @param configure A callback to configure the BackgroundImageBuilder. @returns The builder instance for method chaining. */
  withBackgroundImage(configure: (b: BackgroundImageBuilder) => void): this {
    const b = new BackgroundImageBuilder();
    configure(b);
    this.column.backgroundImage = b.build();
    return this;
  }

  /** Sets the action to invoke when the column is selected. @param configure A callback to configure the ActionBuilder. @returns The builder instance for method chaining. */
  withSelectAction(configure: (b: ActionBuilder) => void): this {
    const b = new ActionBuilder();
    configure(b);
    this.column.selectAction = b.build();
    return this;
  }

  /** Adds a TextBlock element. @param configure A callback to configure the TextBlockBuilder. @returns The builder instance for method chaining. */
  addTextBlock(configure: (b: TextBlockBuilder) => void): this {
    const b = new TextBlockBuilder();
    configure(b);
    this.column.items!.push(b.build());
    return this;
  }

  /** Adds an Image element. @param configure A callback to configure the ImageBuilder. @returns The builder instance for method chaining. */
  addImage(configure: (b: ImageBuilder) => void): this {
    const b = new ImageBuilder();
    configure(b);
    this.column.items!.push(b.build());
    return this;
  }

  /** Adds a nested Container element. @param configure A callback to configure the ContainerBuilder. @returns The builder instance for method chaining. */
  addContainer(configure: (b: ContainerBuilder) => void): this {
    const b = new ContainerBuilder();
    configure(b);
    this.column.items!.push(b.build());
    return this;
  }

  /** Adds a pre-built element directly. @param element The element to add. @returns The builder instance for method chaining. */
  addElement(element: AdaptiveElement): this {
    this.column.items!.push(element);
    return this;
  }

  /** Builds and returns the configured Column. @returns The configured Column instance. */
  build(): Column { return this.column; }
}
