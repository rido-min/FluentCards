package io.fluentcards;

import java.util.*;

public class AuthenticationBuilder {
    private final LinkedHashMap<String, Object> data;

    public AuthenticationBuilder() {
        data = new LinkedHashMap<>();
    }

    public AuthenticationBuilder withText(String text) {
        data.put("text", text);
        return this;
    }

    public AuthenticationBuilder withConnectionName(String connectionName) {
        data.put("connectionName", connectionName);
        return this;
    }

    public AuthenticationBuilder withTokenExchangeResource(Map<String, Object> resource) {
        data.put("tokenExchangeResource", resource);
        return this;
    }

    @SuppressWarnings("unchecked")
    public AuthenticationBuilder addButton(Map<String, Object> button) {
        List<Object> buttons = (List<Object>) data.get("buttons");
        if (buttons == null) {
            buttons = new ArrayList<>();
            data.put("buttons", buttons);
        }
        buttons.add(button);
        return this;
    }

    public Map<String, Object> build() {
        return data;
    }
}
