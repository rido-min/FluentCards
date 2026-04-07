/// BackgroundImageBuilder builds a backgroundImage object for containers and cards.
public final class BackgroundImageBuilder {
    var data: [String: Any] = [:]

    public init() {}

    @discardableResult
    public func withURL(_ url: String) -> BackgroundImageBuilder {
        data["url"] = url
        return self
    }

    @discardableResult
    public func withFillMode(_ fillMode: BackgroundImageFillMode) -> BackgroundImageBuilder {
        data["fillMode"] = fillMode.rawValue
        return self
    }

    @discardableResult
    public func withHorizontalAlignment(_ alignment: HorizontalAlignment) -> BackgroundImageBuilder {
        data["horizontalAlignment"] = alignment.rawValue
        return self
    }

    @discardableResult
    public func withVerticalAlignment(_ alignment: VerticalAlignment) -> BackgroundImageBuilder {
        data["verticalAlignment"] = alignment.rawValue
        return self
    }

    public func build() -> Card {
        return data
    }
}
