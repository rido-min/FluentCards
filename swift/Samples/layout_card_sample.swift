import FluentCards

func createTwoColumnCard() -> Card {
    return AdaptiveCardBuilder()
        .withVersion("1.5")
        .addTextBlock { tb in
            tb.withText("Product Information")
                .withSize(.large)
                .withWeight(.bolder)
        }
        .addColumnSet { cs in
            cs.addColumn { col in
                col.withWidth("auto")
                    .addImage { img in
                        img.withURL("https://adaptivecards.io/content/adaptive-card-50.png")
                            .withSize(.medium)
                    }
            }
            .addColumn { col in
                col.withWidth("stretch")
                    .addTextBlock { tb in
                        tb.withText("Adaptive Cards SDK")
                            .withWeight(.bolder)
                    }
                    .addTextBlock { tb in
                        tb.withText("Create platform-agnostic UI snippets")
                            .withWrap(true)
                    }
                    .addTextBlock { tb in
                        tb.withText("$49.99")
                            .withColor(.good)
                            .withSize(.large)
                    }
            }
        }
        .addAction { a in
            a.submit("Add to Cart").withStyle(.positive)
        }
        .build()
}

func createStyledContainerCard() -> Card {
    return AdaptiveCardBuilder()
        .withVersion("1.5")
        .addContainer { c in
            c.withStyle(.emphasis)
                .addTextBlock { tb in
                    tb.withText("Important Notice")
                        .withSize(.large)
                        .withWeight(.bolder)
                }
                .addTextBlock { tb in
                    tb.withText("This is an emphasized section with important information.")
                        .withWrap(true)
                }
        }
        .addContainer { c in
            c.addTextBlock { tb in
                tb.withText("Regular Section")
                    .withWeight(.bolder)
            }
            .addTextBlock { tb in
                tb.withText("This is a normal section with regular styling.")
                    .withWrap(true)
            }
        }
        .addContainer { c in
            c.withStyle(.accent)
                .addTextBlock { tb in
                    tb.withText("Highlighted Section")
                        .withWeight(.bolder)
                }
                .addTextBlock { tb in
                    tb.withText("This section uses accent styling to stand out.")
                        .withWrap(true)
                }
        }
        .build()
}

func createFactSetCard() -> Card {
    return AdaptiveCardBuilder()
        .withVersion("1.5")
        .addTextBlock { tb in
            tb.withText("Meeting Details")
                .withSize(.large)
                .withWeight(.bolder)
        }
        .addFactSet { fs in
            fs.addFact("Date", "December 15, 2024")
                .addFact("Time", "2:00 PM - 3:00 PM")
                .addFact("Location", "Conference Room A")
                .addFact("Organizer", "John Smith")
                .addFact("Attendees", "12 people")
        }
        .addAction { a in
            a.openURL("https://example.com/meeting/123").withTitle("Join Meeting")
        }
        .build()
}

func createNestedContainerCard() -> Card {
    return AdaptiveCardBuilder()
        .withVersion("1.5")
        .addTextBlock { tb in
            tb.withText("Dashboard")
                .withSize(.extraLarge)
                .withWeight(.bolder)
        }
        .addContainer { c in
            c.withStyle(.emphasis)
                .addTextBlock { tb in
                    tb.withText("Statistics")
                        .withSize(.large)
                        .withWeight(.bolder)
                }
                .addColumnSet { cs in
                    cs.addColumn { col in
                        col.withWidth("stretch")
                            .addContainer { cont in
                                cont.withStyle(.good)
                                    .addTextBlock { tb in
                                        tb.withText("Active Users")
                                            .withWeight(.bolder)
                                    }
                                    .addTextBlock { tb in
                                        tb.withText("1,234")
                                            .withSize(.extraLarge)
                                    }
                            }
                    }
                    .addColumn { col in
                        col.withWidth("stretch")
                            .addContainer { cont in
                                cont.withStyle(.attention)
                                    .addTextBlock { tb in
                                        tb.withText("Pending Issues")
                                            .withWeight(.bolder)
                                    }
                                    .addTextBlock { tb in
                                        tb.withText("42")
                                            .withSize(.extraLarge)
                                    }
                            }
                    }
                }
        }
        .build()
}
