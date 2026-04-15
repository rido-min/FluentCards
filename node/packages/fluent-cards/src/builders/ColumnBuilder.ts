import type { Column, AdaptiveElement } from '../models.js';
import { ContainerStyle, Spacing, VerticalAlignment } from '../enums.js';
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
  /** Sets the column width (e.g. `'auto'`, `'stretch'`, `'50px'`, a weight like `'1'`, or a number for relative weight). @param width The column width. @returns The builder instance for method chaining. */
  withWidth(width: string | number): this { this.column.width = width; return this; }
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

  /** Sets whether the column is visible. @param isVisible True if visible. @returns The builder instance for method chaining. */
  withIsVisible(isVisible: boolean): this { this.column.isVisible = isVisible; return this; }
  /** Sets the spacing between this column and the previous one. @param spacing The spacing value. @returns The builder instance for method chaining. */
  withSpacing(spacing: Spacing): this { this.column.spacing = spacing; return this; }
  /** Sets whether a separator line is drawn at the left of the column. @param separator True to show separator. @returns The builder instance for method chaining. */
  withSeparator(separator = true): this { this.column.separator = separator; return this; }
  /** Sets the height of the column. @param height The height ('auto' or 'stretch'). @returns The builder instance for method chaining. */
  withHeight(height: string): this { this.column.height = height; return this; }
  /** Sets the fallback behavior when the column is unsupported. @param fallback The fallback value ('drop' or an element). @returns The builder instance for method chaining. */
  withFallback(fallback: 'drop' | AdaptiveElement): this { this.column.fallback = fallback; return this; }
  /** Sets the feature requirements for the column. @param key The feature name. @param version The minimum required version. @returns The builder instance for method chaining. */
  withRequires(key: string, version: string): this { this.column.requires = { ...this.column.requires, [key]: version }; return this; }
  /** Sets whether content should be laid out right-to-left. @param rtl True for RTL. @returns The builder instance for method chaining. */
  withRtl(rtl = true): this { this.column.rtl = rtl; return this; }

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
