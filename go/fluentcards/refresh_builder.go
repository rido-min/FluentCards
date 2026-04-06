package fluentcards

// RefreshBuilder builds the refresh configuration for an Adaptive Card.
type RefreshBuilder struct {
	data map[string]any
}

func newRefreshBuilder() *RefreshBuilder {
	return &RefreshBuilder{data: map[string]any{}}
}

func (b *RefreshBuilder) WithAction(configure func(*ActionBuilder)) *RefreshBuilder {
	ab := newActionBuilder()
	configure(ab)
	b.data["action"] = ab.Build()
	return b
}

func (b *RefreshBuilder) AddUserID(userID string) *RefreshBuilder {
	if b.data["userIds"] == nil {
		b.data["userIds"] = []any{}
	}
	b.data["userIds"] = append(b.data["userIds"].([]any), userID)
	return b
}

func (b *RefreshBuilder) WithExpires(expires string) *RefreshBuilder {
	b.data["expires"] = expires
	return b
}

func (b *RefreshBuilder) Build() Card {
	return b.data
}
