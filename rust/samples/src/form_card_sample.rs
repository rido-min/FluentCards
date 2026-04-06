use fluent_cards::*;

pub fn create_contact_form() -> Card {
    AdaptiveCardBuilder::new()
        .with_version("1.5")
        .add_text_block(|tb| {
            tb.with_text("Contact Us")
                .with_size(TextSize::Large)
                .with_weight(TextWeight::Bolder);
        })
        .add_input_text(|i| {
            i.with_id("name")
                .with_label("Name")
                .with_placeholder("Enter your name")
                .with_is_required(true)
                .with_error_message("Name is required");
        })
        .add_input_text(|i| {
            i.with_id("email")
                .with_label("Email")
                .with_placeholder("Enter your email")
                .with_style(TextInputStyle::Email)
                .with_is_required(true);
        })
        .add_input_text(|i| {
            i.with_id("message")
                .with_label("Message")
                .with_placeholder("How can we help?")
                .with_is_multiline(true)
                .with_max_length(500);
        })
        .add_action(|a| {
            a.submit(Some("Send Message"))
                .with_style(ActionStyle::Positive);
        })
        .build()
}

pub fn create_survey_form() -> Card {
    AdaptiveCardBuilder::new()
        .with_version("1.5")
        .add_text_block(|tb| {
            tb.with_text("Customer Satisfaction Survey")
                .with_size(TextSize::Large)
                .with_weight(TextWeight::Bolder);
        })
        .add_input_choice_set(|i| {
            i.with_id("satisfaction")
                .with_label("How satisfied are you?")
                .add_choice("Very Satisfied", "5")
                .add_choice("Satisfied", "4")
                .add_choice("Neutral", "3")
                .add_choice("Dissatisfied", "2")
                .add_choice("Very Dissatisfied", "1")
                .with_is_required(true);
        })
        .add_input_text(|i| {
            i.with_id("feedback")
                .with_label("Additional Feedback")
                .with_placeholder("Tell us more...")
                .with_is_multiline(true);
        })
        .add_action(|a| {
            a.submit(Some("Submit Survey"))
                .with_style(ActionStyle::Positive);
        })
        .build()
}

pub fn create_registration_form() -> Card {
    AdaptiveCardBuilder::new()
        .with_version("1.5")
        .add_text_block(|tb| {
            tb.with_text("Event Registration")
                .with_size(TextSize::Large)
                .with_weight(TextWeight::Bolder);
        })
        .add_input_text(|i| {
            i.with_id("fullName")
                .with_label("Full Name")
                .with_is_required(true);
        })
        .add_input_text(|i| {
            i.with_id("email")
                .with_label("Email Address")
                .with_style(TextInputStyle::Email)
                .with_is_required(true);
        })
        .add_input_date(|i| {
            i.with_id("eventDate").with_label("Event Date");
        })
        .add_input_toggle(|i| {
            i.with_id("newsletter")
                .with_title("Subscribe to newsletter")
                .with_value("true");
        })
        .add_action(|a| {
            a.submit(Some("Register"))
                .with_style(ActionStyle::Positive);
        })
        .build()
}
