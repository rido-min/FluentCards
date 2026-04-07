/// InputDateBuilder builds an Input.Date Adaptive Card element.
public final class InputDateBuilder {
    var data: [String: Any] = ["type": "Input.Date", "id": ""]

    public init() {}

    @discardableResult
    public func withID(_ id: String) -> InputDateBuilder {
        data["id"] = id
        return self
    }

    @discardableResult
    public func withLabel(_ label: String) -> InputDateBuilder {
        data["label"] = label
        return self
    }

    @discardableResult
    public func withPlaceholder(_ placeholder: String) -> InputDateBuilder {
        data["placeholder"] = placeholder
        return self
    }

    @discardableResult
    public func withValue(_ value: String) -> InputDateBuilder {
        data["value"] = value
        return self
    }

    /// Sets the minimum date (format: YYYY-MM-DD).
    @discardableResult
    public func withMin(_ min: String) -> InputDateBuilder {
        data["min"] = min
        return self
    }

    /// Sets the maximum date (format: YYYY-MM-DD).
    @discardableResult
    public func withMax(_ max: String) -> InputDateBuilder {
        data["max"] = max
        return self
    }

    @discardableResult
    public func withIsRequired(_ isRequired: Bool) -> InputDateBuilder {
        data["isRequired"] = isRequired
        return self
    }

    @discardableResult
    public func withErrorMessage(_ errorMessage: String) -> InputDateBuilder {
        data["errorMessage"] = errorMessage
        return self
    }

    @discardableResult
    public func withSpacing(_ spacing: Spacing) -> InputDateBuilder {
        data["spacing"] = spacing.rawValue
        return self
    }

    public func build() -> Card {
        return data
    }
}
