import type { Table, TableColumnDefinition, TableRow } from '../models.js';
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
