/// ActionBuilder builds an Adaptive Card action (OpenUrl, Submit, ShowCard, ToggleVisibility, Execute).
/// Call one of openURL/submit/showCard/toggleVisibility/execute to set the action type,
/// then use with* methods to configure it.
public final class ActionBuilder {
    var data: [String: Any]? = nil

    public init() {}

    /// Creates an Action.OpenUrl action.
    @discardableResult
    public func openURL(_ url: String) -> ActionBuilder {
        data = ["type": "Action.OpenUrl", "url": url]
        return self
    }

    /// Creates an Action.Submit action.
    @discardableResult
    public func submit(_ title: String? = nil) -> ActionBuilder {
        data = ["type": "Action.Submit"]
        if let t = title, !t.isEmpty {
            data!["title"] = t
        }
        return self
    }

    /// Creates an Action.ShowCard action.
    @discardableResult
    public func showCard(_ title: String? = nil) -> ActionBuilder {
        data = ["type": "Action.ShowCard"]
        if let t = title, !t.isEmpty {
            data!["title"] = t
        }
        return self
    }

    /// Creates an Action.ToggleVisibility action.
    @discardableResult
    public func toggleVisibility(_ title: String? = nil) -> ActionBuilder {
        data = ["type": "Action.ToggleVisibility"]
        if let t = title, !t.isEmpty {
            data!["title"] = t
        }
        return self
    }

    /// Creates an Action.Execute action.
    @discardableResult
    public func execute(_ title: String? = nil) -> ActionBuilder {
        data = ["type": "Action.Execute"]
        if let t = title, !t.isEmpty {
            data!["title"] = t
        }
        return self
    }

    @discardableResult
    public func withID(_ id: String) -> ActionBuilder {
        data?["id"] = id
        return self
    }

    @discardableResult
    public func withTitle(_ title: String) -> ActionBuilder {
        if data != nil {
            data!["title"] = title
        }
        return self
    }

    @discardableResult
    public func withIconURL(_ iconURL: String) -> ActionBuilder {
        if data != nil {
            data!["iconUrl"] = iconURL
        }
        return self
    }

    @discardableResult
    public func withStyle(_ style: ActionStyle) -> ActionBuilder {
        if data != nil {
            data!["style"] = style.rawValue
        }
        return self
    }

    @discardableResult
    public func withIsEnabled(_ isEnabled: Bool) -> ActionBuilder {
        if data != nil {
            data!["isEnabled"] = isEnabled
        }
        return self
    }

    @discardableResult
    public func withTooltip(_ tooltip: String) -> ActionBuilder {
        if data != nil {
            data!["tooltip"] = tooltip
        }
        return self
    }

    /// Sets the data payload for Action.Submit or Action.Execute.
    @discardableResult
    public func withData(_ value: Any) -> ActionBuilder {
        guard data != nil,
              let t = data!["type"] as? String,
              t == "Action.Submit" || t == "Action.Execute" else { return self }
        data!["data"] = value
        return self
    }

    /// Sets which inputs are submitted for Action.Submit or Action.Execute.
    @discardableResult
    public func withAssociatedInputs(_ ai: AssociatedInputs) -> ActionBuilder {
        guard data != nil,
              let t = data!["type"] as? String,
              t == "Action.Submit" || t == "Action.Execute" else { return self }
        data!["associatedInputs"] = ai.rawValue
        return self
    }

    /// Sets the verb for Action.Execute.
    @discardableResult
    public func withVerb(_ verb: String) -> ActionBuilder {
        guard data != nil,
              let t = data!["type"] as? String,
              t == "Action.Execute" else { return self }
        data!["verb"] = verb
        return self
    }

    /// Sets the nested card for Action.ShowCard.
    @discardableResult
    public func withCard(_ card: Card) -> ActionBuilder {
        guard data != nil,
              let t = data!["type"] as? String,
              t == "Action.ShowCard" else { return self }
        data!["card"] = card
        return self
    }

    /// Adds a target element for Action.ToggleVisibility.
    /// Pass isVisible as a Bool to pin visibility; pass nil to toggle.
    @discardableResult
    public func addTargetElement(_ elementID: String, isVisible: Bool? = nil) -> ActionBuilder {
        guard data != nil,
              let t = data!["type"] as? String,
              t == "Action.ToggleVisibility" else { return self }
        if data!["targetElements"] == nil {
            data!["targetElements"] = [Any]()
        }
        var targets = data!["targetElements"] as! [Any]
        if let visible = isVisible {
            targets.append(["elementId": elementID, "isVisible": visible] as [String: Any])
        } else {
            targets.append(elementID)
        }
        data!["targetElements"] = targets
        return self
    }

    /// Returns the built action Card. Causes a fatal error if no action type was set.
    public func build() -> Card {
        guard let d = data else {
            fatalError("ActionBuilder: no action type specified — call openURL, submit, showCard, toggleVisibility, or execute first")
        }
        return d
    }
}
