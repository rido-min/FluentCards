package io.fluentcards;

import java.util.*;
import java.util.function.Consumer;

public class ImageSetBuilder {
    private final LinkedHashMap<String, Object> data;

    public ImageSetBuilder() {
        data = new LinkedHashMap<>();
        data.put("type", "ImageSet");
        data.put("images", new ArrayList<>());
    }

    public ImageSetBuilder withId(String id) {
        data.put("id", id);
        return this;
    }

    public ImageSetBuilder withImageSize(ImageSize size) {
        data.put("imageSize", size.getValue());
        return this;
    }

    public ImageSetBuilder withSpacing(Spacing spacing) {
        data.put("spacing", spacing.getValue());
        return this;
    }

    @SuppressWarnings("unchecked")
    public ImageSetBuilder addImage(Consumer<ImageBuilder> configure) {
        ImageBuilder builder = new ImageBuilder();
        configure.accept(builder);
        ((List<Object>) data.get("images")).add(builder.build());
        return this;
    }

    public Map<String, Object> build() {
        return data;
    }
}
