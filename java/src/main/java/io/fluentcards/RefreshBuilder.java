package io.fluentcards;

import java.util.*;
import java.util.function.Consumer;

public class RefreshBuilder {
    private final LinkedHashMap<String, Object> data;

    public RefreshBuilder() {
        data = new LinkedHashMap<>();
    }

    public RefreshBuilder withAction(Consumer<ActionBuilder> configure) {
        ActionBuilder builder = new ActionBuilder();
        configure.accept(builder);
        data.put("action", builder.build());
        return this;
    }

    @SuppressWarnings("unchecked")
    public RefreshBuilder addUserId(String userId) {
        List<Object> userIds = (List<Object>) data.get("userIds");
        if (userIds == null) {
            userIds = new ArrayList<>();
            data.put("userIds", userIds);
        }
        userIds.add(userId);
        return this;
    }

    public RefreshBuilder withExpires(String expires) {
        data.put("expires", expires);
        return this;
    }

    public Map<String, Object> build() {
        return data;
    }
}
