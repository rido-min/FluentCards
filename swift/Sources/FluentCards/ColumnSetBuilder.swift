/// ColumnSetBuilder builds a ColumnSet Adaptive Card element.
public final class ColumnSetBuilder {
    var data: [String: Any] = ["type": "ColumnSet", "columns": [Any]()]

    public init() {}

    @discardableResult
    public func withID(_ id: String) -> ColumnSetBuilder {
        data["id"] = id
        return self
    }

    @discardableResult
    public func withStyle(_ style: ContainerStyle) -> ColumnSetBuilder {
        data["style"] = style.rawValue
        return self
    }

    @discardableResult
    public func withBleed(_ bleed: Bool) -> ColumnSetBuilder {
        data["bleed"] = bleed
        return self
    }

    @discardableResult
    public func withMinHeight(_ minHeight: String) -> ColumnSetBuilder {
        data["minHeight"] = minHeight
        return self
    }

    @discardableResult
    public func withHorizontalAlignment(_ alignment: HorizontalAlignment) -> ColumnSetBuilder {
        data["horizontalAlignment"] = alignment.rawValue
        return self
    }

    @discardableResult
    public func withSpacing(_ spacing: Spacing) -> ColumnSetBuilder {
        data["spacing"] = spacing.rawValue
        return self
    }

    @discardableResult
    public func withSeparator(_ separator: Bool) -> ColumnSetBuilder {
        data["separator"] = separator
        return self
    }

    @discardableResult
    public func withSelectAction(_ configure: (ActionBuilder) -> Void) -> ColumnSetBuilder {
        let ab = ActionBuilder()
        configure(ab)
        data["selectAction"] = ab.build()
        return self
    }

    /// Adds a column configured by the provided closure.
    @discardableResult
    public func addColumn(_ configure: (ColumnBuilder) -> Void) -> ColumnSetBuilder {
        let cb = ColumnBuilder()
        configure(cb)
        var cols = data["columns"] as! [Any]
        cols.append(cb.build())
        data["columns"] = cols
        return self
    }

    /// Adds a column with an explicit width string plus additional configuration.
    @discardableResult
    public func addColumnWithWidth(_ width: String, _ configure: (ColumnBuilder) -> Void) -> ColumnSetBuilder {
        let cb = ColumnBuilder()
        cb.withWidth(width)
        configure(cb)
        var cols = data["columns"] as! [Any]
        cols.append(cb.build())
        data["columns"] = cols
        return self
    }

    public func build() -> Card {
        return data
    }
}
