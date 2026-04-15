/// TextBlockBuilder builds a TextBlock Adaptive Card element.
public final class TextBlockBuilder {
    var data: [String: Any] = ["type": "TextBlock", "text": ""]

    public init() {}

    @discardableResult
    public func withID(_ id: String) -> TextBlockBuilder {
        data["id"] = id
        return self
    }

    @discardableResult
    public func withText(_ text: String) -> TextBlockBuilder {
        data["text"] = text
        return self
    }

    @discardableResult
    public func withSize(_ size: TextSize) -> TextBlockBuilder {
        data["size"] = size.rawValue
        return self
    }

    @discardableResult
    public func withWeight(_ weight: TextWeight) -> TextBlockBuilder {
        data["weight"] = weight.rawValue
        return self
    }

    @discardableResult
    public func withColor(_ color: TextColor) -> TextBlockBuilder {
        data["color"] = color.rawValue
        return self
    }

    @discardableResult
    public func withIsSubtle(_ isSubtle: Bool) -> TextBlockBuilder {
        data["isSubtle"] = isSubtle
        return self
    }

    /// Convenience method that sets isSubtle to true.
    @discardableResult
    public func withSubtle() -> TextBlockBuilder {
        return withIsSubtle(true)
    }

    @discardableResult
    public func withWrap(_ wrap: Bool) -> TextBlockBuilder {
        data["wrap"] = wrap
        return self
    }

    @discardableResult
    public func withMaxLines(_ maxLines: Int) -> TextBlockBuilder {
        data["maxLines"] = maxLines
        return self
    }

    @discardableResult
    public func withHorizontalAlignment(_ alignment: HorizontalAlignment) -> TextBlockBuilder {
        data["horizontalAlignment"] = alignment.rawValue
        return self
    }

    @discardableResult
    public func withFontType(_ fontType: FontType) -> TextBlockBuilder {
        data["fontType"] = fontType.rawValue
        return self
    }

    @discardableResult
    public func withStyle(_ style: TextBlockStyle) -> TextBlockBuilder {
        data["style"] = style.rawValue
        return self
    }

    @discardableResult
    public func withSpacing(_ spacing: Spacing) -> TextBlockBuilder {
        data["spacing"] = spacing.rawValue
        return self
    }

    @discardableResult
    public func withSeparator(_ separator: Bool) -> TextBlockBuilder {
        data["separator"] = separator
        return self
    }

    @discardableResult
    public func withIsVisible(_ isVisible: Bool) -> TextBlockBuilder {
        data["isVisible"] = isVisible
        return self
    }

    @discardableResult
    public func withSelectAction(_ action: Card) -> TextBlockBuilder {
        data["selectAction"] = action
        return self
    }

    public func build() -> Card {
        return data
    }
}
