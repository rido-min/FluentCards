use serde_json::{Map, Value};
use std::collections::HashMap;

/// Card is the top-level representation of an Adaptive Card.
/// It is a plain JSON map that serializes directly to JSON via `to_json`.
pub type Card = Map<String, Value>;

/// Maps Adaptive Cards version strings to their official JSON schema URLs.
pub fn schema_urls() -> HashMap<&'static str, &'static str> {
    HashMap::from([
        ("1.0", "https://adaptivecards.io/schemas/1.0.0/adaptive-card.json"),
        ("1.1", "https://adaptivecards.io/schemas/1.1.0/adaptive-card.json"),
        ("1.2", "https://adaptivecards.io/schemas/1.2.0/adaptive-card.json"),
        ("1.3", "https://adaptivecards.io/schemas/1.3.0/adaptive-card.json"),
        ("1.4", "https://adaptivecards.io/schemas/1.4.0/adaptive-card.json"),
        ("1.5", "https://adaptivecards.io/schemas/1.5.0/adaptive-card.json"),
        ("1.6", "https://adaptivecards.io/schemas/1.6.0/adaptive-card.json"),
    ])
}

/// Returns the schema URL for a given version, or a fallback URL.
pub fn schema_url_for(version: &str) -> &'static str {
    match version {
        "1.0" => "https://adaptivecards.io/schemas/1.0.0/adaptive-card.json",
        "1.1" => "https://adaptivecards.io/schemas/1.1.0/adaptive-card.json",
        "1.2" => "https://adaptivecards.io/schemas/1.2.0/adaptive-card.json",
        "1.3" => "https://adaptivecards.io/schemas/1.3.0/adaptive-card.json",
        "1.4" => "https://adaptivecards.io/schemas/1.4.0/adaptive-card.json",
        "1.5" => "https://adaptivecards.io/schemas/1.5.0/adaptive-card.json",
        "1.6" => "https://adaptivecards.io/schemas/1.6.0/adaptive-card.json",
        _ => "http://adaptivecards.io/schemas/adaptive-card.json",
    }
}

/// Returns true if the version string is a known Adaptive Cards version.
pub fn is_known_version(version: &str) -> bool {
    matches!(version, "1.0" | "1.1" | "1.2" | "1.3" | "1.4" | "1.5" | "1.6")
}
