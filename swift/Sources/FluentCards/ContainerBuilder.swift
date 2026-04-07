/// ContainerBuilder builds a Container Adaptive Card element.
public final class ContainerBuilder {
    var data: [String: Any] = ["type": "Container", "items": [Any]()]

    public init() {}

    @discardableResult
    public func withID(_ id: String) -> ContainerBuilder {
        data["id"] = id
        return self
    }

    @discardableResult
    public func withStyle(_ style: ContainerStyle) -> ContainerBuilder {
        data["style"] = style.rawValue
        return self
    }

    @discardableResult
    public func withVerticalContentAlignment(_ alignment: VerticalAlignment) -> ContainerBuilder {
        data["verticalContentAlignment"] = alignment.rawValue
        return self
    }

    @discardableResult
    public func withBleed(_ bleed: Bool) -> ContainerBuilder {
        data["bleed"] = bleed
        return self
    }

    @discardableResult
    public func withMinHeight(_ minHeight: String) -> ContainerBuilder {
        data["minHeight"] = minHeight
        return self
    }

    @discardableResult
    public func withSpacing(_ spacing: Spacing) -> ContainerBuilder {
        data["spacing"] = spacing.rawValue
        return self
    }

    @discardableResult
    public func withSeparator(_ separator: Bool) -> ContainerBuilder {
        data["separator"] = separator
        return self
    }

    @discardableResult
    public func withIsVisible(_ isVisible: Bool) -> ContainerBuilder {
        data["isVisible"] = isVisible
        return self
    }

    @discardableResult
    public func withBackgroundImage(_ configure: (BackgroundImageBuilder) -> Void) -> ContainerBuilder {
        let bib = BackgroundImageBuilder()
        configure(bib)
        data["backgroundImage"] = bib.build()
        return self
    }

    @discardableResult
    public func withSelectAction(_ configure: (ActionBuilder) -> Void) -> ContainerBuilder {
        let ab = ActionBuilder()
        configure(ab)
        data["selectAction"] = ab.build()
        return self
    }

    @discardableResult
    public func addTextBlock(_ configure: (TextBlockBuilder) -> Void) -> ContainerBuilder {
        let tb = TextBlockBuilder()
        configure(tb)
        appendItem(tb.build())
        return self
    }

    @discardableResult
    public func addImage(_ configure: (ImageBuilder) -> Void) -> ContainerBuilder {
        let ib = ImageBuilder()
        configure(ib)
        appendItem(ib.build())
        return self
    }

    @discardableResult
    public func addContainer(_ configure: (ContainerBuilder) -> Void) -> ContainerBuilder {
        let cb = ContainerBuilder()
        configure(cb)
        appendItem(cb.build())
        return self
    }

    @discardableResult
    public func addColumnSet(_ configure: (ColumnSetBuilder) -> Void) -> ContainerBuilder {
        let cs = ColumnSetBuilder()
        configure(cs)
        appendItem(cs.build())
        return self
    }

    @discardableResult
    public func addFactSet(_ configure: (FactSetBuilder) -> Void) -> ContainerBuilder {
        let fs = FactSetBuilder()
        configure(fs)
        appendItem(fs.build())
        return self
    }

    @discardableResult
    public func addRichTextBlock(_ configure: (RichTextBlockBuilder) -> Void) -> ContainerBuilder {
        let rtb = RichTextBlockBuilder()
        configure(rtb)
        appendItem(rtb.build())
        return self
    }

    @discardableResult
    public func addActionSet(_ configure: (ActionSetBuilder) -> Void) -> ContainerBuilder {
        let asb = ActionSetBuilder()
        configure(asb)
        appendItem(asb.build())
        return self
    }

    @discardableResult
    public func addElement(_ element: Card) -> ContainerBuilder {
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
