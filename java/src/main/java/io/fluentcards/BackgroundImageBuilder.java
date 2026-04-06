package io.fluentcards;

import java.util.*;

public class BackgroundImageBuilder {
    private final LinkedHashMap<String, Object> data;

    public BackgroundImageBuilder() {
        data = new LinkedHashMap<>();
    }

    public BackgroundImageBuilder withUrl(String url) {
        data.put("url", url);
        return this;
    }

    public BackgroundImageBuilder withFillMode(BackgroundImageFillMode fillMode) {
        data.put("fillMode", fillMode.getValue());
        return this;
    }

    public BackgroundImageBuilder withHorizontalAlignment(HorizontalAlignment alignment) {
        data.put("horizontalAlignment", alignment.getValue());
        return this;
    }

    public BackgroundImageBuilder withVerticalAlignment(VerticalAlignment alignment) {
        data.put("verticalAlignment", alignment.getValue());
        return this;
    }

    public Map<String, Object> build() {
        return data;
    }
}
