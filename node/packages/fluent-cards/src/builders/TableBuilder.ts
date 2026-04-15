import type { Table, TableColumnDefinition, TableRow, AdaptiveElement } from '../models.js';
import { ContainerStyle, HorizontalAlignment, VerticalAlignment, Spacing } from '../enums.js';

/** Fluent builder for {@link Table} elements. */
export class TableBuilder {
  private readonly table: Table = { type: 'Table', columns: [], rows: [] };

  /** Sets the unique identifier. @param id The unique identifier. @returns The builder instance for method chaining. */
  withId(id: string): this { this.table.id = id; return this; }
  /** Sets whether the first row is treated as a header row. @param firstRowAsHeader True to treat the first row as a header. @returns The builder instance for method chaining. */
  withFirstRowAsHeader(firstRowAsHeader = true): this { this.table.firstRowAsHeader = firstRowAsHeader; return this; }
  /** Sets whether grid lines are displayed. @param showGridLines True to show grid lines. @returns The builder instance for method chaining. */
  withShowGridLines(showGridLines = true): this { this.table.showGridLines = showGridLines; return this; }
  /** Sets the style of the grid lines. @param gridStyle The container style for grid lines. @returns The builder instance for method chaining. */
  withGridStyle(gridStyle: ContainerStyle): this { this.table.gridStyle = gridStyle; return this; }
  /** Sets the default horizontal alignment of cell content. @param alignment The horizontal alignment. @returns The builder instance for method chaining. */
  withHorizontalCellContentAlignment(alignment: HorizontalAlignment): this { this.table.horizontalCellContentAlignment = alignment; return this; }
  /** Sets the default vertical alignment of cell content. @param alignment The vertical alignment. @returns The builder instance for method chaining. */
  withVerticalCellContentAlignment(alignment: VerticalAlignment): this { this.table.verticalCellContentAlignment = alignment; return this; }
  /** Sets the spacing above the element. @param spacing The spacing value. @returns The builder instance for method chaining. */
  withSpacing(spacing: Spacing): this { this.table.spacing = spacing; return this; }
  /** Sets whether a separator line is displayed above the element. @param separator True to show a separator. @returns The builder instance for method chaining. */
  withSeparator(separator = true): this { this.table.separator = separator; return this; }
  /** Sets whether the element is visible. @param isVisible True to show the element. @returns The builder instance for method chaining. */
  withIsVisible(isVisible: boolean): this { this.table.isVisible = isVisible; return this; }
  /** Sets the height of the element. @param height The height ('auto' or 'stretch'). @returns The builder instance for method chaining. */
  withHeight(height: string): this { this.table.height = height; return this; }
  /** Sets the fallback behavior when the element is unsupported. @param fallback The fallback value ('drop' or an element). @returns The builder instance for method chaining. */
  withFallback(fallback: 'drop' | AdaptiveElement): this { this.table.fallback = fallback; return this; }
  /** Sets the feature requirements for the element. @param key The feature name. @param version The minimum required version. @returns The builder instance for method chaining. */
  withRequires(key: string, version: string): this { this.table.requires = { ...this.table.requires, [key]: version }; return this; }
  /** Sets whether content should be laid out right-to-left. @param rtl True for RTL. @returns The builder instance for method chaining. */
  withRtl(rtl = true): this { this.table.rtl = rtl; return this; }

  /** Adds a column definition. @param column The column definition. @returns The builder instance for method chaining. */
  addColumn(column: TableColumnDefinition): this {
    this.table.columns!.push(column);
    return this;
  }

  /** Adds a row to the table. @param row The table row. @returns The builder instance for method chaining. */
  addRow(row: TableRow): this {
    this.table.rows!.push(row);
    return this;
  }

  /** Builds and returns the configured Table. @returns The configured Table instance. */
  build(): Table { return this.table; }
}
