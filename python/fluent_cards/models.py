"""
Type documentation for Adaptive Card structures.

These TypedDicts serve as documentation only. At runtime, plain Python dicts
are used throughout the library (mirrors TypeScript's structural typing).
"""
from __future__ import annotations
from typing import TypedDict, Optional, Union, List, Any


class Fact(TypedDict, total=False):
    """Represents a key-value fact displayed in a FactSet."""

    title: str
    value: str


class Choice(TypedDict, total=False):
    """Represents a selectable choice in an Input.ChoiceSet."""

    title: str
    value: str


class MediaSource(TypedDict, total=False):
    """Represents a single media source with a URL and MIME type."""

    url: str
    mimeType: str


class BackgroundImage(TypedDict, total=False):
    """Represents a background image with fill mode and alignment options."""

    url: str
    fillMode: str
    horizontalAlignment: str
    verticalAlignment: str


class TokenExchangeResource(TypedDict, total=False):
    """Identifies a resource for OAuth token exchange in authentication."""

    id: str
    uri: str
    providerId: str


class AuthCardButton(TypedDict, total=False):
    """Represents a button displayed in an authentication card."""

    type: str
    title: str
    value: str
    image: str


class AuthenticationConfiguration(TypedDict, total=False):
    """Defines the authentication configuration for an Adaptive Card."""

    text: str
    connectionName: str
    tokenExchangeResource: TokenExchangeResource
    buttons: List[AuthCardButton]


class RefreshConfiguration(TypedDict, total=False):
    """Defines the refresh configuration for an Adaptive Card."""

    action: Any
    userIds: List[str]
    expires: str


class CardMetadata(TypedDict, total=False):
    """Contains metadata for an Adaptive Card, such as a web URL."""

    webUrl: str


class TableColumnDefinition(TypedDict, total=False):
    """Defines the width and alignment properties for a table column."""

    width: Any
    horizontalCellContentAlignment: str
    verticalCellContentAlignment: str


class TableCell(TypedDict, total=False):
    """Represents a single cell within a TableRow."""

    type: str  # 'TableCell'
    items: List[Any]
    selectAction: Any
    style: str
    bleed: bool
    backgroundImage: Any
    minHeight: str
    rtl: bool
    verticalContentAlignment: str


class TableRow(TypedDict, total=False):
    """Represents a row of cells within a Table element."""

    type: str  # 'TableRow'
    cells: List[TableCell]
    style: str


class AdaptiveCard(TypedDict, total=False):
    """Represents the top-level Adaptive Card document structure."""

    type: str  # 'AdaptiveCard'
    version: str
    body: List[Any]
    actions: List[Any]
    selectAction: Any
    fallbackText: str
    backgroundImage: Any
    minHeight: str
    rtl: bool
    speak: str
    lang: str
    verticalContentAlignment: str
    refresh: RefreshConfiguration
    authentication: AuthenticationConfiguration
    metadata: CardMetadata
