/// AuthenticationBuilder builds the authentication configuration for an Adaptive Card.
public final class AuthenticationBuilder {
    var data: [String: Any] = [:]

    public init() {}

    @discardableResult
    public func withText(_ text: String) -> AuthenticationBuilder {
        data["text"] = text
        return self
    }

    @discardableResult
    public func withConnectionName(_ connectionName: String) -> AuthenticationBuilder {
        data["connectionName"] = connectionName
        return self
    }

    @discardableResult
    public func withTokenExchangeResource(_ resource: [String: Any]) -> AuthenticationBuilder {
        data["tokenExchangeResource"] = resource
        return self
    }

    @discardableResult
    public func addButton(_ button: [String: Any]) -> AuthenticationBuilder {
        if data["buttons"] == nil {
            data["buttons"] = [Any]()
        }
        var buttons = data["buttons"] as! [Any]
        buttons.append(button)
        data["buttons"] = buttons
        return self
    }

    public func build() -> Card {
        return data
    }
}
