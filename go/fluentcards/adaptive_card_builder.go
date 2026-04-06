package fluentcards

// AdaptiveCardBuilder builds a root Adaptive Card.
// Use NewAdaptiveCardBuilder() to create one, chain With*/Add* methods, then call Build().
type AdaptiveCardBuilder struct {
	data map[string]any
}

// NewAdaptiveCardBuilder returns a new AdaptiveCardBuilder with default version 1.5.
func NewAdaptiveCardBuilder() *AdaptiveCardBuilder {
	return &AdaptiveCardBuilder{data: map[string]any{
		"type":    "AdaptiveCard",
		"version": "1.5",
		"$schema": schemaURLs["1.5"],
	}}
}

// WithVersion sets the Adaptive Cards schema version (e.g. "1.5").
// The $schema URL is updated automatically for known versions.
func (b *AdaptiveCardBuilder) WithVersion(version string) *AdaptiveCardBuilder {
	b.data["version"] = version
	if url, ok := schemaURLs[version]; ok {
		b.data["$schema"] = url
	} else {
		b.data["$schema"] = "http://adaptivecards.io/schemas/adaptive-card.json"
	}
	return b
}

// WithSchema overrides the $schema URL.
func (b *AdaptiveCardBuilder) WithSchema(schema string) *AdaptiveCardBuilder {
	b.data["$schema"] = schema
	return b
}

func (b *AdaptiveCardBuilder) WithFallbackText(fallbackText string) *AdaptiveCardBuilder {
	b.data["fallbackText"] = fallbackText
	return b
}

func (b *AdaptiveCardBuilder) WithSpeak(speak string) *AdaptiveCardBuilder {
	b.data["speak"] = speak
	return b
}

func (b *AdaptiveCardBuilder) WithLang(lang string) *AdaptiveCardBuilder {
	b.data["lang"] = lang
	return b
}

func (b *AdaptiveCardBuilder) WithRTL(rtl bool) *AdaptiveCardBuilder {
	b.data["rtl"] = rtl
	return b
}

func (b *AdaptiveCardBuilder) WithMinHeight(minHeight string) *AdaptiveCardBuilder {
	b.data["minHeight"] = minHeight
	return b
}

func (b *AdaptiveCardBuilder) WithVerticalContentAlignment(alignment VerticalAlignment) *AdaptiveCardBuilder {
	b.data["verticalContentAlignment"] = string(alignment)
	return b
}

func (b *AdaptiveCardBuilder) WithBackgroundImage(configure func(*BackgroundImageBuilder)) *AdaptiveCardBuilder {
	bib := newBackgroundImageBuilder()
	configure(bib)
	b.data["backgroundImage"] = bib.Build()
	return b
}

func (b *AdaptiveCardBuilder) WithSelectAction(configure func(*ActionBuilder)) *AdaptiveCardBuilder {
	ab := newActionBuilder()
	configure(ab)
	b.data["selectAction"] = ab.Build()
	return b
}

func (b *AdaptiveCardBuilder) WithMetadata(webURL string) *AdaptiveCardBuilder {
	b.data["metadata"] = map[string]any{"webUrl": webURL}
	return b
}

// ── Body elements ─────────────────────────────────────────────────────────────

func (b *AdaptiveCardBuilder) AddTextBlock(configure func(*TextBlockBuilder)) *AdaptiveCardBuilder {
	tb := newTextBlockBuilder()
	configure(tb)
	b.pushBody(tb.Build())
	return b
}

func (b *AdaptiveCardBuilder) AddImage(configure func(*ImageBuilder)) *AdaptiveCardBuilder {
	ib := newImageBuilder()
	configure(ib)
	b.pushBody(ib.Build())
	return b
}

func (b *AdaptiveCardBuilder) AddContainer(configure func(*ContainerBuilder)) *AdaptiveCardBuilder {
	cb := newContainerBuilder()
	configure(cb)
	b.pushBody(cb.Build())
	return b
}

func (b *AdaptiveCardBuilder) AddColumnSet(configure func(*ColumnSetBuilder)) *AdaptiveCardBuilder {
	cs := newColumnSetBuilder()
	configure(cs)
	b.pushBody(cs.Build())
	return b
}

func (b *AdaptiveCardBuilder) AddFactSet(configure func(*FactSetBuilder)) *AdaptiveCardBuilder {
	fs := newFactSetBuilder()
	configure(fs)
	b.pushBody(fs.Build())
	return b
}

