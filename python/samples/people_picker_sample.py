from fluent_cards import AdaptiveCardBuilder


def create_people_picker_card() -> dict:
    """Creates a people picker card that searches users from Microsoft Graph."""
    return (
        AdaptiveCardBuilder.create()
        .with_version('1.6')
        .add_input_choice_set(
            lambda i: i.with_id('people-picker')
            .with_label('Select users in the whole organization')
            .is_multi_select()
            .with_value('user1,user2')
            .with_choices_data('graph.microsoft.com/users')
        )
        .add_action(lambda a: a.submit('Submit'))
        .build()
    )
