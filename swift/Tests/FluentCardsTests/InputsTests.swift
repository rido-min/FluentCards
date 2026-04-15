import XCTest
@testable import FluentCards

final class InputsTests: XCTestCase {
    func testInputTextBuilder() {
        let card = AdaptiveCardBuilder()
            .addInputText { it in
                it.withID("name")
                    .withLabel("Your Name")
                    .withPlaceholder("Enter name")
                    .withMaxLength(100)
                    .withIsMultiline(false)
                    .withStyle(.email)
                    .withIsRequired(true)
                    .withErrorMessage("Name is required")
            }
            .build()
        let el = (card["body"] as! [Any])[0] as! [String: Any]
        XCTAssertEqual(el["type"] as? String, "Input.Text")
        XCTAssertEqual(el["id"] as? String, "name")
        XCTAssertEqual(el["label"] as? String, "Your Name")
        XCTAssertEqual(el["placeholder"] as? String, "Enter name")
        XCTAssertEqual(el["maxLength"] as? Int, 100)
        XCTAssertEqual(el["isMultiline"] as? Bool, false)
        XCTAssertEqual(el["style"] as? String, "email")
        XCTAssertEqual(el["isRequired"] as? Bool, true)
        XCTAssertEqual(el["errorMessage"] as? String, "Name is required")
    }

    func testInputNumberBuilder() {
        let card = AdaptiveCardBuilder()
            .addInputNumber { inp in
                inp.withID("qty")
                    .withLabel("Quantity")
                    .withMin(1)
                    .withMax(100)
                    .withValue(10)
            }
            .build()
        let el = (card["body"] as! [Any])[0] as! [String: Any]
        XCTAssertEqual(el["type"] as? String, "Input.Number")
        XCTAssertEqual(el["id"] as? String, "qty")
        XCTAssertEqual(el["min"] as? Double, 1.0)
        XCTAssertEqual(el["max"] as? Double, 100.0)
        XCTAssertEqual(el["value"] as? Double, 10.0)
    }

    func testInputDateBuilder() {
        let card = AdaptiveCardBuilder()
            .addInputDate { id in
                id.withID("start")
                    .withLabel("Start Date")
                    .withMin("2025-01-01")
                    .withMax("2026-12-31")
                    .withValue("2025-06-15")
            }
            .build()
        let el = (card["body"] as! [Any])[0] as! [String: Any]
        XCTAssertEqual(el["type"] as? String, "Input.Date")
        XCTAssertEqual(el["id"] as? String, "start")
        XCTAssertEqual(el["min"] as? String, "2025-01-01")
        XCTAssertEqual(el["max"] as? String, "2026-12-31")
        XCTAssertEqual(el["value"] as? String, "2025-06-15")
    }

    func testInputTimeBuilder() {
        let card = AdaptiveCardBuilder()
            .addInputTime { it in
                it.withID("meeting-time")
                    .withMin("09:00")
                    .withMax("17:00")
            }
            .build()
        let el = (card["body"] as! [Any])[0] as! [String: Any]
        XCTAssertEqual(el["type"] as? String, "Input.Time")
        XCTAssertEqual(el["id"] as? String, "meeting-time")
        XCTAssertEqual(el["min"] as? String, "09:00")
        XCTAssertEqual(el["max"] as? String, "17:00")
    }

    func testInputToggleBuilder() {
        let card = AdaptiveCardBuilder()
            .addInputToggle { it in
                it.withID("agree")
                    .withTitle("I agree to the terms")
                    .withValueOn("true")
                    .withValueOff("false")
                    .withWrap(true)
            }
            .build()
        let el = (card["body"] as! [Any])[0] as! [String: Any]
        XCTAssertEqual(el["type"] as? String, "Input.Toggle")
        XCTAssertEqual(el["id"] as? String, "agree")
        XCTAssertEqual(el["title"] as? String, "I agree to the terms")
        XCTAssertEqual(el["valueOn"] as? String, "true")
        XCTAssertEqual(el["valueOff"] as? String, "false")
        XCTAssertEqual(el["wrap"] as? Bool, true)
    }

    func testInputChoiceSetBuilder() {
        let card = AdaptiveCardBuilder()
            .addInputChoiceSet { ics in
                ics.withID("color")
                    .withLabel("Favorite Color")
                    .withStyle(.expanded)
                    .addChoice("Red", "red")
                    .addChoice("Green", "green")
                    .addChoice("Blue", "blue")
            }
            .build()
        let el = (card["body"] as! [Any])[0] as! [String: Any]
        XCTAssertEqual(el["type"] as? String, "Input.ChoiceSet")
        XCTAssertEqual(el["id"] as? String, "color")
        XCTAssertEqual(el["style"] as? String, "expanded")
        let choices = el["choices"] as! [Any]
        XCTAssertEqual(choices.count, 3)
        let choice0 = choices[0] as! [String: Any]
        XCTAssertEqual(choice0["title"] as? String, "Red")
        XCTAssertEqual(choice0["value"] as? String, "red")
    }

    func testInputChoiceSetBuilderWithChoicesData() {
        let card = AdaptiveCardBuilder()
            .addInputChoiceSet { ics in
                ics.withID("people-picker")
                    .withChoicesData("graph.microsoft.com/users")
            }
            .build()
        let el = (card["body"] as! [Any])[0] as! [String: Any]
        let choicesData = el["choices.data"] as! [String: Any]
        XCTAssertEqual(choicesData["type"] as? String, "Data.Query")
        XCTAssertEqual(choicesData["dataset"] as? String, "graph.microsoft.com/users")
    }
}
