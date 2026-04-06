package io.fluentcards.samples;

import io.fluentcards.*;
import java.util.Map;

public class BasicCardSample {
    public static void run() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
            .withVersion("1.5")
            .addTextBlock(tb -> tb
                .withText("Hello, FluentCards!")
                .withSize(TextSize.LARGE)
                .withWeight(TextWeight.BOLDER)
                .withWrap(true))
            .addImage(img -> img
                .withUrl("https://adaptivecards.io/content/cats/1.png")
                .withAltText("A cute cat")
                .withSize(ImageSize.MEDIUM))
            .addTextBlock(tb -> tb
                .withText("This is a basic Adaptive Card built with the FluentCards Java library.")
                .withWrap(true)
                .withIsSubtle(true))
            .build();

        System.out.println("=== Basic Card ===");
        System.out.println(CardSerializer.toJson(card));
    }
}
