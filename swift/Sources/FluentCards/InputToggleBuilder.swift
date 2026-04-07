/// InputToggleBuilder builds an Input.Toggle Adaptive Card element.
public final class InputToggleBuilder {
    var data: [String: Any] = ["type": "Input.Toggle", "id": "", "title": ""]

    public init() {}

    @discardableResult
    public func withID(_ id: String) -> InputToggleBuilder {
        data["id"] = id
        return self
    }

    @discardableResult
    public func withTitle(_ title: String) -> InputToggleBuilder {
        data["title"] = title
        return self
    }

    @discardableResult
    public func withLabel(_ label: String) -> InputToggleBuilder {
        data["label"] = label
        return self
    }

    @discardableResult
    public func withValue(_ value: String) -> InputToggleBuilder {
        data["value"] = value
        return self
    }

    @discardableResult
    public func withValueOn(_ valueOn: String) -> InputToggleBuilder {
        data["valueOn"] = valueOn
        return self
    }

    @discardableResult
    public func withValueOff(_ valueOff: String) -> InputToggleBuilder {
        data["valueOff"] = valueOff
        return self
    }

    @discardableResult
    public func withWrap(_ wrap: Bool) -> InputToggleBuilder {
        data["wrap"] = wrap
        return self
    }

    @discardableResult
    public func withIsRequired(_ isRequired: Bool) -> InputToggleBuilder {
        data["isRequired"] = isRequired
        return self
    }

    @discardableResult
    public func withErrorMessage(_ errorMessage: String) -> InputToggleBuilder {
        data["errorMessage"] = errorMessage
        return self
    }

    @discardableResult
    public func withSpacing(_ spacing: Spacing) -> InputToggleBuilder {
        data["spacing"] = spacing.rawValue
        return self
    }

    public func build() -> Card {
        return data
    }
}
