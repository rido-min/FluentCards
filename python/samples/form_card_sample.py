from fluent_cards import (
    AdaptiveCardBuilder,
    TextSize,
    TextWeight,
    TextInputStyle,
    ActionStyle,
    ChoiceInputStyle,
)


def create_contact_form():
    """Creates a contact form card."""
    return (
        AdaptiveCardBuilder.create()
        .with_version('1.5')
        .add_text_block(
            lambda tb: tb.with_text('Contact Us')
            .with_size(TextSize.Large)
            .with_weight(TextWeight.Bolder)
        )
        .add_input_text(
            lambda i: i.with_id('name')
            .with_label('Name')
            .with_placeholder('Enter your name')
            .with_is_required()
            .with_error_message('Name is required')
        )
        .add_input_text(
            lambda i: i.with_id('email')
            .with_label('Email')
            .with_placeholder('Enter your email')
            .with_style(TextInputStyle.Email)
            .with_is_required()
        )
        .add_input_text(
            lambda i: i.with_id('message')
            .with_label('Message')
            .with_placeholder('How can we help?')
            .with_is_multiline()
            .with_max_length(500)
        )
        .add_action(
            lambda a: a.submit('Send Message').with_style(ActionStyle.Positive)
        )
        .build()
    )


def create_survey_form():
    """Creates a survey form card."""
    return (
        AdaptiveCardBuilder.create()
        .with_version('1.5')
        .add_text_block(
            lambda tb: tb.with_text('Customer Satisfaction Survey')
            .with_size(TextSize.Large)
            .with_weight(TextWeight.Bolder)
        )
        .add_input_choice_set(
            lambda i: i.with_id('satisfaction')
            .with_label('How satisfied are you?')
            .add_choice('Very Satisfied', '5')
            .add_choice('Satisfied', '4')
            .add_choice('Neutral', '3')
            .add_choice('Dissatisfied', '2')
            .add_choice('Very Dissatisfied', '1')
            .with_is_required()
        )
        .add_input_text(
            lambda i: i.with_id('feedback')
            .with_label('Additional Feedback')
            .with_placeholder('Tell us more...')
            .with_is_multiline()
        )
        .add_action(
            lambda a: a.submit('Submit Survey').with_style(ActionStyle.Positive)
        )
        .build()
    )


def create_registration_form():
    """Creates a registration form card."""
    return (
        AdaptiveCardBuilder.create()
        .with_version('1.5')
        .add_text_block(
            lambda tb: tb.with_text('Event Registration')
            .with_size(TextSize.Large)
            .with_weight(TextWeight.Bolder)
        )
        .add_input_text(
            lambda i: i.with_id('fullName').with_label('Full Name').with_is_required()
        )
        .add_input_text(
            lambda i: i.with_id('email')
            .with_label('Email Address')
            .with_style(TextInputStyle.Email)
            .with_is_required()
        )
        .add_input_date(
            lambda i: i.with_id('eventDate').with_label('Event Date')
        )
        .add_input_toggle(
            lambda i: i.with_id('newsletter')
            .with_title('Subscribe to newsletter')
            .with_value('true')
        )
        .add_action(
            lambda a: a.submit('Register').with_style(ActionStyle.Positive)
        )
        .build()
    )
