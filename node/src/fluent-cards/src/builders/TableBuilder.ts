import type { Table, TableColumnDefinition, TableRow } from '../models.js';
import { ContainerStyle, HorizontalAlignment, VerticalAlignment, Spacing } from '../enums.js';

/** Fluent builder for {@link Table} elements. */
export class TableBuilder {
  private readonly table: Table = { type: 'Table', columns: [], rows: [] };

  withId(id: string): this { this.table.id = id; return this; }
  withFirstRowAsHeader(firstRowAsHeader = true): this { this.table.firstRowAsHeader = firstRowAsHeader; return this; }
  withShowGridLines(showGridLines = true): this { this.table.showGridLines = showGridLines; return this; }
  withGridStyle(gridStyle: ContainerStyle): this { this.table.gridStyle = gridStyle; return this; }
  withHorizontalCellContentAlignment(alignment: HorizontalAlignment): this { this.table.horizontalCellContentAlignment = alignment; return this; }
  withVerticalCellContentAlignment(alignment: VerticalAlignment): this { this.table.verticalCellContentAlignment = alignment; return this; }
  withSpacing(spacing: Spacing): this { this.table.spacing = spacing; return this; }

  addColumn(column: TableColumnDefinition): this {
    this.table.columns!.push(column);
    return this;
  }

  addRow(row: TableRow): this {
    this.table.rows!.push(row);
    return this;
  }

  build(): Table { return this.table; }
}
