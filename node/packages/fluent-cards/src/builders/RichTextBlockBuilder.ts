import type { RichTextBlock, TextRun } from '../models.js';
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
