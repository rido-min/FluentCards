/// ActionSetBuilder builds an ActionSet body element (a group of actions within the card body).
public final class ActionSetBuilder {
    var data: [String: Any] = ["type": "ActionSet", "actions": [Any]()]

    public init() {}

    @discardableResult
    public func withID(_ id: String) -> ActionSetBuilder {
        data["id"] = id
        return self
    }

    @discardableResult
    public func withSpacing(_ spacing: Spacing) -> ActionSetBuilder {
        data["spacing"] = spacing.rawValue
        return self
    }

    @discardableResult
    public func addAction(_ configure: (ActionBuilder) -> Void) -> ActionSetBuilder {
        let ab = ActionBuilder()
        configure(ab)
        var actions = data["actions"] as! [Any]
        actions.append(ab.build())
        data["actions"] = actions
        return self
    }

    public func build() -> Card {
        return data
    }
}
