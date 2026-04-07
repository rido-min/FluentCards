/// FactSetBuilder builds a FactSet Adaptive Card element.
public final class FactSetBuilder {
    var data: [String: Any] = ["type": "FactSet", "facts": [Any]()]

    public init() {}

    @discardableResult
    public func withID(_ id: String) -> FactSetBuilder {
        data["id"] = id
        return self
    }

    @discardableResult
    public func withSpacing(_ spacing: Spacing) -> FactSetBuilder {
        data["spacing"] = spacing.rawValue
        return self
    }

    /// Adds a fact with the given title and value strings.
    @discardableResult
    public func addFact(_ title: String, _ value: String) -> FactSetBuilder {
        var facts = data["facts"] as! [Any]
        facts.append(["title": title, "value": value] as [String: Any])
        data["facts"] = facts
        return self
    }

    /// Adds a pre-built fact map directly.
    @discardableResult
    public func addFactMap(_ fact: [String: Any]) -> FactSetBuilder {
        var facts = data["facts"] as! [Any]
        facts.append(fact)
        data["facts"] = facts
        return self
    }

    public func build() -> Card {
        return data
    }
}
