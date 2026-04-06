import type { RichTextBlock, TextRun } from '../models.js';
import { HorizontalAlignment, Spacing } from '../enums.js';
import { TextRunBuilder } from './TextRunBuilder.js';

/** Fluent builder for {@link RichTextBlock} elements. */
export class RichTextBlockBuilder {
  private readonly richText: RichTextBlock = { type: 'RichTextBlock', inlines: [] };

  withId(id: string): this { this.richText.id = id; return this; }
  withHorizontalAlignment(alignment: HorizontalAlignment): this { this.richText.horizontalAlignment = alignment; return this; }
  withSpacing(spacing: Spacing): this { this.richText.spacing = spacing; return this; }

  addText(text: string): this {
    this.richText.inlines!.push(text);
    return this;
  }

  addTextRun(configure: (b: TextRunBuilder) => void): this {
    const b = new TextRunBuilder();
    configure(b);
    this.richText.inlines!.push(b.build());
    return this;
  }

  build(): RichTextBlock { return this.richText; }
}
