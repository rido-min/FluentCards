/// TableBuilder builds a Table Adaptive Card element (requires Adaptive Cards 1.5+).
public final class TableBuilder {
    var data: [String: Any] = ["type": "Table", "columns": [Any](), "rows": [Any]()]

    public init() {}

    @discardableResult
    public func withID(_ id: String) -> TableBuilder {
        data["id"] = id
        return self
    }

    @discardableResult
    public func withFirstRowAsHeader(_ firstRowAsHeader: Bool) -> TableBuilder {
        data["firstRowAsHeader"] = firstRowAsHeader
        return self
    }

    @discardableResult
    public func withShowGridLines(_ showGridLines: Bool) -> TableBuilder {
        data["showGridLines"] = showGridLines
        return self
    }

    @discardableResult
    public func withGridStyle(_ gridStyle: ContainerStyle) -> TableBuilder {
        data["gridStyle"] = gridStyle.rawValue
        return self
    }

    @discardableResult
    public func withHorizontalCellContentAlignment(_ alignment: HorizontalAlignment) -> TableBuilder {
        data["horizontalCellContentAlignment"] = alignment.rawValue
        return self
    }

    @discardableResult
    public func withVerticalCellContentAlignment(_ alignment: VerticalAlignment) -> TableBuilder {
        data["verticalCellContentAlignment"] = alignment.rawValue
        return self
    }

    @discardableResult
    public func withSpacing(_ spacing: Spacing) -> TableBuilder {
        data["spacing"] = spacing.rawValue
        return self
    }

    /// Adds a table column definition map (e.g. ["width": 1]).
    @discardableResult
    public func addColumn(_ column: [String: Any]) -> TableBuilder {
        var cols = data["columns"] as! [Any]
        cols.append(column)
        data["columns"] = cols
        return self
    }

    /// Adds a table row map (e.g. ["cells": [...]]).
    @discardableResult
    public func addRow(_ row: [String: Any]) -> TableBuilder {
        var rows = data["rows"] as! [Any]
        rows.append(row)
        data["rows"] = rows
        return self
    }

    public func build() -> Card {
        return data
    }
}
