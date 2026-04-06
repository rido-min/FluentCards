use fluent_cards::*;

pub fn create_rich_text_card() -> Card {
    AdaptiveCardBuilder::new()
        .with_version("1.5")
        .add_rich_text_block(|rtb| {
            rtb.add_text_run(|tr| {
                tr.with_text("Welcome ").with_size(TextSize::Large);
            })
            .add_text_run(|tr| {
                tr.with_text("to FluentCards!")
                    .with_size(TextSize::Large)
                    .with_weight(TextWeight::Bolder)
                    .with_color(TextColor::Accent);
            })
            .add_text_run(|tr| {
                tr.with_text("\n\nThis demonstrates ")
                    .with_size(TextSize::Default);
            })
            .add_text_run(|tr| {
                tr.with_text("rich text formatting")
                    .with_weight(TextWeight::Bolder)
                    .with_color(TextColor::Good);
            })
            .add_text_run(|tr| {
                tr.with_text(" with multiple text runs.")
                    .with_size(TextSize::Default);
            });
        })
        .build()
}

pub fn create_image_set_card() -> Card {
    AdaptiveCardBuilder::new()
        .with_version("1.5")
        .add_text_block(|tb| {
            tb.with_text("Photo Gallery")
                .with_size(TextSize::Large)
                .with_weight(TextWeight::Bolder);
        })
        .add_image_set(|img_set| {
            img_set
                .with_image_size(ImageSize::Medium)
                .add_image(|img| {
                    img.with_url("https://adaptivecards.io/content/adaptive-card-50.png");
                })
                .add_image(|img| {
                    img.with_url("https://adaptivecards.io/content/adaptive-card-50.png");
                })
                .add_image(|img| {
                    img.with_url("https://adaptivecards.io/content/adaptive-card-50.png");
                });
        })
        .add_text_block(|tb| {
            tb.with_text("View more photos in the gallery")
                .with_wrap(true);
        })
        .build()
}

pub fn create_table_card() -> Card {
    AdaptiveCardBuilder::new()
        .with_version("1.5")
        .add_text_block(|tb| {
            tb.with_text("Sales Report")
                .with_size(TextSize::Large)
                .with_weight(TextWeight::Bolder);
        })
        .add_table(|table| {
            table
                .add_column(
                    serde_json::json!({"width": "2"})
                        .as_object()
                        .unwrap()
                        .clone(),
                )
                .add_column(
                    serde_json::json!({"width": "1"})
                        .as_object()
                        .unwrap()
                        .clone(),
                )
                .add_column(
                    serde_json::json!({"width": "1"})
                        .as_object()
                        .unwrap()
                        .clone(),
                )
                .add_row(
                    serde_json::json!({
                        "type": "TableRow",
                        "cells": [
                            {"type": "TableCell", "items": [{"type": "TextBlock", "text": "Product A"}]},
                            {"type": "TableCell", "items": [{"type": "TextBlock", "text": "150"}]},
                            {"type": "TableCell", "items": [{"type": "TextBlock", "text": "$15,000"}]}
                        ]
                    })
                    .as_object()
                    .unwrap()
                    .clone(),
                )
                .add_row(
                    serde_json::json!({
                        "type": "TableRow",
                        "cells": [
                            {"type": "TableCell", "items": [{"type": "TextBlock", "text": "Product B"}]},
                            {"type": "TableCell", "items": [{"type": "TextBlock", "text": "200"}]},
                            {"type": "TableCell", "items": [{"type": "TextBlock", "text": "$20,000"}]}
                        ]
                    })
                    .as_object()
                    .unwrap()
                    .clone(),
                )
                .add_row(
                    serde_json::json!({
                        "type": "TableRow",
                        "cells": [
                            {"type": "TableCell", "items": [{"type": "TextBlock", "text": "Product C"}]},
                            {"type": "TableCell", "items": [{"type": "TextBlock", "text": "100"}]},
                            {"type": "TableCell", "items": [{"type": "TextBlock", "text": "$10,000"}]}
                        ]
                    })
                    .as_object()
                    .unwrap()
                    .clone(),
                );
        })
        .build()
}

pub fn create_media_card() -> Card {
    AdaptiveCardBuilder::new()
        .with_version("1.5")
        .add_text_block(|tb| {
            tb.with_text("Video Tutorial")
                .with_size(TextSize::Large)
                .with_weight(TextWeight::Bolder);
        })
        .add_media(|media| {
            media
                .add_source("https://example.com/video.mp4", "video/mp4")
                .with_poster("https://example.com/poster.jpg")
                .with_alt_text("Getting started with FluentCards");
        })
        .add_text_block(|tb| {
            tb.with_text("Watch this tutorial to learn the basics of FluentCards.")
                .with_wrap(true);
        })
        .build()
}

pub fn create_comprehensive_card() -> Card {
    AdaptiveCardBuilder::new()
        .with_version("1.5")
        .add_text_block(|tb| {
            tb.with_text("Product Launch Announcement")
                .with_size(TextSize::ExtraLarge)
                .with_weight(TextWeight::Bolder)
                .with_color(TextColor::Accent);
        })
        .add_image(|img| {
            img.with_url("https://adaptivecards.io/content/adaptive-card-50.png")
                .with_size(ImageSize::Large)
                .with_horizontal_alignment(HorizontalAlignment::Center);
        })
        .add_rich_text_block(|rtb| {
            rtb.add_text_run(|tr| {
                tr.with_text("Introducing ").with_size(TextSize::Medium);
            })
            .add_text_run(|tr| {
                tr.with_text("FluentCards 2.0")
                    .with_size(TextSize::Medium)
                    .with_weight(TextWeight::Bolder)
                    .with_color(TextColor::Good);
            });
        })
        .add_fact_set(|fs| {
            fs.add_fact("Release Date", "January 1, 2025")
                .add_fact("Version", "2.0.0")
                .add_fact("License", "MIT");
        })
        .add_action(|a| {
            a.open_url("https://github.com/rido-min/FluentCards")
                .with_title("View on GitHub");
        })
        .add_action(|a| {
            a.submit("Get Notified")
                .with_style(ActionStyle::Positive);
        })
        .build()
}
