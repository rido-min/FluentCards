import type { TextRun } from '../models.js';
import { TextColor, TextSize, TextWeight } from '../enums.js';
import { ActionBuilder } from './ActionBuilder.js';

/** Fluent builder for {@link TextRun} inline elements. */
export class TextRunBuilder {
  private readonly run: TextRun = { type: 'TextRun' };

  /** Sets the text content. @param text The text content. @returns The builder instance for method chaining. */
  withText(text: string): this { this.run.text = text; return this; }
  /** Sets the font size. @param size The text size. @returns The builder instance for method chaining. */
  withSize(size: TextSize): this { this.run.size = size; return this; }
  /** Sets the font weight. @param weight The text weight. @returns The builder instance for method chaining. */
  withWeight(weight: TextWeight): this { this.run.weight = weight; return this; }
  /** Sets the text color. @param color The text color. @returns The builder instance for method chaining. */
  withColor(color: TextColor): this { this.run.color = color; return this; }
  /** Sets whether the text is displayed with subtle styling. @param subtle True for subtle styling. @returns The builder instance for method chaining. */
  isSubtle(subtle = true): this { this.run.isSubtle = subtle; return this; }
  /** Sets whether the text is italicized. @param italic True to italicize. @returns The builder instance for method chaining. */
  isItalic(italic = true): this { this.run.italic = italic; return this; }
  /** Sets whether the text is struck through. @param strikethrough True to apply strikethrough. @returns The builder instance for method chaining. */
  isStrikethrough(strikethrough = true): this { this.run.strikethrough = strikethrough; return this; }
  /** Sets whether the text is underlined. @param underline True to underline. @returns The builder instance for method chaining. */
  isUnderline(underline = true): this { this.run.underline = underline; return this; }
  /** Sets whether the text is highlighted. @param highlight True to highlight. @returns The builder instance for method chaining. */
  isHighlight(highlight = true): this { this.run.highlight = highlight; return this; }

  /** Sets the action to invoke when the text run is selected. @param configure A callback to configure the ActionBuilder. @returns The builder instance for method chaining. */
  withSelectAction(configure: (b: ActionBuilder) => void): this {
    const b = new ActionBuilder();
    configure(b);
    this.run.selectAction = b.build();
    return this;
  }

  /** Builds and returns the configured TextRun. @returns The configured TextRun instance. */
  build(): TextRun { return this.run; }
}
