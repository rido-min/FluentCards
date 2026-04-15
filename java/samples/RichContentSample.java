package io.fluentcards.samples;

import io.fluentcards.*;
import java.util.Map;

public class RichContentSample {
    public static void run() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
            .withVersion("1.5")
            .addRichTextBlock(rtb -> rtb
                .addTextRun(tr -> tr
                    .withText("Welcome ")
                    .withSize(TextSize.LARGE))
                .addTextRun(tr -> tr
                    .withText("to FluentCards!")
                    .withSize(TextSize.LARGE)
                    .withWeight(TextWeight.BOLDER)
                    .withColor(TextColor.ACCENT)))
            .addImageSet(imgSet -> imgSet
                .withImageSize(ImageSize.MEDIUM)
                .addImage(img -> img
                    .withUrl("https://adaptivecards.io/content/cats/1.png"))
                .addImage(img -> img
                    .withUrl("https://adaptivecards.io/content/cats/2.png"))
                .addImage(img -> img
                    .withUrl("https://adaptivecards.io/content/cats/3.png")))
            .addMedia(media -> media
                .addSource("https://example.com/video.mp4", "video/mp4")
                .withPoster("https://example.com/poster.jpg")
                .withAltText("Getting started with FluentCards"))
            .addActionSet(actionSet -> actionSet
                .addAction(a -> a
                    .openUrl("https://adaptivecards.io")
                    .withTitle("Learn More"))
                .addAction(a -> a
                    .submit("Get Started")
                    .withStyle(ActionStyle.POSITIVE)))
            .build();

        System.out.println("=== Rich Content Card ===");
        System.out.println(CardSerializer.toJson(card));
    }
}