func (b *AdaptiveCardBuilder) AddRichTextBlock(configure func(*RichTextBlockBuilder)) *AdaptiveCardBuilder {
	rtb := newRichTextBlockBuilder()
	configure(rtb)
	b.pushBody(rtb.Build())
	return b
}

func (b *AdaptiveCardBuilder) AddActionSet(configure func(*ActionSetBuilder)) *AdaptiveCardBuilder {
	as := newActionSetBuilder()
	configure(as)
	b.pushBody(as.Build())
	return b
}

func (b *AdaptiveCardBuilder) AddMedia(configure func(*MediaBuilder)) *AdaptiveCardBuilder {
	mb := newMediaBuilder()
	configure(mb)
	b.pushBody(mb.Build())
	return b
}

func (b *AdaptiveCardBuilder) AddImageSet(configure func(*ImageSetBuilder)) *AdaptiveCardBuilder {
	isb := newImageSetBuilder()
	configure(isb)
	b.pushBody(isb.Build())
	return b
}

func (b *AdaptiveCardBuilder) AddTable(configure func(*TableBuilder)) *AdaptiveCardBuilder {
	tb := newTableBuilder()
	configure(tb)
	b.pushBody(tb.Build())
	return b
}

// ── Input elements ────────────────────────────────────────────────────────────

func (b *AdaptiveCardBuilder) AddInputText(configure func(*InputTextBuilder)) *AdaptiveCardBuilder {
	ib := newInputTextBuilder()
	configure(ib)
	b.pushBody(ib.Build())
	return b
}

func (b *AdaptiveCardBuilder) AddInputNumber(configure func(*InputNumberBuilder)) *AdaptiveCardBuilder {
	ib := newInputNumberBuilder()
	configure(ib)
	b.pushBody(ib.Build())
	return b
}

func (b *AdaptiveCardBuilder) AddInputDate(configure func(*InputDateBuilder)) *AdaptiveCardBuilder {
	ib := newInputDateBuilder()
	configure(ib)
	b.pushBody(ib.Build())
	return b
}

func (b *AdaptiveCardBuilder) AddInputTime(configure func(*InputTimeBuilder)) *AdaptiveCardBuilder {
	ib := newInputTimeBuilder()
	configure(ib)
	b.pushBody(ib.Build())
	return b
}

func (b *AdaptiveCardBuilder) AddInputToggle(configure func(*InputToggleBuilder)) *AdaptiveCardBuilder {
	ib := newInputToggleBuilder()
	configure(ib)
	b.pushBody(ib.Build())
	return b
}

func (b *AdaptiveCardBuilder) AddInputChoiceSet(configure func(*InputChoiceSetBuilder)) *AdaptiveCardBuilder {
	ib := newInputChoiceSetBuilder()
	configure(ib)
	b.pushBody(ib.Build())
	return b
}

// AddElement adds a pre-built element Card directly to the card body.
func (b *AdaptiveCardBuilder) AddElement(element Card) *AdaptiveCardBuilder {
	b.pushBody(element)
	return b
}

// ── Actions ───────────────────────────────────────────────────────────────────

func (b *AdaptiveCardBuilder) AddAction(configure func(*ActionBuilder)) *AdaptiveCardBuilder {
	ab := newActionBuilder()
	configure(ab)
	if b.data["actions"] == nil {
		b.data["actions"] = []any{}
	}
	b.data["actions"] = append(b.data["actions"].([]any), ab.Build())
	return b
}

// ── Advanced configuration ────────────────────────────────────────────────────

func (b *AdaptiveCardBuilder) WithRefresh(configure func(*RefreshBuilder)) *AdaptiveCardBuilder {
	rb := newRefreshBuilder()
	configure(rb)
	b.data["refresh"] = rb.Build()
	return b
}

func (b *AdaptiveCardBuilder) WithAuthentication(configure func(*AuthenticationBuilder)) *AdaptiveCardBuilder {
	authb := newAuthenticationBuilder()
	configure(authb)
	b.data["authentication"] = authb.Build()
	return b
}

// Build returns the completed Adaptive Card as a Card (map[string]any).
func (b *AdaptiveCardBuilder) Build() Card {
	return b.data
}

func (b *AdaptiveCardBuilder) pushBody(element Card) {
	if b.data["body"] == nil {
		b.data["body"] = []any{}
	}
	b.data["body"] = append(b.data["body"].([]any), element)
}
