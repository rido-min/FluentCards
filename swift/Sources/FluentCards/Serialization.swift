import Foundation

/// Recursively removes NSNull values from maps and slices before JSON serialization.
func stripNilValues(_ value: Any) -> Any? {
    if value is NSNull { return nil }
    if let dict = value as? [String: Any] {
        var result: [String: Any] = [:]
        for (k, v) in dict {
            if let stripped = stripNilValues(v) {
                result[k] = stripped
            }
        }
        return result
    }
    if let arr = value as? [Any] {
        return arr.compactMap { stripNilValues($0) }
    }
    return value
}

/// Serializes an Adaptive Card to a JSON string with 2-space indentation.
public func toJSON(_ card: Card) throws -> String {
    return try toJSON(card, indent: 2)
}

/// Serializes an Adaptive Card to a JSON string with custom indentation.
/// Pass 0 for compact (no indentation) output.
public func toJSON(_ card: Card, indent: Int) throws -> String {
    let clean = stripNilValues(card) ?? [:]
    var options: JSONSerialization.WritingOptions = [.sortedKeys]
    if indent > 0 {
        options.insert(.prettyPrinted)
    }
    let data = try JSONSerialization.data(withJSONObject: clean, options: options)
    return String(data: data, encoding: .utf8) ?? ""
}

/// Parses a JSON string and returns the Adaptive Card if the root object
/// has type "AdaptiveCard". Returns nil if parsing fails or the root type is wrong.
public func fromJSON(_ json: String) -> Card? {
    guard let data = json.data(using: .utf8),
          let obj = try? JSONSerialization.jsonObject(with: data),
          let dict = obj as? [String: Any],
          dict["type"] as? String == "AdaptiveCard" else {
        return nil
    }
    return dict
}
