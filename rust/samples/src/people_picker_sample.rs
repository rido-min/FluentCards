use fluent_cards::*;

pub fn create_people_picker_card() -> Card {
    AdaptiveCardBuilder::new()
        .with_version("1.6")
        .add_input_choice_set(|i| {
            i.with_id("people-picker")
                .with_label("Select users in the whole organization")
                .with_is_multi_select(true)
                .with_value("user1,user2")
                .with_choices_data("graph.microsoft.com/users");
        })
        .add_action(|a| {
            a.submit("Submit");
        })
        .build()
}
