import XCTest
@testable import FluentCards

final class ValidationTests: XCTestCase {
    func testValidateValidCard() {
        let card = AdaptiveCardBuilder()
            .withVersion("1.5")
            .addTextBlock { tb in tb.withText("Hello") }
            .build()
        let issues = validate(card)
        XCTAssertTrue(issues.isEmpty)
    }

    func testValidateMissingVersion() {
        let card: Card = ["type": "AdaptiveCard", "$schema": "https://x.com"]
        let issues = validate(card)
        XCTAssertFalse(issues.isEmpty)
        let found = issues.contains { $0.code == "MISSING_VERSION" && $0.severity == .error }
        XCTAssertTrue(found, "expected MISSING_VERSION issue")
    }

    func testValidateEmptyCard() {
        let card = AdaptiveCardBuilder().build()
        let issues = validate(card)
        let found = issues.contains { $0.code == "EMPTY_CARD" && $0.severity == .warning }
        XCTAssertTrue(found, "expected EMPTY_CARD warning")
    }

    func testValidateMissingTextBlockText() {
        let card = AdaptiveCardBuilder()
            .addTextBlock { _ in /* no text set */ }
            .build()
        let issues = validate(card)
        let found = issues.contains { $0.code == "MISSING_TEXT" && $0.severity == .error }
        XCTAssertTrue(found, "expected MISSING_TEXT issue")
    }

    func testValidateMissingImageURL() {
        let card = AdaptiveCardBuilder()
            .addImage { _ in /* no URL set */ }
            .build()
        let issues = validate(card)
        let found = issues.contains { $0.code == "MISSING_IMAGE_URL" }
        XCTAssertTrue(found)
    }

    func testValidateMissingInputID() {
        let card = AdaptiveCardBuilder()
            .addInputText { _ in /* no ID set */ }
            .build()
        let issues = validate(card)
        let found = issues.contains { $0.code == "MISSING_INPUT_ID" && $0.severity == .error }
        XCTAssertTrue(found)
    }

    func testValidateInputNumberMinGreaterThanMax() {
        let card = AdaptiveCardBuilder()
            .addInputNumber { inp in
                inp.withID("qty").withMin(100).withMax(10)
            }
            .build()
        let issues = validate(card)
        let found = issues.contains { $0.code == "MIN_GREATER_THAN_MAX" && $0.severity == .error }
        XCTAssertTrue(found)
    }

    func testValidateDuplicateID() {
        let card = AdaptiveCardBuilder()
            .addTextBlock { tb in tb.withText("First").withID("dup") }
            .addTextBlock { tb in tb.withText("Second").withID("dup") }
            .build()
        let issues = validate(card)
        let found = issues.contains { $0.code == "DUPLICATE_ID" }
        XCTAssertTrue(found)
    }

    func testValidateInvalidSelectActionShowCard() {
        let showCard: Card = ["type": "Action.ShowCard"]
        let card: Card = [
            "type": "AdaptiveCard",
            "version": "1.5",
            "$schema": "https://x.com",
            "selectAction": showCard,
            "body": [["type": "TextBlock", "text": "x"]] as [Any],
        ]
        let issues = validate(card)
        let found = issues.contains { $0.code == "INVALID_SELECT_ACTION" }
        XCTAssertTrue(found)
    }

    func testValidateVersionMismatchTable() {
        let card = AdaptiveCardBuilder()
            .withVersion("1.2")  // Table requires 1.5
            .addTable { tb in
                tb.addColumn(["width": 1])
                    .addRow(["cells": [] as [Any]])
            }
            .build()
        let issues = validate(card)
        let found = issues.contains { $0.code == "VERSION_MISMATCH" && $0.severity == .warning }
        XCTAssertTrue(found)
    }

    func testValidateAndThrowValidCard() {
        let card = AdaptiveCardBuilder()
            .withVersion("1.5")
            .addTextBlock { tb in tb.withText("OK") }
            .build()
        XCTAssertNoThrow(try validateAndThrow(card))
    }

    func testValidateAndThrowInvalidCard() {
        let card: Card = ["type": "AdaptiveCard"]  // missing version
        XCTAssertThrowsError(try validateAndThrow(card))
    }

    func testAdaptiveCardValidationErrorMessage() {
        let card: Card = ["type": "AdaptiveCard"]  // missing version
        do {
            try validateAndThrow(card)
            XCTFail("Expected error to be thrown")
        } catch let error as AdaptiveCardValidationError {
            XCTAssertTrue(error.description.contains("validation failed"))
            XCTAssertFalse(error.issues.isEmpty)
        } catch {
            XCTFail("Unexpected error type: \(error)")
        }
    }
}
