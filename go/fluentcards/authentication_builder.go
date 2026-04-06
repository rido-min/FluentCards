package fluentcards

// AuthenticationBuilder builds the authentication configuration for an Adaptive Card.
type AuthenticationBuilder struct {
	data map[string]any
}

func newAuthenticationBuilder() *AuthenticationBuilder {
	return &AuthenticationBuilder{data: map[string]any{}}
}

func (b *AuthenticationBuilder) WithText(text string) *AuthenticationBuilder {
	b.data["text"] = text
	return b
}

func (b *AuthenticationBuilder) WithConnectionName(connectionName string) *AuthenticationBuilder {
	b.data["connectionName"] = connectionName
	return b
}

func (b *AuthenticationBuilder) WithTokenExchangeResource(resource map[string]any) *AuthenticationBuilder {
	b.data["tokenExchangeResource"] = resource
	return b
}

func (b *AuthenticationBuilder) AddButton(button map[string]any) *AuthenticationBuilder {
	if b.data["buttons"] == nil {
		b.data["buttons"] = []any{}
	}
	b.data["buttons"] = append(b.data["buttons"].([]any), button)
	return b
}

func (b *AuthenticationBuilder) Build() Card {
	return b.data
}
