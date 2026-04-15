import XCTest
@testable import FluentCards

final class AdaptiveCardBuilderTests: XCTestCase {
    func testDefaultVersionAndSchema() {
        let card = AdaptiveCardBuilder().build()
        XCTAssertEqual(card["type"] as? String, "AdaptiveCard")
        XCTAssertEqual(card["version"] as? String, "1.5")
        XCTAssertNotNil(card["$schema"])
    }

    func testWithVersion() {
        let card = AdaptiveCardBuilder().withVersion("1.6").build()
        XCTAssertEqual(card["version"] as? String, "1.6")
        let schema = card["$schema"] as? String ?? ""
        XCTAssertTrue(schema.contains("1.6"))
    }

    func testWithSchemaOverride() {
        let card = AdaptiveCardBuilder()
            .withSchema("https://example.com/custom-schema.json")
            .build()
        XCTAssertEqual(card["$schema"] as? String, "https://example.com/custom-schema.json")
    }

    func testAddTextBlock() {
        let card = AdaptiveCardBuilder()
            .addTextBlock { tb in
                tb.withText("Hello, World!")
            }
            .build()
        let body = card["body"] as! [Any]
        XCTAssertEqual(body.count, 1)
        let el = body[0] as! [String: Any]
        XCTAssertEqual(el["type"] as? String, "TextBlock")
        XCTAssertEqual(el["text"] as? String, "Hello, World!")
    }

    func testAddAction() {
        let card = AdaptiveCardBuilder()
            .addTextBlock { tb in tb.withText("x") }
            .addAction { a in
                a.submit("Click me")
            }
            .build()
        let actions = card["actions"] as! [Any]
        XCTAssertEqual(actions.count, 1)
        let action = actions[0] as! [String: Any]
        XCTAssertEqual(action["type"] as? String, "Action.Submit")
        XCTAssertEqual(action["title"] as? String, "Click me")
    }

    func testMultipleBodyElements() {
        let card = AdaptiveCardBuilder()
            .addTextBlock { tb in tb.withText("First") }
            .addTextBlock { tb in tb.withText("Second") }
            .addImage { img in img.withURL("https://example.com/img.png") }
            .build()
        let body = card["body"] as! [Any]
        XCTAssertEqual(body.count, 3)
    }

    func testWithMetadata() {
        let card = AdaptiveCardBuilder()
            .withMetadata("https://example.com/card")
            .build()
        let meta = card["metadata"] as! [String: Any]
        XCTAssertEqual(meta["webUrl"] as? String, "https://example.com/card")
    }

    func testWithRefresh() {
        let card = AdaptiveCardBuilder()
            .withRefresh { r in
                r.addUserID("user1").withExpires("2026-01-01T00:00:00Z")
            }
            .build()
        let refresh = card["refresh"] as! [String: Any]
        XCTAssertEqual(refresh["expires"] as? String, "2026-01-01T00:00:00Z")
        let userIds = refresh["userIds"] as! [Any]
        XCTAssertEqual(userIds[0] as? String, "user1")
    }

    func testAddElementPreBuilt() {
        let prebuilt: Card = ["type": "TextBlock", "text": "Pre-built"]
        let card = AdaptiveCardBuilder()
            .addElement(prebuilt)
            .build()
        let body = card["body"] as! [Any]
        XCTAssertEqual(body.count, 1)
        let el = body[0] as! [String: Any]
        XCTAssertEqual(el["text"] as? String, "Pre-built")
    }
}
