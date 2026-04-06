import type { TextBlock, AdaptiveAction } from '../models.js';
import { FontType, HorizontalAlignment, Spacing, TextBlockStyle, TextColor, TextSize, TextWeight } from '../enums.js';

/** Fluent builder for {@link TextBlock} elements. */
export class TextBlockBuilder {
  private readonly block: TextBlock = { type: 'TextBlock', text: '' };

  withId(id: string): this { this.block.id = id; return this; }
  withText(text: string): this { this.block.text = text; return this; }
  withSize(size: TextSize): this { this.block.size = size; return this; }
  withWeight(weight: TextWeight): this { this.block.weight = weight; return this; }
  withColor(color: TextColor): this { this.block.color = color; return this; }
  withIsSubtle(isSubtle = true): this { this.block.isSubtle = isSubtle; return this; }
  withWrap(wrap: boolean): this { this.block.wrap = wrap; return this; }
  withMaxLines(maxLines: number): this { this.block.maxLines = maxLines; return this; }
  withHorizontalAlignment(alignment: HorizontalAlignment): this { this.block.horizontalAlignment = alignment; return this; }
  withFontType(fontType: FontType): this { this.block.fontType = fontType; return this; }
  withStyle(style: TextBlockStyle): this { this.block.style = style; return this; }
  withSelectAction(action: AdaptiveAction): this { this.block.selectAction = action; return this; }
  withSpacing(spacing: Spacing): this { this.block.spacing = spacing; return this; }
  withSeparator(separator: boolean): this { this.block.separator = separator; return this; }
  withIsVisible(isVisible: boolean): this { this.block.isVisible = isVisible; return this; }

  build(): TextBlock { return this.block; }
}
