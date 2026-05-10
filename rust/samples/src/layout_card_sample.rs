use fluent_cards::*;

pub fn create_two_column_card() -> Card {
    AdaptiveCardBuilder::new()
        .with_version("1.5")
        .add_text_block(|tb| {
            tb.with_text("Product Information")
                .with_size(TextSize::Large)
                .with_weight(TextWeight::Bolder);
        })
        .add_column_set(|cs| {
            cs.add_column(|col| {
                col.with_width("auto").add_image(|img| {
                    img.with_url("https://adaptivecards.io/content/adaptive-card-50.png")
                        .with_size(ImageSize::Medium);
                });
            })
            .add_column(|col| {
                col.with_width("stretch")
                    .add_text_block(|tb| {
                        tb.with_text("Adaptive Cards SDK")
                            .with_weight(TextWeight::Bolder);
                    })
                    .add_text_block(|tb| {
                        tb.with_text("Create platform-agnostic UI snippets")
                            .with_wrap(true);
                    })
                    .add_text_block(|tb| {
                        tb.with_text("$49.99")
                            .with_color(TextColor::Good)
                            .with_size(TextSize::Large);
                    });
            });
        })
        .add_action(|a| {
            a.submit("Add to Cart")
                .with_style(ActionStyle::Positive);
        })
        .build()
}

pub fn create_styled_container_card() -> Card {
    AdaptiveCardBuilder::new()
        .with_version("1.5")
        .add_container(|c| {
            c.with_style(ContainerStyle::Emphasis)
                .add_text_block(|tb| {
                    tb.with_text("Important Notice")
                        .with_size(TextSize::Large)
                        .with_weight(TextWeight::Bolder);
                })
                .add_text_block(|tb| {
                    tb.with_text(
                        "This is an emphasized section with important information.",
                    )
                    .with_wrap(true);
                });
        })
        .add_container(|c| {
            c.add_text_block(|tb| {
                tb.with_text("Regular Section")
                    .with_weight(TextWeight::Bolder);
            })
            .add_text_block(|tb| {
                tb.with_text("This is a normal section with regular styling.")
                    .with_wrap(true);
            });
        })
        .add_container(|c| {
            c.with_style(ContainerStyle::Accent)
                .add_text_block(|tb| {
                    tb.with_text("Highlighted Section")
                        .with_weight(TextWeight::Bolder);
                })
                .add_text_block(|tb| {
                    tb.with_text("This section uses accent styling to stand out.")
                        .with_wrap(true);
                });
        })
        .build()
}

pub fn create_fact_set_card() -> Card {
    AdaptiveCardBuilder::new()
        .with_version("1.5")
        .add_text_block(|tb| {
            tb.with_text("Meeting Details")
                .with_size(TextSize::Large)
                .with_weight(TextWeight::Bolder);
        })
        .add_fact_set(|fs| {
            fs.add_fact("Date", "December 15, 2024")
                .add_fact("Time", "2:00 PM - 3:00 PM")
                .add_fact("Location", "Conference Room A")
                .add_fact("Organizer", "John Smith")
                .add_fact("Attendees", "12 people");
        })
        .add_action(|a| {
            a.open_url("https://example.com/meeting/123")
                .with_title("Join Meeting");
        })
        .build()
}

pub fn create_nested_container_card() -> Card {
    AdaptiveCardBuilder::new()
        .with_version("1.5")
        .add_text_block(|tb| {
            tb.with_text("Dashboard")
                .with_size(TextSize::ExtraLarge)
                .with_weight(TextWeight::Bolder);
        })
        .add_container(|c| {
            c.with_style(ContainerStyle::Emphasis)
                .add_text_block(|tb| {
                    tb.with_text("Statistics")
                        .with_size(TextSize::Large)
                        .with_weight(TextWeight::Bolder);
                })
                .add_column_set(|cs| {
                    cs.add_column(|col| {
                        col.with_width("stretch").add_container(|cont| {
                            cont.with_style(ContainerStyle::Good)
                                .add_text_block(|tb| {
                                    tb.with_text("Active Users")
                                        .with_weight(TextWeight::Bolder);
                                })
                                .add_text_block(|tb| {
                                    tb.with_text("1,234")
                                        .with_size(TextSize::ExtraLarge);
                                });
                        });
                    })
                    .add_column(|col| {
                        col.with_width("stretch").add_container(|cont| {
                            cont.with_style(ContainerStyle::Attention)
                                .add_text_block(|tb| {
                                    tb.with_text("Pending Issues")
                                        .with_weight(TextWeight::Bolder);
                                })
                                .add_text_block(|tb| {
                                    tb.with_text("42")
                                        .with_size(TextSize::ExtraLarge);
                                });
                        });
                    });
                });
        })
        .build()
}
