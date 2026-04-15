/// InputNumberBuilder builds an Input.Number Adaptive Card element.
public final class InputNumberBuilder {
    var data: [String: Any] = ["type": "Input.Number", "id": ""]

    public init() {}

    @discardableResult
    public func withID(_ id: String) -> InputNumberBuilder {
        data["id"] = id
        return self
    }

    @discardableResult
    public func withLabel(_ label: String) -> InputNumberBuilder {
        data["label"] = label
        return self
    }

    @discardableResult
    public func withPlaceholder(_ placeholder: String) -> InputNumberBuilder {
        data["placeholder"] = placeholder
        return self
    }

    @discardableResult
    public func withValue(_ value: Double) -> InputNumberBuilder {
        data["value"] = value
        return self
    }

    @discardableResult
    public func withMin(_ min: Double) -> InputNumberBuilder {
        data["min"] = min
        return self
    }

    @discardableResult
    public func withMax(_ max: Double) -> InputNumberBuilder {
        data["max"] = max
        return self
    }

    @discardableResult
    public func withIsRequired(_ isRequired: Bool) -> InputNumberBuilder {
        data["isRequired"] = isRequired
        return self
    }

    @discardableResult
    public func withErrorMessage(_ errorMessage: String) -> InputNumberBuilder {
        data["errorMessage"] = errorMessage
        return self
    }

    @discardableResult
    public func withSpacing(_ spacing: Spacing) -> InputNumberBuilder {
        data["spacing"] = spacing.rawValue
        return self
    }

    public func build() -> Card {
        return data
    }
}
