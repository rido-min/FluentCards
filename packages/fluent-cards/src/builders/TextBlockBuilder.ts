import type { TextBlock } from '../models.js';
import { HorizontalAlignment, TextColor, TextSize, TextWeight } from '../enums.js';

/** Fluent builder for {@link TextBlock} elements. */
export class TextBlockBuilder {
  private readonly block: TextBlock = { type: 'TextBlock', text: '' };

  withId(id: string): this { this.block.id = id; return this; }
  withText(text: string): this { this.block.text = text; return this; }
  withSize(size: TextSize): this { this.block.size = size; return this; }
  withWeight(weight: TextWeight): this { this.block.weight = weight; return this; }
  withColor(color: TextColor): this { this.block.color = color; return this; }
  withWrap(wrap: boolean): this { this.block.wrap = wrap; return this; }
  withMaxLines(maxLines: number): this { this.block.maxLines = maxLines; return this; }
  withHorizontalAlignment(alignment: HorizontalAlignment): this { this.block.horizontalAlignment = alignment; return this; }
  withSpacing(spacing: string): this { this.block.spacing = spacing; return this; }
  withSeparator(separator: boolean): this { this.block.separator = separator; return this; }
  withIsVisible(isVisible: boolean): this { this.block.isVisible = isVisible; return this; }

  build(): TextBlock { return this.block; }
}
