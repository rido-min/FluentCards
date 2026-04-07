import XCTest
@testable import FluentCards

final class ElementsTests: XCTestCase {
    func testTextBlockAllProperties() {
        let card = AdaptiveCardBuilder()
            .addTextBlock { tb in
                tb.withText("Hello")
                    .withSize(.large)
                    .withWeight(.bolder)
                    .withColor(.accent)
                    .withWrap(true)
                    .withMaxLines(3)
                    .withIsSubtle(false)
                    .withHorizontalAlignment(.center)
                    .withFontType(.monospace)
                    .withStyle(.heading)
                    .withSpacing(.medium)
                    .withSeparator(true)
                    .withID("tb1")
            }
            .build()
        let el = (card["body"] as! [Any])[0] as! [String: Any]
        XCTAssertEqual(el["type"] as? String, "TextBlock")
        XCTAssertEqual(el["text"] as? String, "Hello")
        XCTAssertEqual(el["size"] as? String, "large")
        XCTAssertEqual(el["weight"] as? String, "bolder")
        XCTAssertEqual(el["color"] as? String, "accent")
        XCTAssertEqual(el["wrap"] as? Bool, true)
        XCTAssertEqual(el["maxLines"] as? Int, 3)
        XCTAssertEqual(el["horizontalAlignment"] as? String, "center")
        XCTAssertEqual(el["fontType"] as? String, "monospace")
        XCTAssertEqual(el["style"] as? String, "heading")
        XCTAssertEqual(el["spacing"] as? String, "medium")
        XCTAssertEqual(el["separator"] as? Bool, true)
        XCTAssertEqual(el["id"] as? String, "tb1")
    }

    func testImageBuilderProperties() {
        let card = AdaptiveCardBuilder()
            .addImage { img in
                img.withURL("https://example.com/img.png")
                    .withAltText("An image")
                    .withSize(.medium)
                    .withStyle(.person)
                    .withWidth("100px")
                    .withHeight("100px")
                    .withBackgroundColor("#FFFFFF")
            }
            .build()
        let el = (card["body"] as! [Any])[0] as! [String: Any]
        XCTAssertEqual(el["type"] as? String, "Image")
        XCTAssertEqual(el["url"] as? String, "https://example.com/img.png")
        XCTAssertEqual(el["altText"] as? String, "An image")
        XCTAssertEqual(el["size"] as? String, "medium")
        XCTAssertEqual(el["style"] as? String, "person")
        XCTAssertEqual(el["width"] as? String, "100px")
        XCTAssertEqual(el["backgroundColor"] as? String, "#FFFFFF")
    }

    func testContainerBuilderWithItems() {
        let card = AdaptiveCardBuilder()
            .addContainer { c in
                c.withStyle(.emphasis)
                    .withBleed(true)
                    .addTextBlock { tb in tb.withText("Inside container") }
                    .addImage { img in img.withURL("https://example.com/x.png") }
            }
            .build()
        let el = (card["body"] as! [Any])[0] as! [String: Any]
        XCTAssertEqual(el["type"] as? String, "Container")
        XCTAssertEqual(el["style"] as? String, "emphasis")
        XCTAssertEqual(el["bleed"] as? Bool, true)
        let items = el["items"] as! [Any]
        XCTAssertEqual(items.count, 2)
    }

    func testColumnSetBuilderWithColumns() {
        let card = AdaptiveCardBuilder()
            .addColumnSet { cs in
                cs.addColumnWithWidth("auto") { col in
                    col.addTextBlock { tb in tb.withText("Left") }
                }
                .addColumnWithWidth("stretch") { col in
                    col.withVerticalContentAlignment(.center)
                        .addTextBlock { tb in tb.withText("Right") }
                }
            }
            .build()
        let el = (card["body"] as! [Any])[0] as! [String: Any]
        XCTAssertEqual(el["type"] as? String, "ColumnSet")
        let cols = el["columns"] as! [Any]
        XCTAssertEqual(cols.count, 2)
        XCTAssertEqual((cols[0] as! [String: Any])["width"] as? String, "auto")
        XCTAssertEqual((cols[1] as! [String: Any])["width"] as? String, "stretch")
    }

