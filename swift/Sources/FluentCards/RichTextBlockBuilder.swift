/// RichTextBlockBuilder builds a RichTextBlock Adaptive Card element.
public final class RichTextBlockBuilder {
    var data: [String: Any] = ["type": "RichTextBlock", "inlines": [Any]()]

    public init() {}

    @discardableResult
    public func withID(_ id: String) -> RichTextBlockBuilder {
        data["id"] = id
        return self
    }

    @discardableResult
    public func withHorizontalAlignment(_ alignment: HorizontalAlignment) -> RichTextBlockBuilder {
        data["horizontalAlignment"] = alignment.rawValue
        return self
    }

    @discardableResult
    public func withSpacing(_ spacing: Spacing) -> RichTextBlockBuilder {
        data["spacing"] = spacing.rawValue
        return self
    }

    /// Adds a plain string inline.
    @discardableResult
    public func addText(_ text: String) -> RichTextBlockBuilder {
        var inlines = data["inlines"] as! [Any]
        inlines.append(text)
        data["inlines"] = inlines
        return self
    }

    /// Adds a TextRun inline configured by the provided closure.
    @discardableResult
    public func addTextRun(_ configure: (TextRunBuilder) -> Void) -> RichTextBlockBuilder {
        let tb = TextRunBuilder()
        configure(tb)
        var inlines = data["inlines"] as! [Any]
        inlines.append(tb.build())
        data["inlines"] = inlines
        return self
    }

    public func build() -> Card {
        return data
    }
}
