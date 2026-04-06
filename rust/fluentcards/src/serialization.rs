use serde_json::{Map, Value};

use crate::Card;

/// Recursively removes null values from JSON maps and arrays before serialization.
fn strip_null(v: Value) -> Value {
    match v {
        Value::Object(map) => {
            let mut out = Map::new();
            for (k, v2) in map {
                if v2.is_null() {
                    continue;
                }
                out.insert(k, strip_null(v2));
            }
            Value::Object(out)
        }
        Value::Array(arr) => {
            Value::Array(arr.into_iter().map(strip_null).collect())
        }
        other => other,
    }
}

/// Serializes an Adaptive Card to a JSON string with 2-space indentation.
/// Null/unset optional properties are omitted from the output.
pub fn to_json(card: &Card) -> Result<String, serde_json::Error> {
    to_json_indent(card, 2)
}

/// Serializes an Adaptive Card to a JSON string with the given indentation width.
/// Pass 0 for compact (no indentation) output.
pub fn to_json_indent(card: &Card, indent: usize) -> Result<String, serde_json::Error> {
    let clean = strip_null(Value::Object(card.clone()));
    if indent > 0 {
        let buf = Vec::new();
        let formatter = PrettyFormatter::with_indent(indent);
        let mut ser = serde_json::Serializer::with_formatter(buf, formatter);
        serde::Serialize::serialize(&clean, &mut ser)?;
        Ok(String::from_utf8(ser.into_inner()).unwrap())
    } else {
        serde_json::to_string(&clean)
    }
}

/// Parses a JSON string and returns the Adaptive Card if the root object
/// has type "AdaptiveCard". Returns `None` if parsing fails or the root type is wrong.
pub fn from_json(json_str: &str) -> Option<Card> {
    let parsed: Value = serde_json::from_str(json_str).ok()?;
    let obj = parsed.as_object()?;
    let type_val = obj.get("type")?.as_str()?;
    if type_val != "AdaptiveCard" {
        return None;
    }
    Some(obj.clone())
}

/// Custom pretty formatter that uses a configurable indent width.
struct PrettyFormatter {
    indent: Vec<u8>,
    current_indent: usize,
    has_value: bool,
}

impl PrettyFormatter {
    fn with_indent(width: usize) -> Self {
        PrettyFormatter {
            indent: " ".repeat(width).into_bytes(),
            current_indent: 0,
            has_value: false,
        }
    }
}

impl serde_json::ser::Formatter for PrettyFormatter {
    fn begin_array<W: ?Sized + std::io::Write>(&mut self, writer: &mut W) -> std::io::Result<()> {
        self.current_indent += 1;
        self.has_value = false;
        writer.write_all(b"[")
    }

    fn end_array<W: ?Sized + std::io::Write>(&mut self, writer: &mut W) -> std::io::Result<()> {
        self.current_indent -= 1;
        if self.has_value {
            writer.write_all(b"\n")?;
            write_indent(writer, &self.indent, self.current_indent)?;
        }
        writer.write_all(b"]")
    }

    fn begin_array_value<W: ?Sized + std::io::Write>(
        &mut self,
        writer: &mut W,
        first: bool,
    ) -> std::io::Result<()> {
        if !first {
            writer.write_all(b",")?;
        }
        writer.write_all(b"\n")?;
        write_indent(writer, &self.indent, self.current_indent)
    }

    fn end_array_value<W: ?Sized + std::io::Write>(
        &mut self,
        _writer: &mut W,
    ) -> std::io::Result<()> {
        self.has_value = true;
        Ok(())
    }

    fn begin_object<W: ?Sized + std::io::Write>(
        &mut self,
        writer: &mut W,
    ) -> std::io::Result<()> {
        self.current_indent += 1;
        self.has_value = false;
        writer.write_all(b"{")
    }

    fn end_object<W: ?Sized + std::io::Write>(&mut self, writer: &mut W) -> std::io::Result<()> {
        self.current_indent -= 1;
        if self.has_value {
            writer.write_all(b"\n")?;
            write_indent(writer, &self.indent, self.current_indent)?;
        }
        writer.write_all(b"}")
    }