    func testFactSetBuilderAddFact() {
        let card = AdaptiveCardBuilder()
            .addFactSet { fs in
                fs.addFact("Name", "Alice")
                    .addFact("Role", "Engineer")
            }
            .build()
        let el = (card["body"] as! [Any])[0] as! [String: Any]
        let facts = el["facts"] as! [Any]
        XCTAssertEqual(facts.count, 2)
        let fact0 = facts[0] as! [String: Any]
        XCTAssertEqual(fact0["title"] as? String, "Name")
        XCTAssertEqual(fact0["value"] as? String, "Alice")
    }

    func testRichTextBlockInlines() {
        let card = AdaptiveCardBuilder()
            .addRichTextBlock { rtb in
                rtb.addText("plain text")
                    .addTextRun { tr in
                        tr.withText("bold").withWeight(.bolder).withItalic(true)
                    }
            }
            .build()
        let el = (card["body"] as! [Any])[0] as! [String: Any]
        XCTAssertEqual(el["type"] as? String, "RichTextBlock")
        let inlines = el["inlines"] as! [Any]
        XCTAssertEqual(inlines.count, 2)
        XCTAssertEqual(inlines[0] as? String, "plain text")
        let run = inlines[1] as! [String: Any]
        XCTAssertEqual(run["type"] as? String, "TextRun")
        XCTAssertEqual(run["text"] as? String, "bold")
        XCTAssertEqual(run["weight"] as? String, "bolder")
        XCTAssertEqual(run["italic"] as? Bool, true)
    }

    func testActionSetBuilderWithActions() {
        let card = AdaptiveCardBuilder()
            .addActionSet { asb in
                asb.addAction { a in a.submit("OK") }
                    .addAction { a in a.openURL("https://example.com") }
            }
            .build()
        let el = (card["body"] as! [Any])[0] as! [String: Any]
        XCTAssertEqual(el["type"] as? String, "ActionSet")
        let actions = el["actions"] as! [Any]
        XCTAssertEqual(actions.count, 2)
    }

    func testMediaBuilderWithSources() {
        let card = AdaptiveCardBuilder()
            .addMedia { m in
                m.withPoster("https://example.com/poster.png")
                    .addSource("https://example.com/video.mp4", "video/mp4")
            }
            .build()
        let el = (card["body"] as! [Any])[0] as! [String: Any]
        XCTAssertEqual(el["type"] as? String, "Media")
        let sources = el["sources"] as! [Any]
        XCTAssertEqual(sources.count, 1)
        let s = sources[0] as! [String: Any]
        XCTAssertEqual(s["url"] as? String, "https://example.com/video.mp4")
        XCTAssertEqual(s["mimeType"] as? String, "video/mp4")
    }

    func testImageSetBuilderAddImages() {
        let card = AdaptiveCardBuilder()
            .addImageSet { isb in
                isb.withImageSize(.medium)
                    .addImage { img in img.withURL("https://example.com/1.png") }
                    .addImage { img in img.withURL("https://example.com/2.png") }
            }
            .build()
        let el = (card["body"] as! [Any])[0] as! [String: Any]
        XCTAssertEqual(el["type"] as? String, "ImageSet")
        XCTAssertEqual(el["imageSize"] as? String, "medium")
        let images = el["images"] as! [Any]
        XCTAssertEqual(images.count, 2)
    }

    func testTableBuilderAddColumnsAndRows() {
        let card = AdaptiveCardBuilder()
            .addTable { tb in
                tb.withFirstRowAsHeader(true)
                    .withShowGridLines(true)
                    .addColumn(["width": 1])
                    .addColumn(["width": 2])
                    .addRow([
                        "cells": [
                            ["items": [["type": "TextBlock", "text": "H1"]]],
                            ["items": [["type": "TextBlock", "text": "H2"]]],
                        ]
                    ])
            }
            .build()
        let el = (card["body"] as! [Any])[0] as! [String: Any]
        XCTAssertEqual(el["type"] as? String, "Table")
        XCTAssertEqual(el["firstRowAsHeader"] as? Bool, true)
        let cols = el["columns"] as! [Any]
        XCTAssertEqual(cols.count, 2)
        let rows = el["rows"] as! [Any]
        XCTAssertEqual(rows.count, 1)
    }
}
