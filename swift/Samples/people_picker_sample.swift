import FluentCards

func createPeoplePickerCard() -> Card {
    return AdaptiveCardBuilder()
        .withVersion("1.6")
        .addInputChoiceSet { i in
            i.withID("people-picker")
                .withLabel("Select users in the whole organization")
                .withIsMultiSelect(true)
                .withValue("user1,user2")
                .withChoicesData("graph.microsoft.com/users")
        }
        .addAction { a in
            a.submit("Submit")
        }
        .build()
}
