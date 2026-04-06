from __future__ import annotations
import json
from typing import Any, Optional


def _strip_none(obj: Any) -> Any:
    if isinstance(obj, dict):
        return {k: _strip_none(v) for k, v in obj.items() if v is not None}
    if isinstance(obj, list):
        return [_strip_none(item) for item in obj]
    return obj


def to_json(card: dict, indent: int = 2) -> str:
    """Serializes an Adaptive Card dictionary to a JSON string.

    Args:
        card: The Adaptive Card dictionary to serialize.
        indent: Number of spaces to use for indentation. Use 0 for compact output.

    Returns:
        A JSON string representation of the card with None values omitted.
    """
    actual_indent = indent if indent > 0 else None
    return json.dumps(_strip_none(card), indent=actual_indent,
                      separators=(",", ":") if actual_indent is None else None)


def from_json(json_str: str) -> Optional[dict]:
    """Deserializes a JSON string into an Adaptive Card dictionary.

    Args:
        json_str: The JSON string to parse.

    Returns:
        The parsed Adaptive Card dictionary, or None if the string is not a valid
        AdaptiveCard JSON object.
    """
    try:
        parsed = json.loads(json_str)
        if isinstance(parsed, dict) and parsed.get("type") == "AdaptiveCard":
            return parsed
        return None
    except Exception:
        return None
