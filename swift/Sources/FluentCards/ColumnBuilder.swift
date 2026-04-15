/// ColumnBuilder builds a Column element within a ColumnSet.
public final class ColumnBuilder {
    var data: [String: Any] = ["type": "Column", "items": [Any]()]

    public init() {}

    @discardableResult
    public func withID(_ id: String) -> ColumnBuilder {
        data["id"] = id
        return self
    }

    @discardableResult
    public func withWidth(_ width: String) -> ColumnBuilder {
        data["width"] = width
        return self
    }

    @discardableResult
    public func withStyle(_ style: ContainerStyle) -> ColumnBuilder {
        data["style"] = style.rawValue
        return self
    }

    @discardableResult
    public func withVerticalContentAlignment(_ alignment: VerticalAlignment) -> ColumnBuilder {
        data["verticalContentAlignment"] = alignment.rawValue
        return self
    }

    @discardableResult
    public func withBleed(_ bleed: Bool) -> ColumnBuilder {
        data["bleed"] = bleed
        return self
    }

    @discardableResult
    public func withMinHeight(_ minHeight: String) -> ColumnBuilder {
        data["minHeight"] = minHeight
        return self
    }

    @discardableResult
    public func withBackgroundImage(_ configure: (BackgroundImageBuilder) -> Void) -> ColumnBuilder {
        let bib = BackgroundImageBuilder()
        configure(bib)
        data["backgroundImage"] = bib.build()
        return self
    }

    @discardableResult
    public func withSelectAction(_ configure: (ActionBuilder) -> Void) -> ColumnBuilder {
        let ab = ActionBuilder()
        configure(ab)
        data["selectAction"] = ab.build()
        return self
    }

    @discardableResult
    public func addTextBlock(_ configure: (TextBlockBuilder) -> Void) -> ColumnBuilder {
        let tb = TextBlockBuilder()
        configure(tb)
        appendItem(tb.build())
        return self
    }

    @discardableResult
    public func addImage(_ configure: (ImageBuilder) -> Void) -> ColumnBuilder {
        let ib = ImageBuilder()
        configure(ib)
        appendItem(ib.build())
        return self
    }

    @discardableResult
    public func addContainer(_ configure: (ContainerBuilder) -> Void) -> ColumnBuilder {
        let cb = ContainerBuilder()
        configure(cb)
        appendItem(cb.build())
        return self
    }

    @discardableResult
    public func addElement(_ element: Card) -> ColumnBuilder {
        appendItem(element)
        return self
    }

    public func build() -> Card {
        return data
    }

    private func appendItem(_ item: Any) {
        var items = data["items"] as! [Any]
        items.append(item)
        data["items"] = items
    }
}
