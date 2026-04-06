import type { ColumnSet } from '../models.js';
import { ContainerStyle, HorizontalAlignment, Spacing } from '../enums.js';
import { ColumnBuilder } from './ColumnBuilder.js';
import { ActionBuilder } from './ActionBuilder.js';

/** Fluent builder for {@link ColumnSet} elements. */
export class ColumnSetBuilder {
  private readonly columnSet: ColumnSet = { type: 'ColumnSet', columns: [] };

  /** Sets the unique identifier. @param id The unique identifier. @returns The builder instance for method chaining. */
  withId(id: string): this { this.columnSet.id = id; return this; }
  /** Sets the container style. @param style The container style. @returns The builder instance for method chaining. */
  withStyle(style: ContainerStyle): this { this.columnSet.style = style; return this; }
  /** Sets whether the column set bleeds into the parent container. @param bleed True to enable bleed. @returns The builder instance for method chaining. */
  withBleed(bleed = true): this { this.columnSet.bleed = bleed; return this; }
  /** Sets the minimum height of the column set. @param minHeight The minimum height (e.g. `'100px'`). @returns The builder instance for method chaining. */
  withMinHeight(minHeight: string): this { this.columnSet.minHeight = minHeight; return this; }
  /** Sets the horizontal alignment of the columns. @param alignment The horizontal alignment. @returns The builder instance for method chaining. */
  withHorizontalAlignment(alignment: HorizontalAlignment): this { this.columnSet.horizontalAlignment = alignment; return this; }
  /** Sets the spacing above the element. @param spacing The spacing value. @returns The builder instance for method chaining. */
  withSpacing(spacing: Spacing): this { this.columnSet.spacing = spacing; return this; }
  /** Sets whether a separator line is displayed above the element. @param separator True to show a separator. @returns The builder instance for method chaining. */
  withSeparator(separator: boolean): this { this.columnSet.separator = separator; return this; }

  /** Sets the action to invoke when the column set is selected. @param configure A callback to configure the ActionBuilder. @returns The builder instance for method chaining. */
  withSelectAction(configure: (b: ActionBuilder) => void): this {
    const b = new ActionBuilder();
    configure(b);
    this.columnSet.selectAction = b.build();
    return this;
  }

  /** Adds a column using a configure callback. @param configure A callback to configure the Column builder. @returns The builder instance for method chaining. */
  addColumn(configure: (b: ColumnBuilder) => void): this;
  /** Adds a column with an explicit width. @param width The column width (e.g. `"auto"`, `"stretch"`, or a pixel value). @param configure A callback to configure the Column builder. @returns The builder instance for method chaining. */
  addColumn(width: string, configure: (b: ColumnBuilder) => void): this;
  addColumn(
    configureOrWidth: string | ((b: ColumnBuilder) => void),
    configure?: (b: ColumnBuilder) => void,
  ): this {
    let b = new ColumnBuilder();
    if (typeof configureOrWidth === 'string') {
      b.withWidth(configureOrWidth);
      configure!(b);
    } else {
      configureOrWidth(b);
    }
    this.columnSet.columns!.push(b.build());
    return this;
  }

  /** Builds and returns the configured ColumnSet. @returns The configured ColumnSet instance. */
  build(): ColumnSet { return this.columnSet; }
}
