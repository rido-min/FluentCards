import type { TextBlock, AdaptiveAction } from '../models.js';
import { FontType, HorizontalAlignment, Spacing, TextBlockStyle, TextColor, TextSize, TextWeight } from '../enums.js';

/** Fluent builder for {@link TextBlock} elements. */
export class TextBlockBuilder {
  private readonly block: TextBlock = { type: 'TextBlock', text: '' };

  /** Sets the unique identifier. @param id The unique identifier. @returns The builder instance for method chaining. */
  withId(id: string): this { this.block.id = id; return this; }
  /** Sets the text to display. @param text The text content. @returns The builder instance for method chaining. */
  withText(text: string): this { this.block.text = text; return this; }
  /** Sets the size of the text. @param size The text size. @returns The builder instance for method chaining. */
  withSize(size: TextSize): this { this.block.size = size; return this; }
  /** Sets the weight of the text. @param weight The text weight. @returns The builder instance for method chaining. */
  withWeight(weight: TextWeight): this { this.block.weight = weight; return this; }
  /** Sets the color of the text. @param color The text color. @returns The builder instance for method chaining. */
  withColor(color: TextColor): this { this.block.color = color; return this; }
  /** Sets whether the text displays with subtle styling. @param isSubtle True for subtle styling. @returns The builder instance for method chaining. */
  withIsSubtle(isSubtle = true): this { this.block.isSubtle = isSubtle; return this; }
  /** Sets whether the text should wrap. @param wrap True to allow text wrapping, false to clip. @returns The builder instance for method chaining. */
  withWrap(wrap: boolean): this { this.block.wrap = wrap; return this; }
  /** Sets the maximum number of lines to display. @param maxLines The maximum number of lines. @returns The builder instance for method chaining. */
  withMaxLines(maxLines: number): this { this.block.maxLines = maxLines; return this; }
  /** Sets the horizontal alignment of the text. @param alignment The horizontal alignment. @returns The builder instance for method chaining. */
  withHorizontalAlignment(alignment: HorizontalAlignment): this { this.block.horizontalAlignment = alignment; return this; }
  /** Sets the font type. @param fontType The font type. @returns The builder instance for method chaining. */
  withFontType(fontType: FontType): this { this.block.fontType = fontType; return this; }
  /** Sets the style of the text block. @param style The text block style. @returns The builder instance for method chaining. */
  withStyle(style: TextBlockStyle): this { this.block.style = style; return this; }
  /** Sets the action to invoke when the element is selected. @param action The select action. @returns The builder instance for method chaining. */
  withSelectAction(action: AdaptiveAction): this { this.block.selectAction = action; return this; }
  /** Sets the spacing above the element. @param spacing The spacing value. @returns The builder instance for method chaining. */
  withSpacing(spacing: Spacing): this { this.block.spacing = spacing; return this; }
  /** Sets whether a separator line is displayed above the element. @param separator True to show a separator. @returns The builder instance for method chaining. */
  withSeparator(separator: boolean): this { this.block.separator = separator; return this; }
  /** Sets whether the element is visible. @param isVisible True to show the element. @returns The builder instance for method chaining. */
  withIsVisible(isVisible: boolean): this { this.block.isVisible = isVisible; return this; }

  /** Builds and returns the configured TextBlock. @returns The configured TextBlock instance. */
  build(): TextBlock { return this.block; }
}
