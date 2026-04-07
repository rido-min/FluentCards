import XCTest
@testable import FluentCards

final class ActionsTests: XCTestCase {
    func testActionBuilderOpenURL() {
        let card = AdaptiveCardBuilder()
            .addAction { a in
                a.openURL("https://example.com").withTitle("Go There")
            }
            .build()
        let action = (card["actions"] as! [Any])[0] as! [String: Any]
        XCTAssertEqual(action["type"] as? String, "Action.OpenUrl")
        XCTAssertEqual(action["url"] as? String, "https://example.com")
        XCTAssertEqual(action["title"] as? String, "Go There")
    }

    func testActionBuilderSubmit() {
        let card = AdaptiveCardBuilder()
            .addAction { a in
                a.submit("Send").withStyle(.positive)
            }
            .build()
        let action = (card["actions"] as! [Any])[0] as! [String: Any]
        XCTAssertEqual(action["type"] as? String, "Action.Submit")
        XCTAssertEqual(action["title"] as? String, "Send")
        XCTAssertEqual(action["style"] as? String, "positive")
    }

    func testActionBuilderSubmitWithData() {
        let card = AdaptiveCardBuilder()
            .addAction { a in
                a.submit().withData(["action": "approve"] as [String: Any])
            }
            .build()
        let action = (card["actions"] as! [Any])[0] as! [String: Any]
        let data = action["data"] as! [String: Any]
        XCTAssertEqual(data["action"] as? String, "approve")
    }

    func testActionBuilderShowCard() {
        let innerCard = AdaptiveCardBuilder()
            .addTextBlock { tb in tb.withText("Inner") }
            .build()
        let card = AdaptiveCardBuilder()
            .addAction { a in
                a.showCard("Show More").withCard(innerCard)
            }
            .build()
        let action = (card["actions"] as! [Any])[0] as! [String: Any]
        XCTAssertEqual(action["type"] as? String, "Action.ShowCard")
        XCTAssertNotNil(action["card"])
    }

    func testActionBuilderToggleVisibility() {
        let card = AdaptiveCardBuilder()
            .addAction { a in
                a.toggleVisibility("Toggle")
                    .addTargetElement("details-section")
                    .addTargetElement("header", isVisible: true)
            }
            .build()
        let action = (card["actions"] as! [Any])[0] as! [String: Any]
        XCTAssertEqual(action["type"] as? String, "Action.ToggleVisibility")
        let targets = action["targetElements"] as! [Any]
        XCTAssertEqual(targets.count, 2)
        XCTAssertEqual(targets[0] as? String, "details-section")
        let target2 = targets[1] as! [String: Any]
        XCTAssertEqual(target2["elementId"] as? String, "header")
        XCTAssertEqual(target2["isVisible"] as? Bool, true)
    }

    func testActionBuilderExecute() {
        let card = AdaptiveCardBuilder()
            .addAction { a in
                a.execute("Run")
                    .withVerb("doSomething")
                    .withAssociatedInputs(.auto)
            }
            .build()
        let action = (card["actions"] as! [Any])[0] as! [String: Any]
        XCTAssertEqual(action["type"] as? String, "Action.Execute")
        XCTAssertEqual(action["verb"] as? String, "doSomething")
        XCTAssertEqual(action["associatedInputs"] as? String, "auto")
    }

    func testActionBuilderModifierIgnoredWithoutType() {
        // Ensure modifier calls on an untyped builder don't crash before build()
        let ab = ActionBuilder()
        ab.withTitle("ignored")  // should not crash
        // Note: calling ab.build() here would fatalError, so we just verify no crash on modifier
    }

    func testActionBuilderBuildWithType() {
        let ab = ActionBuilder()
        ab.openURL("https://example.com")
        let built = ab.build()
        XCTAssertEqual(built["type"] as? String, "Action.OpenUrl")
    }
}
