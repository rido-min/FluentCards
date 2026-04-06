import type { TextRun } from '../models.js';
import { TextColor, TextSize, TextWeight } from '../enums.js';
import { ActionBuilder } from './ActionBuilder.js';

/** Fluent builder for {@link TextRun} inline elements. */
export class TextRunBuilder {
  private readonly run: TextRun = { type: 'TextRun' };

  withText(text: string): this { this.run.text = text; return this; }
  withSize(size: TextSize): this { this.run.size = size; return this; }
  withWeight(weight: TextWeight): this { this.run.weight = weight; return this; }
  withColor(color: TextColor): this { this.run.color = color; return this; }
  isSubtle(subtle = true): this { this.run.isSubtle = subtle; return this; }
  isItalic(italic = true): this { this.run.italic = italic; return this; }
  isStrikethrough(strikethrough = true): this { this.run.strikethrough = strikethrough; return this; }
  isUnderline(underline = true): this { this.run.underline = underline; return this; }
  isHighlight(highlight = true): this { this.run.highlight = highlight; return this; }

  withSelectAction(configure: (b: ActionBuilder) => void): this {
    const b = new ActionBuilder();
    configure(b);
    this.run.selectAction = b.build();
    return this;
  }

  build(): TextRun { return this.run; }
}
