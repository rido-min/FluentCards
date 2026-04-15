package io.fluentcards.samples;

import io.fluentcards.*;
import java.util.Map;

public class PeoplePickerSample {
    public static void run() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
            .withVersion("1.6")
            .addTextBlock(tb -> tb
                .withText("People Picker")
                .withSize(TextSize.LARGE)
                .withWeight(TextWeight.BOLDER))
            .addInputChoiceSet(i -> i
                .withId("people-picker")
                .withLabel("Select users in the whole organization")
                .withIsMultiSelect(true)
                .withStyle(ChoiceInputStyle.FILTERED)
                .withChoicesData("graph.microsoft.com/users"))
            .addAction(a -> a
                .submit("Submit"))
            .build();

        System.out.println("=== People Picker Card ===");
        System.out.println(CardSerializer.toJson(card));
    }
}
