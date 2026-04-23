package fluentcards

import (
	"testing"

	"github.com/stretchr/testify/assert"
	"github.com/stretchr/testify/require"
)

// ── Version handling ─────────────────────────────────────────────────────────

func TestAdvanced_Version_AllKnown(t *testing.T) {
	t.Parallel()
	versions := []struct {
		ver    string
		schema string
	}{
		{"1.0", "https://adaptivecards.io/schemas/1.0.0/adaptive-card.json"},
		{"1.1", "https://adaptivecards.io/schemas/1.1.0/adaptive-card.json"},
		{"1.2", "https://adaptivecards.io/schemas/1.2.0/adaptive-card.json"},
		{"1.3", "https://adaptivecards.io/schemas/1.3.0/adaptive-card.json"},
		{"1.4", "https://adaptivecards.io/schemas/1.4.0/adaptive-card.json"},
		{"1.5", "https://adaptivecards.io/schemas/1.5.0/adaptive-card.json"},
		{"1.6", "https://adaptivecards.io/schemas/1.6.0/adaptive-card.json"},
	}
	for _, tc := range versions {
		card := NewAdaptiveCardBuilder().WithVersion(tc.ver).Build()
		assert.Equal(t, tc.ver, card["version"], "version %s", tc.ver)
		assert.Equal(t, tc.schema, card["$schema"], "schema for version %s", tc.ver)
	}
}

func TestAdvanced_Version_Unknown(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().WithVersion("9.9").Build()
	assert.Equal(t, "9.9", card["version"])
	assert.Equal(t, "http://adaptivecards.io/schemas/adaptive-card.json", card["$schema"])
}

func TestAdvanced_Version_SchemaOverride(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().
		WithVersion("1.5").
		WithSchema("https://custom.schema/v1").
		Build()
	assert.Equal(t, "https://custom.schema/v1", card["$schema"])
}

// ── Authentication ──────────────────────────────────────────────────────────

func TestAdvanced_Authentication_Full(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().
		WithAuthentication(func(auth *AuthenticationBuilder) {
			auth.
				WithText("Please sign in").
				WithConnectionName("myConnection").
				WithTokenExchangeResource(map[string]any{
					"id":  "res1",
					"uri": "https://token.example.com",
				}).
				AddButton(map[string]any{
					"type":  "signin",
					"title": "Sign In",
					"value": "https://login.example.com",
				})
		}).
		Build()

	authData := card["authentication"].(Card)
	assert.Equal(t, "Please sign in", authData["text"])
	assert.Equal(t, "myConnection", authData["connectionName"])

	ter := authData["tokenExchangeResource"].(map[string]any)
	assert.Equal(t, "res1", ter["id"])
	assert.Equal(t, "https://token.example.com", ter["uri"])

	buttons := authData["buttons"].([]any)
	require.Len(t, buttons, 1)
	btn := buttons[0].(map[string]any)
	assert.Equal(t, "signin", btn["type"])
	assert.Equal(t, "Sign In", btn["title"])
}

func TestAdvanced_Authentication_MultipleButtons(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().
		WithAuthentication(func(auth *AuthenticationBuilder) {
			auth.
				AddButton(map[string]any{"type": "signin", "title": "SSO"}).
				AddButton(map[string]any{"type": "signin", "title": "OAuth"})
		}).
		Build()

	authData := card["authentication"].(Card)
	buttons := authData["buttons"].([]any)
	require.Len(t, buttons, 2)
}

// ── Refresh ─────────────────────────────────────────────────────────────────

func TestAdvanced_Refresh_Full(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().
		WithRefresh(func(r *RefreshBuilder) {
			r.
				WithAction(func(a *ActionBuilder) {
					a.Execute("Refresh").WithVerb("refresh")
				}).
				AddUserID("user1").
				AddUserID("user2").
				WithExpires("2024-12-31T23:59:59Z")
		}).
		Build()

	refresh := card["refresh"].(Card)
	action := refresh["action"].(Card)
	assert.Equal(t, "Action.Execute", action["type"])
	assert.Equal(t, "refresh", action["verb"])

	userIds := refresh["userIds"].([]any)
	require.Len(t, userIds, 2)
	assert.Equal(t, "user1", userIds[0])
	assert.Equal(t, "user2", userIds[1])

	assert.Equal(t, "2024-12-31T23:59:59Z", refresh["expires"])
}

func TestAdvanced_Refresh_ActionOnly(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().
		WithRefresh(func(r *RefreshBuilder) {
			r.WithAction(func(a *ActionBuilder) {
				a.Execute("Refresh")
			})
		}).
		Build()

	refresh := card["refresh"].(Card)
	assert.NotNil(t, refresh["action"])
	assert.Nil(t, refresh["userIds"])
	assert.Nil(t, refresh["expires"])
}

// ── Metadata ────────────────────────────────────────────────────────────────

func TestAdvanced_Metadata(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().
		WithMetadata("https://example.com/card-meta").
		Build()

	meta := card["metadata"].(Card)
	assert.Equal(t, "https://example.com/card-meta", meta["webUrl"])
}

