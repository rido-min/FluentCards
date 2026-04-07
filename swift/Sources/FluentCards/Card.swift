import Foundation

/// Card is the top-level representation of an Adaptive Card as a plain dictionary.
public typealias Card = [String: Any]

let schemaURLs: [String: String] = [
    "1.0": "https://adaptivecards.io/schemas/1.0.0/adaptive-card.json",
    "1.1": "https://adaptivecards.io/schemas/1.1.0/adaptive-card.json",
    "1.2": "https://adaptivecards.io/schemas/1.2.0/adaptive-card.json",
    "1.3": "https://adaptivecards.io/schemas/1.3.0/adaptive-card.json",
    "1.4": "https://adaptivecards.io/schemas/1.4.0/adaptive-card.json",
    "1.5": "https://adaptivecards.io/schemas/1.5.0/adaptive-card.json",
    "1.6": "https://adaptivecards.io/schemas/1.6.0/adaptive-card.json",
]

let knownVersions: Set<String> = ["1.0", "1.1", "1.2", "1.3", "1.4", "1.5", "1.6"]
