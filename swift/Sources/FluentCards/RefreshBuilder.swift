/// RefreshBuilder builds the refresh configuration for an Adaptive Card.
public final class RefreshBuilder {
    var data: [String: Any] = [:]

    public init() {}

    @discardableResult
    public func withAction(_ configure: (ActionBuilder) -> Void) -> RefreshBuilder {
        let ab = ActionBuilder()
        configure(ab)
        data["action"] = ab.build()
        return self
    }

    @discardableResult
    public func addUserID(_ userID: String) -> RefreshBuilder {
        if data["userIds"] == nil {
            data["userIds"] = [Any]()
        }
        var userIds = data["userIds"] as! [Any]
        userIds.append(userID)
        data["userIds"] = userIds
        return self
    }

    @discardableResult
    public func withExpires(_ expires: String) -> RefreshBuilder {
        data["expires"] = expires
        return self
    }

    public func build() -> Card {
        return data
    }
}