// ── FallbackText, Speak, Lang, RTL, MinHeight ───────────────────────────────

func TestAdvanced_FallbackText(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().WithFallbackText("Cannot display card").Build()
	assert.Equal(t, "Cannot display card", card["fallbackText"])
}

func TestAdvanced_Speak(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().WithSpeak("Hello from the card").Build()
	assert.Equal(t, "Hello from the card", card["speak"])
}

func TestAdvanced_Lang(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().WithLang("fr").Build()
	assert.Equal(t, "fr", card["lang"])
}

func TestAdvanced_RTL(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().WithRTL(true).Build()
	assert.Equal(t, true, card["rtl"])
}

func TestAdvanced_RTL_False(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().WithRTL(false).Build()
	assert.Equal(t, false, card["rtl"])
}

func TestAdvanced_MinHeight(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().WithMinHeight("500px").Build()
	assert.Equal(t, "500px", card["minHeight"])
}

// ── SelectAction ────────────────────────────────────────────────────────────

func TestAdvanced_CardSelectAction(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().
		WithSelectAction(func(a *ActionBuilder) {
			a.OpenURL("https://example.com").WithTitle("Open Site")
		}).
		Build()

	sa := card["selectAction"].(Card)
	assert.Equal(t, "Action.OpenUrl", sa["type"])
	assert.Equal(t, "https://example.com", sa["url"])
	assert.Equal(t, "Open Site", sa["title"])
}

func TestAdvanced_CardSelectAction_Execute(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().
		WithSelectAction(func(a *ActionBuilder) {
			a.Execute("Tap").WithVerb("onTap").WithData(map[string]any{"x": 1})
		}).
		Build()

	sa := card["selectAction"].(Card)
	assert.Equal(t, "Action.Execute", sa["type"])
	assert.Equal(t, "onTap", sa["verb"])
	data := sa["data"].(map[string]any)
	assert.Equal(t, 1, data["x"])
}

// ── VerticalContentAlignment ────────────────────────────────────────────────

func TestAdvanced_VerticalContentAlignment(t *testing.T) {
	t.Parallel()
	for _, align := range []VerticalAlignment{VerticalAlignmentTop, VerticalAlignmentCenter, VerticalAlignmentBottom} {
		card := NewAdaptiveCardBuilder().WithVerticalContentAlignment(align).Build()
		assert.Equal(t, string(align), card["verticalContentAlignment"])
	}
}

// ── BackgroundImage ─────────────────────────────────────────────────────────

func TestAdvanced_BackgroundImage_Full(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().
		WithBackgroundImage(func(bi *BackgroundImageBuilder) {
			bi.
				WithURL("https://example.com/bg.png").
				WithFillMode(BackgroundImageFillModeRepeat).
				WithHorizontalAlignment(HorizontalAlignmentCenter).
				WithVerticalAlignment(VerticalAlignmentBottom)
		}).
		Build()

	bg := card["backgroundImage"].(Card)
	assert.Equal(t, "https://example.com/bg.png", bg["url"])
	assert.Equal(t, string(BackgroundImageFillModeRepeat), bg["fillMode"])
	assert.Equal(t, string(HorizontalAlignmentCenter), bg["horizontalAlignment"])
	assert.Equal(t, string(VerticalAlignmentBottom), bg["verticalAlignment"])
}

// ── Combined advanced card ──────────────────────────────────────────────────

func TestAdvanced_FullCard(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().
		WithVersion("1.6").
		WithFallbackText("fallback").
		WithSpeak("speak").
		WithLang("en").
		WithRTL(false).
		WithMinHeight("200px").
		WithMetadata("https://example.com").
		WithAuthentication(func(auth *AuthenticationBuilder) {
			auth.WithText("Sign in").WithConnectionName("conn")
		}).
		WithRefresh(func(r *RefreshBuilder) {
			r.WithAction(func(a *ActionBuilder) {
				a.Execute("Refresh")
			}).AddUserID("u1")
		}).
		WithSelectAction(func(a *ActionBuilder) {
			a.OpenURL("https://example.com")
		}).
		AddTextBlock(func(tb *TextBlockBuilder) {
			tb.WithText("Title").WithSize(TextSizeExtraLarge)
		}).
		AddAction(func(a *ActionBuilder) {
			a.Submit("OK")
		}).
		Build()

	assert.Equal(t, "1.6", card["version"])
	assert.Equal(t, "fallback", card["fallbackText"])
	assert.Equal(t, "speak", card["speak"])
	assert.Equal(t, "en", card["lang"])
	assert.Equal(t, false, card["rtl"])
	assert.Equal(t, "200px", card["minHeight"])
	assert.NotNil(t, card["metadata"])
	assert.NotNil(t, card["authentication"])
	assert.NotNil(t, card["refresh"])
	assert.NotNil(t, card["selectAction"])

	body := card["body"].([]any)
	require.Len(t, body, 1)

	actions := card["actions"].([]any)
	require.Len(t, actions, 1)
}
