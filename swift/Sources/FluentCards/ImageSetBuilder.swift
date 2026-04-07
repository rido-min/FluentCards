/// ImageSetBuilder builds an ImageSet Adaptive Card element.
public final class ImageSetBuilder {
    var data: [String: Any] = ["type": "ImageSet", "images": [Any]()]

    public init() {}

    @discardableResult
    public func withID(_ id: String) -> ImageSetBuilder {
        data["id"] = id
        return self
    }

    @discardableResult
    public func withImageSize(_ size: ImageSize) -> ImageSetBuilder {
        data["imageSize"] = size.rawValue
        return self
    }

    @discardableResult
    public func withSpacing(_ spacing: Spacing) -> ImageSetBuilder {
        data["spacing"] = spacing.rawValue
        return self
    }

    /// Adds an image configured by the provided closure.
    @discardableResult
    public func addImage(_ configure: (ImageBuilder) -> Void) -> ImageSetBuilder {
        let ib = ImageBuilder()
        configure(ib)
        var images = data["images"] as! [Any]
        images.append(ib.build())
        data["images"] = images
        return self
    }

    public func build() -> Card {
        return data
    }
}
