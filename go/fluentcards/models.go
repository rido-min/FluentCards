package fluentcards

// Card is the top-level representation of an Adaptive Card.
// It is a plain map that serialises directly to JSON via ToJSON.
type Card = map[string]any

// schemaURLs maps Adaptive Cards version strings to their official JSON schema URLs.
var schemaURLs = map[string]string{
	"1.0": "https://adaptivecards.io/schemas/1.0.0/adaptive-card.json",
	"1.1": "https://adaptivecards.io/schemas/1.1.0/adaptive-card.json",
	"1.2": "https://adaptivecards.io/schemas/1.2.0/adaptive-card.json",
	"1.3": "https://adaptivecards.io/schemas/1.3.0/adaptive-card.json",
	"1.4": "https://adaptivecards.io/schemas/1.4.0/adaptive-card.json",
	"1.5": "https://adaptivecards.io/schemas/1.5.0/adaptive-card.json",
	"1.6": "https://adaptivecards.io/schemas/1.6.0/adaptive-card.json",
}

// knownVersions is the set of officially supported Adaptive Cards version strings.
var knownVersions = map[string]bool{
	"1.0": true, "1.1": true, "1.2": true, "1.3": true,
	"1.4": true, "1.5": true, "1.6": true,
}
