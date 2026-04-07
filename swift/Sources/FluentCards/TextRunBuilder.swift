/// TextRunBuilder builds a TextRun inline element for use within RichTextBlock.
public final class TextRunBuilder {
    var data: [String: Any] = ["type": "TextRun"]

    public init() {}

    @discardableResult
    public func withText(_ text: String) -> TextRunBuilder {
        data["text"] = text
        return self
    }

    @discardableResult
    public func withSize(_ size: TextSize) -> TextRunBuilder {
        data["size"] = size.rawValue
        return self
    }

    @discardableResult
    public func withWeight(_ weight: TextWeight) -> TextRunBuilder {
        data["weight"] = weight.rawValue
        return self
    }

    @discardableResult
    public func withColor(_ color: TextColor) -> TextRunBuilder {
        data["color"] = color.rawValue
        return self
    }

    @discardableResult
    public func withIsSubtle(_ subtle: Bool) -> TextRunBuilder {
        data["isSubtle"] = subtle
        return self
    }

    @discardableResult
    public func withItalic(_ italic: Bool) -> TextRunBuilder {
        data["italic"] = italic
        return self
    }

    @discardableResult
    public func withStrikethrough(_ strikethrough: Bool) -> TextRunBuilder {
        data["strikethrough"] = strikethrough
        return self
    }

    @discardableResult
    public func withUnderline(_ underline: Bool) -> TextRunBuilder {
        data["underline"] = underline
        return self
    }

    @discardableResult
    public func withHighlight(_ highlight: Bool) -> TextRunBuilder {
        data["highlight"] = highlight
        return self
    }

    @discardableResult
    public func withSelectAction(_ configure: (ActionBuilder) -> Void) -> TextRunBuilder {
        let ab = ActionBuilder()
        configure(ab)
        data["selectAction"] = ab.build()
        return self
    }

    public func build() -> Card {
        return data
    }
}