    fn begin_object_key<W: ?Sized + std::io::Write>(
        &mut self,
        writer: &mut W,
        first: bool,
    ) -> std::io::Result<()> {
        if !first {
            writer.write_all(b",")?;
        }
        writer.write_all(b"\n")?;
        write_indent(writer, &self.indent, self.current_indent)
    }

    fn begin_object_value<W: ?Sized + std::io::Write>(
        &mut self,
        writer: &mut W,
    ) -> std::io::Result<()> {
        writer.write_all(b": ")
    }

    fn end_object_value<W: ?Sized + std::io::Write>(
        &mut self,
        _writer: &mut W,
    ) -> std::io::Result<()> {
        self.has_value = true;
        Ok(())
    }
}

fn write_indent<W: ?Sized + std::io::Write>(
    writer: &mut W,
    indent: &[u8],
    count: usize,
) -> std::io::Result<()> {
    for _ in 0..count {
        writer.write_all(indent)?;
    }
    Ok(())
}

#[cfg(test)]
mod tests {
    use super::*;
    use crate::AdaptiveCardBuilder;

    #[test]
    fn to_json_basic_card() {
        let card = AdaptiveCardBuilder::new()
            .add_text_block(|tb| { tb.with_text("Hello"); })
            .build();
        let json = to_json(&card).unwrap();
        assert!(json.contains("\"type\": \"AdaptiveCard\""));
        assert!(json.contains("\"Hello\""));
    }

    #[test]
    fn to_json_omits_unset_optional_properties() {
        let card = AdaptiveCardBuilder::new()
            .add_text_block(|tb| { tb.with_text("Test"); })
            .build();
        let json = to_json(&card).unwrap();
        assert!(!json.contains("\"size\""));
        assert!(!json.contains("\"weight\""));
        assert!(!json.contains("\"color\""));
        assert!(!json.contains("\"wrap\""));
    }

    #[test]
    fn to_json_enum_values_are_camel_case() {
        let card = AdaptiveCardBuilder::new()
            .add_text_block(|tb| {
                tb.with_text("x")
                  .with_size(crate::TextSize::ExtraLarge)
                  .with_color(crate::TextColor::Attention);
            })
            .build();
        let json = to_json(&card).unwrap();
        assert!(json.contains("\"extraLarge\""));
        assert!(json.contains("\"attention\""));
    }

    #[test]
    fn to_json_indent_compact() {
        let card = AdaptiveCardBuilder::new()
            .add_text_block(|tb| { tb.with_text("Test"); })
            .build();
        let json = to_json_indent(&card, 0).unwrap();
        assert!(!json.contains('\n'));
    }

    #[test]
    fn to_json_indent_two_spaces() {
        let card = AdaptiveCardBuilder::new()
            .add_text_block(|tb| { tb.with_text("Test"); })
            .build();
        let json = to_json_indent(&card, 2).unwrap();
        assert!(json.contains('\n'));
        assert!(json.contains("  "));
    }

    #[test]
    fn from_json_valid_card() {
        let raw = r#"{"type":"AdaptiveCard","version":"1.5","$schema":"https://example.com"}"#;
        let card = from_json(raw).unwrap();
        assert_eq!(card["type"].as_str().unwrap(), "AdaptiveCard");
        assert_eq!(card["version"].as_str().unwrap(), "1.5");
    }

    #[test]
    fn from_json_invalid_json() {
        assert!(from_json("not json").is_none());
    }

    #[test]
    fn from_json_wrong_root_type() {
        let raw = r#"{"type":"TextBlock","text":"oops"}"#;
        assert!(from_json(raw).is_none());
    }

    #[test]
    fn round_trip() {
        let original = AdaptiveCardBuilder::new()
            .with_version("1.5")
            .add_text_block(|tb| {
                tb.with_text("Round trip").with_size(crate::TextSize::Large);
            })
            .add_action(|a| {
                a.submit(Some("OK")).with_style(crate::ActionStyle::Positive);
            })
            .build();
        let json = to_json(&original).unwrap();
        let parsed = from_json(&json).unwrap();
        assert_eq!(parsed["version"].as_str().unwrap(), "1.5");
        let body = parsed["body"].as_array().unwrap();
        assert_eq!(body.len(), 1);
    }
}
