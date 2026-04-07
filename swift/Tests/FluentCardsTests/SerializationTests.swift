import XCTest
@testable import FluentCards

final class SerializationTests: XCTestCase {
    func testToJSONBasicCard() throws {
        let card = AdaptiveCardBuilder()
            .addTextBlock { tb in tb.withText("Hello") }
            .build()
        let json = try toJSON(card)
        XCTAssertTrue(json.contains("\"type\""))
        XCTAssertTrue(json.contains("AdaptiveCard"))
        XCTAssertTrue(json.contains("Hello"))
    }

    func testToJSONOmitsUnsetOptionalProperties() throws {
        let card = AdaptiveCardBuilder()
            .addTextBlock { tb in tb.withText("Test") }
            .build()
        let json = try toJSON(card)
        XCTAssertFalse(json.contains("\"size\""))
        XCTAssertFalse(json.contains("\"weight\""))
        XCTAssertFalse(json.contains("\"color\""))
        XCTAssertFalse(json.contains("\"wrap\""))
    }

    func testToJSONEnumValuesAreCamelCase() throws {
        let card = AdaptiveCardBuilder()
            .addTextBlock { tb in
                tb.withText("x")
                    .withSize(.extraLarge)
                    .withColor(.attention)
            }
            .build()
        let json = try toJSON(card)
        XCTAssertTrue(json.contains("extraLarge"))
        XCTAssertTrue(json.contains("attention"))
    }

    func testToJSONIndentCompact() throws {
        let card = AdaptiveCardBuilder()
            .addTextBlock { tb in tb.withText("Test") }
            .build()
        let json = try toJSON(card, indent: 0)
        XCTAssertFalse(json.contains("\n"))
    }

    func testToJSONIndentTwoSpaces() throws {
        let card = AdaptiveCardBuilder()
            .addTextBlock { tb in tb.withText("Test") }
            .build()
        let json = try toJSON(card, indent: 2)
        XCTAssertTrue(json.contains("\n"))
        XCTAssertTrue(json.contains("  "))
    }

    func testFromJSONValidCard() {
        let raw = "{\"type\":\"AdaptiveCard\",\"version\":\"1.5\",\"$schema\":\"https://example.com\"}"
        let card = fromJSON(raw)
        XCTAssertNotNil(card)
        XCTAssertEqual(card?["type"] as? String, "AdaptiveCard")
        XCTAssertEqual(card?["version"] as? String, "1.5")
    }

    func testFromJSONInvalidJSON() {
        let card = fromJSON("not json")
        XCTAssertNil(card)
    }

    func testFromJSONWrongRootType() {
        let card = fromJSON("{\"type\":\"TextBlock\",\"text\":\"oops\"}")
        XCTAssertNil(card)
    }

    func testToJSONRoundTrip() throws {
        let original = AdaptiveCardBuilder()
            .withVersion("1.5")
            .addTextBlock { tb in
                tb.withText("Round trip").withSize(.large)
            }
            .addAction { a in
                a.submit("OK").withStyle(.positive)
            }
            .build()
        let json = try toJSON(original)
        let parsed = fromJSON(json)
        XCTAssertNotNil(parsed)
        XCTAssertEqual(parsed?["version"] as? String, "1.5")
        let body = parsed?["body"] as! [Any]
        XCTAssertEqual(body.count, 1)
    }
}
