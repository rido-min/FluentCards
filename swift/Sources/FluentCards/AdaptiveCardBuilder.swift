/// AdaptiveCardBuilder builds a root Adaptive Card.
/// Use AdaptiveCardBuilder(), chain with*/add* methods, then call build().
public final class AdaptiveCardBuilder {
    var data: [String: Any]

    public init() {
        data = [
            "type": "AdaptiveCard",
            "version": "1.5",
            "$schema": schemaURLs["1.5"]!,
        ]
    }

    /// Sets the Adaptive Cards schema version (e.g. "1.5").
    /// The $schema URL is updated automatically for known versions.
    @discardableResult
    public func withVersion(_ version: String) -> AdaptiveCardBuilder {
        data["version"] = version
        if let url = schemaURLs[version] {
            data["$schema"] = url
        } else {
            data["$schema"] = "https://adaptivecards.io/schemas/adaptive-card.json"
        }
        return self
    }

    /// Overrides the $schema URL.
    @discardableResult
    public func withSchema(_ schema: String) -> AdaptiveCardBuilder {
        data["$schema"] = schema
        return self
    }

    @discardableResult
    public func withFallbackText(_ fallbackText: String) -> AdaptiveCardBuilder {
        data["fallbackText"] = fallbackText
        return self
    }

    @discardableResult
    public func withSpeak(_ speak: String) -> AdaptiveCardBuilder {
        data["speak"] = speak
        return self
    }

    @discardableResult
    public func withLang(_ lang: String) -> AdaptiveCardBuilder {
        data["lang"] = lang
        return self
    }

    @discardableResult
    public func withRTL(_ rtl: Bool) -> AdaptiveCardBuilder {
        data["rtl"] = rtl
        return self
    }

    @discardableResult
    public func withMinHeight(_ minHeight: String) -> AdaptiveCardBuilder {
        data["minHeight"] = minHeight
        return self
    }

    @discardableResult
    public func withVerticalContentAlignment(_ alignment: VerticalAlignment) -> AdaptiveCardBuilder {
        data["verticalContentAlignment"] = alignment.rawValue
        return self
    }

    @discardableResult
    public func withBackgroundImage(_ configure: (BackgroundImageBuilder) -> Void) -> AdaptiveCardBuilder {
        let bib = BackgroundImageBuilder()
        configure(bib)
        data["backgroundImage"] = bib.build()
        return self
    }

    @discardableResult
    public func withSelectAction(_ configure: (ActionBuilder) -> Void) -> AdaptiveCardBuilder {
        let ab = ActionBuilder()
        configure(ab)
        data["selectAction"] = ab.build()
        return self
    }

    @discardableResult
    public func withMetadata(_ webURL: String) -> AdaptiveCardBuilder {
        data["metadata"] = ["webUrl": webURL]
        return self
    }

    // MARK: - Body elements

    @discardableResult
    public func addTextBlock(_ configure: (TextBlockBuilder) -> Void) -> AdaptiveCardBuilder {
        let tb = TextBlockBuilder()
        configure(tb)
        pushBody(tb.build())
        return self
    }

    @discardableResult
    public func addImage(_ configure: (ImageBuilder) -> Void) -> AdaptiveCardBuilder {
        let ib = ImageBuilder()
        configure(ib)
        pushBody(ib.build())
        return self
    }

    @discardableResult
    public func addContainer(_ configure: (ContainerBuilder) -> Void) -> AdaptiveCardBuilder {
        let cb = ContainerBuilder()
        configure(cb)
        pushBody(cb.build())
        return self
    }

    @discardableResult
    public func addColumnSet(_ configure: (ColumnSetBuilder) -> Void) -> AdaptiveCardBuilder {
        let cs = ColumnSetBuilder()
        configure(cs)
        pushBody(cs.build())
        return self
    }

    @discardableResult
    public func addFactSet(_ configure: (FactSetBuilder) -> Void) -> AdaptiveCardBuilder {
        let fs = FactSetBuilder()
        configure(fs)
        pushBody(fs.build())
        return self
    }

