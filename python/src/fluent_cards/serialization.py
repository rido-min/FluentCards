from __future__ import annotations
import json
from enum import Enum
from typing import Any, Optional


def _strip_none(obj: Any) -> Any:
    if isinstance(obj, dict):
        return {k: _strip_none(v) for k, v in obj.items() if v is not None}
    if isinstance(obj, list):
        return [_strip_none(item) for item in obj]
    return obj


def _clean(obj: Any) -> Any:
    """Recursively strips None values and converts enums to plain strings."""
    if isinstance(obj, dict):
        return {k: _clean(v) for k, v in obj.items() if v is not None}
    if isinstance(obj, list):
        return [_clean(item) for item in obj]
    if isinstance(obj, Enum):
        return obj.value
    return obj


def to_dict(card: dict) -> dict:
    """Returns a clean dictionary representation of an Adaptive Card.

    Applies the same cleanup as ``to_json`` (removing None values, converting
    enums to plain strings) but returns a ``dict`` instead of a JSON string.
    Useful for embedding cards directly in API response objects without
    double-serialization.

    Args:
        card: The Adaptive Card dictionary to clean.

    Returns:
        A cleaned dictionary with None values removed and enums resolved to
        their string values.
    """
    return _clean(card)


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
    except (json.JSONDecodeError, ValueError):
        return None
