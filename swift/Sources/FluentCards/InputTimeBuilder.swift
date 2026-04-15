/// InputTimeBuilder builds an Input.Time Adaptive Card element.
public final class InputTimeBuilder {
    var data: [String: Any] = ["type": "Input.Time", "id": ""]

    public init() {}

    @discardableResult
    public func withID(_ id: String) -> InputTimeBuilder {
        data["id"] = id
        return self
    }

    @discardableResult
    public func withLabel(_ label: String) -> InputTimeBuilder {
        data["label"] = label
        return self
    }

    @discardableResult
    public func withPlaceholder(_ placeholder: String) -> InputTimeBuilder {
        data["placeholder"] = placeholder
        return self
    }

    @discardableResult
    public func withValue(_ value: String) -> InputTimeBuilder {
        data["value"] = value
        return self
    }

    /// Sets the minimum time (format: HH:MM).
    @discardableResult
    public func withMin(_ min: String) -> InputTimeBuilder {
        data["min"] = min
        return self
    }

    /// Sets the maximum time (format: HH:MM).
    @discardableResult
    public func withMax(_ max: String) -> InputTimeBuilder {
        data["max"] = max
        return self
    }

    @discardableResult
    public func withIsRequired(_ isRequired: Bool) -> InputTimeBuilder {
        data["isRequired"] = isRequired
        return self
    }

    @discardableResult
    public func withErrorMessage(_ errorMessage: String) -> InputTimeBuilder {
        data["errorMessage"] = errorMessage
        return self
    }

    @discardableResult
    public func withSpacing(_ spacing: Spacing) -> InputTimeBuilder {
        data["spacing"] = spacing.rawValue
        return self
    }

    public func build() -> Card {
        return data
    }
}
