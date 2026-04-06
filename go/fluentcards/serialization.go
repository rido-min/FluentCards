package fluentcards

import (
	"encoding/json"
	"strings"
)

// stripNil recursively removes nil values from maps and slices before JSON marshaling.
// This mirrors the _strip_none behavior in the Python SDK.
func stripNil(v any) any {
	switch val := v.(type) {
	case map[string]any:
		out := make(map[string]any, len(val))
		for k, v2 := range val {
			if v2 == nil {
				continue
			}
			out[k] = stripNil(v2)
		}
		return out
	case []any:
		out := make([]any, len(val))
		for i, v2 := range val {
			out[i] = stripNil(v2)
		}
		return out
	default:
		return v
	}
}

// ToJSON serializes an Adaptive Card to a JSON string with 2-space indentation.
// Nil/unset optional properties are omitted from the output.
func ToJSON(card Card) (string, error) {
	return ToJSONIndent(card, 2)
}

// ToJSONIndent serializes an Adaptive Card to a JSON string with the given indentation width.
// Pass 0 for compact (no indentation) output.
func ToJSONIndent(card Card, indent int) (string, error) {
	clean := stripNil(card)
	if indent > 0 {
		b, err := json.MarshalIndent(clean, "", strings.Repeat(" ", indent))
		return string(b), err
	}
	b, err := json.Marshal(clean)
	return string(b), err
}

// FromJSON parses a JSON string and returns the Adaptive Card if the root object
// has type "AdaptiveCard". Returns nil if parsing fails or the root type is wrong.
func FromJSON(jsonStr string) Card {
	var parsed map[string]any
	if err := json.Unmarshal([]byte(jsonStr), &parsed); err != nil {
		return nil
	}
	if t, ok := parsed["type"].(string); !ok || t != "AdaptiveCard" {
		return nil
	}
	return parsed
}
