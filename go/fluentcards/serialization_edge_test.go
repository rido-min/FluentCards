package fluentcards

import (
	"encoding/json"
	"strings"
	"testing"

	"github.com/stretchr/testify/assert"
	"github.com/stretchr/testify/require"
)

func TestSerializationEdge_MinimalCard_OmitsNilValues(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().Build()
	jsonStr, err := ToJSON(card)
	require.NoError(t, err)

	assert.NotContains(t, jsonStr, `"body"`)
	assert.NotContains(t, jsonStr, `"actions"`)
	assert.NotContains(t, jsonStr, `"fallbackText"`)
	assert.Contains(t, jsonStr, `"type"`)
	assert.Contains(t, jsonStr, `"version"`)
}

func TestSerializationEdge_RoundTrip(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddTextBlock(func(tb *TextBlockBuilder) {
			tb.WithText("Hello World").WithWrap(true)
		}).
		AddAction(func(a *ActionBuilder) {
			a.OpenURL("https://example.com").WithTitle("Go")
		}).
		Build()

	jsonStr, err := ToJSON(card)
	require.NoError(t, err)

	parsed := FromJSON(jsonStr)
	require.NotNil(t, parsed)

	assert.Equal(t, "AdaptiveCard", parsed["type"])
	assert.Equal(t, "1.5", parsed["version"])

	body := parsed["body"].([]any)
	require.Len(t, body, 1)
	tb := body[0].(map[string]any)
	assert.Equal(t, "Hello World", tb["text"])
	assert.Equal(t, true, tb["wrap"])

	actions := parsed["actions"].([]any)
	require.Len(t, actions, 1)
	act := actions[0].(map[string]any)
	assert.Equal(t, "Action.OpenUrl", act["type"])
}

func TestSerializationEdge_Unicode(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().
		AddTextBlock(func(tb *TextBlockBuilder) {
			tb.WithText("こんにちは世界 🌍 مرحبا")
		}).
		Build()

	jsonStr, err := ToJSON(card)
	require.NoError(t, err)

	parsed := FromJSON(jsonStr)
	require.NotNil(t, parsed)
	body := parsed["body"].([]any)
	tb := body[0].(map[string]any)
	assert.Equal(t, "こんにちは世界 🌍 مرحبا", tb["text"])
}

func TestSerializationEdge_VeryLongText(t *testing.T) {
	t.Parallel()
	longText := strings.Repeat("A", 10000)
	card := NewAdaptiveCardBuilder().
		AddTextBlock(func(tb *TextBlockBuilder) {
			tb.WithText(longText)
		}).
		Build()

	jsonStr, err := ToJSON(card)
	require.NoError(t, err)

	parsed := FromJSON(jsonStr)
	require.NotNil(t, parsed)
	body := parsed["body"].([]any)
	tb := body[0].(map[string]any)
	assert.Equal(t, longText, tb["text"])
}

func TestSerializationEdge_EmptyBody(t *testing.T) {
	t.Parallel()
	// Card with explicit empty body array
	card := Card{
		"type":    "AdaptiveCard",
		"version": "1.5",
		"body":    []any{},
	}

	jsonStr, err := ToJSON(card)
	require.NoError(t, err)
	assert.Contains(t, jsonStr, `"body"`)

	parsed := FromJSON(jsonStr)
	require.NotNil(t, parsed)
	body := parsed["body"].([]any)
	assert.Len(t, body, 0)
}

func TestSerializationEdge_NestedContainers(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().
		AddContainer(func(c1 *ContainerBuilder) {
			c1.WithID("level1").AddContainer(func(c2 *ContainerBuilder) {
				c2.WithID("level2").AddContainer(func(c3 *ContainerBuilder) {
					c3.WithID("level3").AddTextBlock(func(tb *TextBlockBuilder) {
						tb.WithText("deep")
					})
				})
			})
		}).
		Build()

	jsonStr, err := ToJSON(card)
	require.NoError(t, err)

	parsed := FromJSON(jsonStr)
	require.NotNil(t, parsed)

	body := parsed["body"].([]any)
	l1 := body[0].(map[string]any)
	assert.Equal(t, "level1", l1["id"])

	l2 := l1["items"].([]any)[0].(map[string]any)
	assert.Equal(t, "level2", l2["id"])

	l3 := l2["items"].([]any)[0].(map[string]any)
	assert.Equal(t, "level3", l3["id"])

	innerItems := l3["items"].([]any)
	assert.Equal(t, "deep", innerItems[0].(map[string]any)["text"])
}

func TestSerializationEdge_FromJSON_InvalidJSON(t *testing.T) {
	t.Parallel()
	result := FromJSON("{invalid json")
	assert.Nil(t, result)
}

func TestSerializationEdge_FromJSON_NotAdaptiveCard(t *testing.T) {
	t.Parallel()
	result := FromJSON(`{"type": "SomethingElse", "version": "1.0"}`)
	assert.Nil(t, result)
}

func TestSerializationEdge_FromJSON_MissingType(t *testing.T) {
	t.Parallel()
	result := FromJSON(`{"version": "1.0"}`)
	assert.Nil(t, result)
}

func TestSerializationEdge_CompactSerialization(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().
		AddTextBlock(func(tb *TextBlockBuilder) {
			tb.WithText("Hi")
		}).
		Build()

	compact, err := ToJSONIndent(card, 0)
	require.NoError(t, err)
	assert.NotContains(t, compact, "\n")

	indented, err := ToJSONIndent(card, 4)
	require.NoError(t, err)
	assert.Contains(t, indented, "\n")
	assert.Contains(t, indented, "    ") // 4-space indent
}

func TestSerializationEdge_IndentedVsCompact_SameData(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().
		AddTextBlock(func(tb *TextBlockBuilder) {
			tb.WithText("Test")
		}).
		Build()

	compact, err := ToJSONIndent(card, 0)
	require.NoError(t, err)

	indented, err := ToJSONIndent(card, 2)
	require.NoError(t, err)

	var compactMap, indentedMap map[string]any
	require.NoError(t, json.Unmarshal([]byte(compact), &compactMap))
	require.NoError(t, json.Unmarshal([]byte(indented), &indentedMap))

	// Same keys/values regardless of formatting
	assert.Equal(t, compactMap["type"], indentedMap["type"])
	assert.Equal(t, compactMap["version"], indentedMap["version"])
}

func TestSerializationEdge_NilValuesStripped(t *testing.T) {
	t.Parallel()
	card := Card{
		"type":    "AdaptiveCard",
		"version": "1.5",
		"body":    nil,
		"actions": nil,
		"speak":   nil,
	}
	jsonStr, err := ToJSON(card)
	require.NoError(t, err)
	assert.NotContains(t, jsonStr, `"body"`)
	assert.NotContains(t, jsonStr, `"actions"`)
	assert.NotContains(t, jsonStr, `"speak"`)
}
