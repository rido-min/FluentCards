/// InputTextBuilder builds an Input.Text Adaptive Card element.
public final class InputTextBuilder {
    var data: [String: Any] = ["type": "Input.Text", "id": ""]

    public init() {}

    @discardableResult
    public func withID(_ id: String) -> InputTextBuilder {
        data["id"] = id
        return self
    }

    @discardableResult
    public func withLabel(_ label: String) -> InputTextBuilder {
        data["label"] = label
        return self
    }

    @discardableResult
    public func withPlaceholder(_ placeholder: String) -> InputTextBuilder {
        data["placeholder"] = placeholder
        return self
    }

    @discardableResult
    public func withValue(_ value: String) -> InputTextBuilder {
        data["value"] = value
        return self
    }

    @discardableResult
    public func withMaxLength(_ maxLength: Int) -> InputTextBuilder {
        data["maxLength"] = maxLength
        return self
    }

    @discardableResult
    public func withIsMultiline(_ isMultiline: Bool) -> InputTextBuilder {
        data["isMultiline"] = isMultiline
        return self
    }

    @discardableResult
    public func withStyle(_ style: TextInputStyle) -> InputTextBuilder {
        data["style"] = style.rawValue
        return self
    }

    @discardableResult
    public func withRegex(_ regex: String) -> InputTextBuilder {
        data["regex"] = regex
        return self
    }

    @discardableResult
    public func withIsRequired(_ isRequired: Bool) -> InputTextBuilder {
        data["isRequired"] = isRequired
        return self
    }

    @discardableResult
    public func withErrorMessage(_ errorMessage: String) -> InputTextBuilder {
        data["errorMessage"] = errorMessage
        return self
    }

    @discardableResult
    public func withSpacing(_ spacing: Spacing) -> InputTextBuilder {
        data["spacing"] = spacing.rawValue
        return self
    }

    @discardableResult
    public func withInlineAction(_ configure: (ActionBuilder) -> Void) -> InputTextBuilder {
        let ab = ActionBuilder()
        configure(ab)
        data["inlineAction"] = ab.build()
        return self
    }

    public func build() -> Card {
        return data
    }
}
