/// ImageBuilder builds an Image Adaptive Card element.
public final class ImageBuilder {
    var data: [String: Any] = ["type": "Image"]

    public init() {}

    @discardableResult
    public func withID(_ id: String) -> ImageBuilder {
        data["id"] = id
        return self
    }

    @discardableResult
    public func withURL(_ url: String) -> ImageBuilder {
        data["url"] = url
        return self
    }

    @discardableResult
    public func withAltText(_ altText: String) -> ImageBuilder {
        data["altText"] = altText
        return self
    }

    @discardableResult
    public func withSize(_ size: ImageSize) -> ImageBuilder {
        data["size"] = size.rawValue
        return self
    }

    @discardableResult
    public func withStyle(_ style: ImageStyle) -> ImageBuilder {
        data["style"] = style.rawValue
        return self
    }

    @discardableResult
    public func withWidth(_ width: String) -> ImageBuilder {
        data["width"] = width
        return self
    }

    @discardableResult
    public func withHeight(_ height: String) -> ImageBuilder {
        data["height"] = height
        return self
    }

    @discardableResult
    public func withHorizontalAlignment(_ alignment: HorizontalAlignment) -> ImageBuilder {
        data["horizontalAlignment"] = alignment.rawValue
        return self
    }

    @discardableResult
    public func withBackgroundColor(_ color: String) -> ImageBuilder {
        data["backgroundColor"] = color
        return self
    }

    @discardableResult
    public func withSpacing(_ spacing: Spacing) -> ImageBuilder {
        data["spacing"] = spacing.rawValue
        return self
    }

    @discardableResult
    public func withSeparator(_ separator: Bool) -> ImageBuilder {
        data["separator"] = separator
        return self
    }

    @discardableResult
    public func withSelectAction(_ configure: (ActionBuilder) -> Void) -> ImageBuilder {
        let ab = ActionBuilder()
        configure(ab)
        data["selectAction"] = ab.build()
        return self
    }

    public func build() -> Card {
        return data
    }
}
