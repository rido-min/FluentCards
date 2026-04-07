/// MediaBuilder builds a Media Adaptive Card element.
public final class MediaBuilder {
    var data: [String: Any] = ["type": "Media", "sources": [Any]()]

    public init() {}

    @discardableResult
    public func withID(_ id: String) -> MediaBuilder {
        data["id"] = id
        return self
    }

    @discardableResult
    public func withPoster(_ poster: String) -> MediaBuilder {
        data["poster"] = poster
        return self
    }

    @discardableResult
    public func withAltText(_ altText: String) -> MediaBuilder {
        data["altText"] = altText
        return self
    }

    @discardableResult
    public func withSpacing(_ spacing: Spacing) -> MediaBuilder {
        data["spacing"] = spacing.rawValue
        return self
    }

    /// Adds a media source with the given URL and MIME type.
    @discardableResult
    public func addSource(_ url: String, _ mimeType: String) -> MediaBuilder {
        var sources = data["sources"] as! [Any]
        sources.append(["url": url, "mimeType": mimeType] as [String: Any])
        data["sources"] = sources
        return self
    }

    /// Adds a pre-built source map directly.
    @discardableResult
    public func addSourceMap(_ source: [String: Any]) -> MediaBuilder {
        var sources = data["sources"] as! [Any]
        sources.append(source)
        data["sources"] = sources
        return self
    }

    public func build() -> Card {
        return data
    }
}
