from fluent_cards import AdaptiveCardBuilder, TextSize, TextWeight


def create_action_submit_execute_card() -> dict:
    """Creates a card with Action.Execute and Action.Submit actions and custom verbs/data."""
    return (
        AdaptiveCardBuilder.create()
        .with_version('1.4')
        .add_text_block(
            lambda tb: tb.with_text('welcome to ac 11')
            .with_size(TextSize.Large)
            .with_weight(TextWeight.Bolder)
        )
        .add_text_block(lambda tb: tb.with_text('click the buttons below'))
        .add_action(
            lambda a: a.execute('Test AC Action')
            .with_data({'message': 'button clicked !!'})
            .with_verb('testAction')
        )
        .add_action(
            lambda a: a.submit('Open Task Module').with_data(
                {'msteams': {'type': 'task/fetch'}}
            )
        )
        .add_action(
            lambda a: a.execute('Request File Upload').with_verb('requestFileUpload')
        )
        .build()
    )
