import type { RichTextBlock, TextRun, AdaptiveElement } from '../models.js';
import { HorizontalAlignment, Spacing } from '../enums.js';
import { TextRunBuilder } from './TextRunBuilder.js';

/** Fluent builder for {@link RichTextBlock} elements. */
export class RichTextBlockBuilder {
  private readonly richText: RichTextBlock = { type: 'RichTextBlock', inlines: [] };

  /** Sets the unique identifier. @param id The unique identifier. @returns The builder instance for method chaining. */
  withId(id: string): this { this.richText.id = id; return this; }
  /** Sets the horizontal alignment of the text block. @param alignment The horizontal alignment. @returns The builder instance for method chaining. */
  withHorizontalAlignment(alignment: HorizontalAlignment): this { this.richText.horizontalAlignment = alignment; return this; }
  /** Sets the spacing above the element. @param spacing The spacing value. @returns The builder instance for method chaining. */
  withSpacing(spacing: Spacing): this { this.richText.spacing = spacing; return this; }
  /** Sets whether a separator line is displayed above the element. @param separator True to show a separator. @returns The builder instance for method chaining. */
  withSeparator(separator = true): this { this.richText.separator = separator; return this; }
  /** Sets whether the element is visible. @param isVisible True to show the element. @returns The builder instance for method chaining. */
  withIsVisible(isVisible: boolean): this { this.richText.isVisible = isVisible; return this; }
  /** Sets the height of the element. @param height The height ('auto' or 'stretch'). @returns The builder instance for method chaining. */
  withHeight(height: string): this { this.richText.height = height; return this; }
  /** Sets the fallback behavior when the element is unsupported. @param fallback The fallback value ('drop' or an element). @returns The builder instance for method chaining. */
  withFallback(fallback: 'drop' | AdaptiveElement): this { this.richText.fallback = fallback; return this; }
  /** Sets the feature requirements for the element. @param key The feature name. @param version The minimum required version. @returns The builder instance for method chaining. */
  withRequires(key: string, version: string): this { this.richText.requires = { ...this.richText.requires, [key]: version }; return this; }
  /** Sets whether content should be laid out right-to-left. @param rtl True for RTL. @returns The builder instance for method chaining. */
  withRtl(rtl = true): this { this.richText.rtl = rtl; return this; }

  /** Adds a plain text string as an inline. @param text The text to add. @returns The builder instance for method chaining. */
  addText(text: string): this {
    this.richText.inlines!.push(text);
    return this;
  }

  /** Adds a formatted TextRun inline. @param configure A callback to configure the TextRunBuilder. @returns The builder instance for method chaining. */
  addTextRun(configure: (b: TextRunBuilder) => void): this {
    const b = new TextRunBuilder();
    configure(b);
    this.richText.inlines!.push(b.build());
    return this;
  }

  /** Builds and returns the configured RichTextBlock. @returns The configured RichTextBlock instance. */
  build(): RichTextBlock { return this.richText; }
}
