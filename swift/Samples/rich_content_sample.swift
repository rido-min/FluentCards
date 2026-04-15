import FluentCards

func createRichTextCard() -> Card {
    return AdaptiveCardBuilder()
        .withVersion("1.5")
        .addRichTextBlock { rtb in
            rtb.addTextRun { tr in
                tr.withText("Welcome ").withSize(.large)
            }
            .addTextRun { tr in
                tr.withText("to FluentCards!")
                    .withSize(.large)
                    .withWeight(.bolder)
                    .withColor(.accent)
            }
            .addTextRun { tr in
                tr.withText("\n\nThis demonstrates ").withSize(.default)
            }
            .addTextRun { tr in
                tr.withText("rich text formatting")
                    .withWeight(.bolder)
                    .withColor(.good)
            }
            .addTextRun { tr in
                tr.withText(" with multiple text runs.").withSize(.default)
            }
        }
        .build()
}

func createImageSetCard() -> Card {
    return AdaptiveCardBuilder()
        .withVersion("1.5")
        .addTextBlock { tb in
            tb.withText("Photo Gallery")
                .withSize(.large)
                .withWeight(.bolder)
        }
        .addImageSet { imgSet in
            imgSet.withImageSize(.medium)
                .addImage { img in
                    img.withURL("https://adaptivecards.io/content/adaptive-card-50.png")
                }
                .addImage { img in
                    img.withURL("https://adaptivecards.io/content/adaptive-card-50.png")
                }
                .addImage { img in
                    img.withURL("https://adaptivecards.io/content/adaptive-card-50.png")
                }
        }
        .addTextBlock { tb in
            tb.withText("View more photos in the gallery").withWrap(true)
        }
        .build()
}

func createTableCard() -> Card {
    return AdaptiveCardBuilder()
        .withVersion("1.5")
        .addTextBlock { tb in
            tb.withText("Sales Report")
                .withSize(.large)
                .withWeight(.bolder)
        }
        .addTable { table in
            table
                .addColumn(["width": "2"])
                .addColumn(["width": "1"])
                .addColumn(["width": "1"])
                .addRow([
                    "type": "TableRow",
                    "cells": [
                        ["type": "TableCell", "items": [["type": "TextBlock", "text": "Product A"]]] as [String: Any],
                        ["type": "TableCell", "items": [["type": "TextBlock", "text": "150"]]] as [String: Any],
                        ["type": "TableCell", "items": [["type": "TextBlock", "text": "$15,000"]]] as [String: Any],
                    ] as [Any],
                ])
                .addRow([
                    "type": "TableRow",
                    "cells": [
                        ["type": "TableCell", "items": [["type": "TextBlock", "text": "Product B"]]] as [String: Any],
                        ["type": "TableCell", "items": [["type": "TextBlock", "text": "200"]]] as [String: Any],
                        ["type": "TableCell", "items": [["type": "TextBlock", "text": "$20,000"]]] as [String: Any],
                    ] as [Any],
                ])
                .addRow([
                    "type": "TableRow",
                    "cells": [
                        ["type": "TableCell", "items": [["type": "TextBlock", "text": "Product C"]]] as [String: Any],
                        ["type": "TableCell", "items": [["type": "TextBlock", "text": "100"]]] as [String: Any],
                        ["type": "TableCell", "items": [["type": "TextBlock", "text": "$10,000"]]] as [String: Any],
                    ] as [Any],
                ])
        }
        .build()
}

func createMediaCard() -> Card {
    return AdaptiveCardBuilder()
        .withVersion("1.5")
        .addTextBlock { tb in
            tb.withText("Video Tutorial")
                .withSize(.large)
                .withWeight(.bolder)
        }
        .addMedia { media in
            media.addSource("https://example.com/video.mp4", "video/mp4")
                .withPoster("https://example.com/poster.jpg")
                .withAltText("Getting started with FluentCards")
        }
        .addTextBlock { tb in
            tb.withText("Watch this tutorial to learn the basics of FluentCards.")
                .withWrap(true)
        }
        .build()
}

func createComprehensiveCard() -> Card {
    return AdaptiveCardBuilder()
        .withVersion("1.5")
        .addTextBlock { tb in
            tb.withText("Product Launch Announcement")
                .withSize(.extraLarge)
                .withWeight(.bolder)
                .withColor(.accent)
        }
        .addImage { img in
            img.withURL("https://adaptivecards.io/content/adaptive-card-50.png")
                .withSize(.large)
                .withHorizontalAlignment(.center)
        }
        .addRichTextBlock { rtb in
            rtb.addTextRun { tr in
                tr.withText("Introducing ").withSize(.medium)
            }
            .addTextRun { tr in
                tr.withText("FluentCards 2.0")
                    .withSize(.medium)
                    .withWeight(.bolder)
                    .withColor(.good)
            }
        }
        .addFactSet { fs in
            fs.addFact("Release Date", "January 1, 2025")
                .addFact("Version", "2.0.0")
                .addFact("License", "MIT")
        }
        .addAction { a in
            a.openURL("https://github.com/rido-min/FluentCards").withTitle("View on GitHub")
        }
        .addAction { a in
            a.submit("Get Notified").withStyle(.positive)
        }
        .build()
}
