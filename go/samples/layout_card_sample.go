package main

import "github.com/rido-min/FluentCards/go/fluentcards"

func createTwoColumnCard() fluentcards.Card {
	return fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			tb.WithText("Product Information").
				WithSize(fluentcards.TextSizeLarge).
				WithWeight(fluentcards.TextWeightBolder)
		}).
		AddColumnSet(func(cs *fluentcards.ColumnSetBuilder) {
			cs.AddColumn(func(col *fluentcards.ColumnBuilder) {
				col.WithWidth("auto").
					AddImage(func(img *fluentcards.ImageBuilder) {
						img.WithURL("https://adaptivecards.io/content/adaptive-card-50.png").
							WithSize(fluentcards.ImageSizeMedium)
					})
			}).
				AddColumn(func(col *fluentcards.ColumnBuilder) {
					col.WithWidth("stretch").
						AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
							tb.WithText("Adaptive Cards SDK").
								WithWeight(fluentcards.TextWeightBolder)
						}).
						AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
							tb.WithText("Create platform-agnostic UI snippets").
								WithWrap(true)
						}).
						AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
							tb.WithText("$49.99").
								WithColor(fluentcards.TextColorGood).
								WithSize(fluentcards.TextSizeLarge)
						})
				})
		}).
		AddAction(func(a *fluentcards.ActionBuilder) {
			a.Submit("Add to Cart").WithStyle(fluentcards.ActionStylePositive)
		}).
		Build()
}

func createStyledContainerCard() fluentcards.Card {
	return fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddContainer(func(c *fluentcards.ContainerBuilder) {
			c.WithStyle(fluentcards.ContainerStyleEmphasis).
				AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
					tb.WithText("Important Notice").
						WithSize(fluentcards.TextSizeLarge).
						WithWeight(fluentcards.TextWeightBolder)
				}).
				AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
					tb.WithText("This is an emphasized section with important information.").
						WithWrap(true)
				})
		}).
		AddContainer(func(c *fluentcards.ContainerBuilder) {
			c.AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
				tb.WithText("Regular Section").
					WithWeight(fluentcards.TextWeightBolder)
			}).
				AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
					tb.WithText("This is a normal section with regular styling.").
						WithWrap(true)
				})
		}).
		AddContainer(func(c *fluentcards.ContainerBuilder) {
			c.WithStyle(fluentcards.ContainerStyleAccent).
				AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
					tb.WithText("Highlighted Section").
						WithWeight(fluentcards.TextWeightBolder)
				}).
				AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
					tb.WithText("This section uses accent styling to stand out.").
						WithWrap(true)
				})
		}).
		Build()
}

func createFactSetCard() fluentcards.Card {
	return fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			tb.WithText("Meeting Details").
				WithSize(fluentcards.TextSizeLarge).
				WithWeight(fluentcards.TextWeightBolder)
		}).
		AddFactSet(func(fs *fluentcards.FactSetBuilder) {
			fs.AddFact("Date", "December 15, 2024").
				AddFact("Time", "2:00 PM - 3:00 PM").
				AddFact("Location", "Conference Room A").
				AddFact("Organizer", "John Smith").
				AddFact("Attendees", "12 people")
		}).
		AddAction(func(a *fluentcards.ActionBuilder) {
			a.OpenURL("https://example.com/meeting/123").WithTitle("Join Meeting")
		}).
		Build()
}

func createNestedContainerCard() fluentcards.Card {
	return fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			tb.WithText("Dashboard").
				WithSize(fluentcards.TextSizeExtraLarge).
				WithWeight(fluentcards.TextWeightBolder)
		}).
		AddContainer(func(c *fluentcards.ContainerBuilder) {
			c.WithStyle(fluentcards.ContainerStyleEmphasis).
				AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
					tb.WithText("Statistics").
						WithSize(fluentcards.TextSizeLarge).
						WithWeight(fluentcards.TextWeightBolder)
				}).
				AddColumnSet(func(cs *fluentcards.ColumnSetBuilder) {
					cs.AddColumn(func(col *fluentcards.ColumnBuilder) {
						col.WithWidth("stretch").
							AddContainer(func(cont *fluentcards.ContainerBuilder) {
								cont.WithStyle(fluentcards.ContainerStyleGood).
									AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
										tb.WithText("Active Users").
											WithWeight(fluentcards.TextWeightBolder)
									}).
									AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
										tb.WithText("1,234").
											WithSize(fluentcards.TextSizeExtraLarge)
									})
							})
					}).
						AddColumn(func(col *fluentcards.ColumnBuilder) {
							col.WithWidth("stretch").
								AddContainer(func(cont *fluentcards.ContainerBuilder) {
									cont.WithStyle(fluentcards.ContainerStyleAttention).
										AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
											tb.WithText("Pending Issues").
												WithWeight(fluentcards.TextWeightBolder)
										}).
										AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
											tb.WithText("42").
												WithSize(fluentcards.TextSizeExtraLarge)
										})
								})
						})
				})
		}).
		Build()
}
