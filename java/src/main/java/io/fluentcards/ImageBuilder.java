package io.fluentcards;

import java.util.*;
import java.util.function.Consumer;

public class ImageBuilder {
    private final LinkedHashMap<String, Object> data;

    public ImageBuilder() {
        data = new LinkedHashMap<>();
        data.put("type", "Image");
    }

    public ImageBuilder withId(String id) {
        data.put("id", id);
        return this;
    }

    public ImageBuilder withUrl(String url) {
        data.put("url", url);
        return this;
    }

    public ImageBuilder withAltText(String altText) {
        data.put("altText", altText);
        return this;
    }

    public ImageBuilder withSize(ImageSize size) {
        data.put("size", size.getValue());
        return this;
    }

    public ImageBuilder withStyle(ImageStyle style) {
        data.put("style", style.getValue());
        return this;
    }

    public ImageBuilder withWidth(String width) {
        data.put("width", width);
        return this;
    }

    public ImageBuilder withHeight(String height) {
        data.put("height", height);
        return this;
    }

    public ImageBuilder withHorizontalAlignment(HorizontalAlignment alignment) {
        data.put("horizontalAlignment", alignment.getValue());
        return this;
    }

    public ImageBuilder withBackgroundColor(String color) {
        data.put("backgroundColor", color);
        return this;
    }

    public ImageBuilder withSpacing(Spacing spacing) {
        data.put("spacing", spacing.getValue());
        return this;
    }

    public ImageBuilder withSeparator(boolean separator) {
        data.put("separator", separator);
        return this;
    }

    public ImageBuilder withSelectAction(Consumer<ActionBuilder> configure) {
        ActionBuilder builder = new ActionBuilder();
        configure.accept(builder);
        data.put("selectAction", builder.build());
        return this;
    }

    public Map<String, Object> build() {
        return data;
    }
}
