import type { ColumnSet } from '../models.js';
import { ContainerStyle, HorizontalAlignment, Spacing } from '../enums.js';
import { ColumnBuilder } from './ColumnBuilder.js';
import { ActionBuilder } from './ActionBuilder.js';

/** Fluent builder for {@link ColumnSet} elements. */
export class ColumnSetBuilder {
  private readonly columnSet: ColumnSet = { type: 'ColumnSet', columns: [] };

  withId(id: string): this { this.columnSet.id = id; return this; }
  withStyle(style: ContainerStyle): this { this.columnSet.style = style; return this; }
  withBleed(bleed = true): this { this.columnSet.bleed = bleed; return this; }
  withMinHeight(minHeight: string): this { this.columnSet.minHeight = minHeight; return this; }
  withHorizontalAlignment(alignment: HorizontalAlignment): this { this.columnSet.horizontalAlignment = alignment; return this; }
  withSpacing(spacing: Spacing): this { this.columnSet.spacing = spacing; return this; }
  withSeparator(separator: boolean): this { this.columnSet.separator = separator; return this; }

  withSelectAction(configure: (b: ActionBuilder) => void): this {
    const b = new ActionBuilder();
    configure(b);
    this.columnSet.selectAction = b.build();
    return this;
  }

  addColumn(configure: (b: ColumnBuilder) => void): this;
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

  build(): ColumnSet { return this.columnSet; }
}