    @discardableResult
    public func addRichTextBlock(_ configure: (RichTextBlockBuilder) -> Void) -> AdaptiveCardBuilder {
        let rtb = RichTextBlockBuilder()
        configure(rtb)
        pushBody(rtb.build())
        return self
    }

    @discardableResult
    public func addActionSet(_ configure: (ActionSetBuilder) -> Void) -> AdaptiveCardBuilder {
        let asb = ActionSetBuilder()
        configure(asb)
        pushBody(asb.build())
        return self
    }

    @discardableResult
    public func addMedia(_ configure: (MediaBuilder) -> Void) -> AdaptiveCardBuilder {
        let mb = MediaBuilder()
        configure(mb)
        pushBody(mb.build())
        return self
    }

    @discardableResult
    public func addImageSet(_ configure: (ImageSetBuilder) -> Void) -> AdaptiveCardBuilder {
        let isb = ImageSetBuilder()
        configure(isb)
        pushBody(isb.build())
        return self
    }

    @discardableResult
    public func addTable(_ configure: (TableBuilder) -> Void) -> AdaptiveCardBuilder {
        let tb = TableBuilder()
        configure(tb)
        pushBody(tb.build())
        return self
    }

    // MARK: - Input elements

    @discardableResult
    public func addInputText(_ configure: (InputTextBuilder) -> Void) -> AdaptiveCardBuilder {
        let ib = InputTextBuilder()
        configure(ib)
        pushBody(ib.build())
        return self
    }

    @discardableResult
    public func addInputNumber(_ configure: (InputNumberBuilder) -> Void) -> AdaptiveCardBuilder {
        let ib = InputNumberBuilder()
        configure(ib)
        pushBody(ib.build())
        return self
    }

    @discardableResult
    public func addInputDate(_ configure: (InputDateBuilder) -> Void) -> AdaptiveCardBuilder {
        let ib = InputDateBuilder()
        configure(ib)
        pushBody(ib.build())
        return self
    }

    @discardableResult
    public func addInputTime(_ configure: (InputTimeBuilder) -> Void) -> AdaptiveCardBuilder {
        let ib = InputTimeBuilder()
        configure(ib)
        pushBody(ib.build())
        return self
    }

    @discardableResult
    public func addInputToggle(_ configure: (InputToggleBuilder) -> Void) -> AdaptiveCardBuilder {
        let ib = InputToggleBuilder()
        configure(ib)
        pushBody(ib.build())
        return self
    }

    @discardableResult
    public func addInputChoiceSet(_ configure: (InputChoiceSetBuilder) -> Void) -> AdaptiveCardBuilder {
        let ib = InputChoiceSetBuilder()
        configure(ib)
        pushBody(ib.build())
        return self
    }

    /// Adds a pre-built element Card directly to the card body.
    @discardableResult
    public func addElement(_ element: Card) -> AdaptiveCardBuilder {
        pushBody(element)
        return self
    }

    // MARK: - Actions

    @discardableResult
    public func addAction(_ configure: (ActionBuilder) -> Void) -> AdaptiveCardBuilder {
        let ab = ActionBuilder()
        configure(ab)
        if data["actions"] == nil {
            data["actions"] = [Any]()
        }
        var actions = data["actions"] as! [Any]
        actions.append(ab.build())
        data["actions"] = actions
        return self
    }

    // MARK: - Advanced configuration

    @discardableResult
    public func withRefresh(_ configure: (RefreshBuilder) -> Void) -> AdaptiveCardBuilder {
        let rb = RefreshBuilder()
        configure(rb)
        data["refresh"] = rb.build()
        return self
    }

    @discardableResult
    public func withAuthentication(_ configure: (AuthenticationBuilder) -> Void) -> AdaptiveCardBuilder {
        let authb = AuthenticationBuilder()
        configure(authb)
        data["authentication"] = authb.build()
        return self
    }

    /// Returns the completed Adaptive Card as a Card ([String: Any]).
    public func build() -> Card {
        return data
    }

    private func pushBody(_ element: Card) {
        if data["body"] == nil {
            data["body"] = [Any]()
        }
        var body = data["body"] as! [Any]
        body.append(element)
        data["body"] = body
    }
}
