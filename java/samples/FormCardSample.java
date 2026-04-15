package io.fluentcards.samples;

import io.fluentcards.*;
import java.util.Map;

public class FormCardSample {
    public static void run() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
            .withVersion("1.5")
            .addTextBlock(tb -> tb
                .withText("Registration Form")
                .withSize(TextSize.LARGE)
                .withWeight(TextWeight.BOLDER))
            .addInputText(i -> i
                .withId("name")
                .withLabel("Full Name")
                .withPlaceholder("Enter your full name")
                .withIsRequired(true)
                .withErrorMessage("Name is required"))
            .addInputNumber(i -> i
                .withId("age")
                .withLabel("Age")
                .withMin(0)
                .withMax(120))
            .addInputDate(i -> i
                .withId("birthDate")
                .withLabel("Date of Birth"))
            .addInputToggle(i -> i
                .withId("newsletter")
                .withTitle("Subscribe to newsletter"))
            .addInputChoiceSet(i -> i
                .withId("department")
                .withLabel("Department")
                .withStyle(ChoiceInputStyle.COMPACT)
                .addChoice("Engineering", "eng")
                .addChoice("Marketing", "mkt")
                .addChoice("Sales", "sales"))
            .addAction(a -> a
                .submit("Submit")
                .withStyle(ActionStyle.POSITIVE))
            .build();

        System.out.println("=== Form Card ===");
        System.out.println(CardSerializer.toJson(card));
    }
}
