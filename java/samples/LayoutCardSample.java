package io.fluentcards.samples;

import io.fluentcards.*;
import java.util.Map;

public class LayoutCardSample {
    public static void run() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
            .withVersion("1.5")
            .addTextBlock(tb -> tb
                .withText("Product Information")
                .withSize(TextSize.LARGE)
                .withWeight(TextWeight.BOLDER))
            .addColumnSet(cs -> cs
                .addColumn(col -> col
                    .withWidth("auto")
                    .addImage(img -> img
                        .withUrl("https://adaptivecards.io/content/adaptive-card-50.png")
                        .withSize(ImageSize.MEDIUM)))
                .addColumn(col -> col
                    .withWidth("stretch")
                    .addTextBlock(tb -> tb
                        .withText("Adaptive Cards SDK")
                        .withWeight(TextWeight.BOLDER))
                    .addTextBlock(tb -> tb
                        .withText("Create platform-agnostic UI snippets")
                        .withWrap(true))
                    .addTextBlock(tb -> tb
                        .withText("$49.99")
                        .withColor(TextColor.GOOD)
                        .withSize(TextSize.LARGE))))
            .addContainer(c -> c
                .withStyle(ContainerStyle.EMPHASIS)
                .addTextBlock(tb -> tb
                    .withText("Important Notice")
                    .withSize(TextSize.MEDIUM)
                    .withWeight(TextWeight.BOLDER))
                .addTextBlock(tb -> tb
                    .withText("This is an emphasized container with important information.")
                    .withWrap(true)))
            .addFactSet(fs -> fs
                .addFact("Date", "December 15, 2024")
                .addFact("Time", "2:00 PM - 3:00 PM")
                .addFact("Location", "Conference Room A"))
            .addAction(a -> a
                .submit("Add to Cart")
                .withStyle(ActionStyle.POSITIVE))
            .build();

        System.out.println("=== Layout Card ===");
        System.out.println(CardSerializer.toJson(card));
    }
}
