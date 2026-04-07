/// InputChoiceSetBuilder builds an Input.ChoiceSet Adaptive Card element.
public final class InputChoiceSetBuilder {
    var data: [String: Any] = ["type": "Input.ChoiceSet", "id": "", "choices": [Any]()]

    public init() {}

    @discardableResult
    public func withID(_ id: String) -> InputChoiceSetBuilder {
        data["id"] = id
        return self
    }

    @discardableResult
    public func withLabel(_ label: String) -> InputChoiceSetBuilder {
        data["label"] = label
        return self
    }

    @discardableResult
    public func withPlaceholder(_ placeholder: String) -> InputChoiceSetBuilder {
        data["placeholder"] = placeholder
        return self
    }

    @discardableResult
    public func withValue(_ value: String) -> InputChoiceSetBuilder {
        data["value"] = value
        return self
    }

    @discardableResult
    public func withStyle(_ style: ChoiceInputStyle) -> InputChoiceSetBuilder {
        data["style"] = style.rawValue
        return self
    }

    @discardableResult
    public func withIsMultiSelect(_ isMultiSelect: Bool) -> InputChoiceSetBuilder {
        data["isMultiSelect"] = isMultiSelect
        return self
    }

    @discardableResult
    public func withWrap(_ wrap: Bool) -> InputChoiceSetBuilder {
        data["wrap"] = wrap
        return self
    }

    @discardableResult
    public func withIsRequired(_ isRequired: Bool) -> InputChoiceSetBuilder {
        data["isRequired"] = isRequired
        return self
    }

    @discardableResult
    public func withErrorMessage(_ errorMessage: String) -> InputChoiceSetBuilder {
        data["errorMessage"] = errorMessage
        return self
    }

    @discardableResult
    public func withSpacing(_ spacing: Spacing) -> InputChoiceSetBuilder {
        data["spacing"] = spacing.rawValue
        return self
    }

    /// Adds a choice with the given title and value strings.
    @discardableResult
    public func addChoice(_ title: String, _ value: String) -> InputChoiceSetBuilder {
        var choices = data["choices"] as! [Any]
        choices.append(["title": title, "value": value] as [String: Any])
        data["choices"] = choices
        return self
    }

    /// Adds a pre-built choice map directly.
    @discardableResult
    public func addChoiceMap(_ choice: [String: Any]) -> InputChoiceSetBuilder {
        var choices = data["choices"] as! [Any]
        choices.append(choice)
        data["choices"] = choices
        return self
    }

    /// Sets a dynamic data query for fetching choices from a data source (Adaptive Cards 1.6+).
    @discardableResult
    public func withChoicesData(_ dataset: String) -> InputChoiceSetBuilder {
        data["choices.data"] = ["type": "Data.Query", "dataset": dataset] as [String: Any]
        return self
    }

    public func build() -> Card {
        return data
    }
}
