package io.fluentcards;

import java.util.*;

public class MediaBuilder {
    private final LinkedHashMap<String, Object> data;

    public MediaBuilder() {
        data = new LinkedHashMap<>();
        data.put("type", "Media");
        data.put("sources", new ArrayList<>());
    }

    public MediaBuilder withId(String id) {
        data.put("id", id);
        return this;
    }

    public MediaBuilder withPoster(String poster) {
        data.put("poster", poster);
        return this;
    }

    public MediaBuilder withAltText(String altText) {
        data.put("altText", altText);
        return this;
    }

    public MediaBuilder withSpacing(Spacing spacing) {
        data.put("spacing", spacing.getValue());
        return this;
    }

    @SuppressWarnings("unchecked")
    public MediaBuilder addSource(String url, String mimeType) {
        LinkedHashMap<String, Object> source = new LinkedHashMap<>();
        source.put("mimeType", mimeType);
        source.put("url", url);
        ((List<Object>) data.get("sources")).add(source);
        return this;
    }

    @SuppressWarnings("unchecked")
    public MediaBuilder addSourceMap(Map<String, Object> source) {
        ((List<Object>) data.get("sources")).add(source);
        return this;
    }

    public Map<String, Object> build() {
        return data;
    }
}
