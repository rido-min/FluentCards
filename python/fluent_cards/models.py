"""
Type documentation for Adaptive Card structures.

These TypedDicts serve as documentation only. At runtime, plain Python dicts
are used throughout the library (mirrors TypeScript's structural typing).
"""
from __future__ import annotations
from typing import TypedDict, Optional, Union, List, Any


class Fact(TypedDict, total=False):
    title: str
    value: str


class Choice(TypedDict, total=False):
    title: str
    value: str


class MediaSource(TypedDict, total=False):
    url: str
    mimeType: str


class BackgroundImage(TypedDict, total=False):
    url: str
    fillMode: str
    horizontalAlignment: str
    verticalAlignment: str


class TokenExchangeResource(TypedDict, total=False):
    id: str
    uri: str
    providerId: str


class AuthCardButton(TypedDict, total=False):
    type: str
    title: str
    value: str
    image: str


class AuthenticationConfiguration(TypedDict, total=False):
    text: str
    connectionName: str
    tokenExchangeResource: TokenExchangeResource
    buttons: List[AuthCardButton]


class RefreshConfiguration(TypedDict, total=False):
    action: Any
    userIds: List[str]
    expires: str


class CardMetadata(TypedDict, total=False):
    webUrl: str


class TableColumnDefinition(TypedDict, total=False):
    width: Any
    horizontalCellContentAlignment: str
    verticalCellContentAlignment: str


class TableCell(TypedDict, total=False):
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
    type: str  # 'TableRow'
    cells: List[TableCell]
    style: str


class AdaptiveCard(TypedDict, total=False):
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
