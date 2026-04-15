package io.fluentcards;

import java.util.*;
import java.util.function.Consumer;

public class ActionBuilder {
    private LinkedHashMap<String, Object> data;

    public ActionBuilder() {
        data = null;
    }

    // Type-setting methods

    public ActionBuilder openUrl(String url) {
        data = new LinkedHashMap<>();
        data.put("type", "Action.OpenUrl");
        data.put("url", url);
        return this;
    }

    public ActionBuilder submit(String... title) {
        data = new LinkedHashMap<>();
        data.put("type", "Action.Submit");
        if (title.length > 0) {
            data.put("title", title[0]);
        }
        return this;
    }

    public ActionBuilder showCard(String... title) {
        data = new LinkedHashMap<>();
        data.put("type", "Action.ShowCard");
        if (title.length > 0) {
            data.put("title", title[0]);
        }
        return this;
    }

    public ActionBuilder toggleVisibility(String... title) {
        data = new LinkedHashMap<>();
        data.put("type", "Action.ToggleVisibility");
        if (title.length > 0) {
            data.put("title", title[0]);
        }
        return this;
    }

    public ActionBuilder execute(String... title) {
        data = new LinkedHashMap<>();
        data.put("type", "Action.Execute");
        if (title.length > 0) {
            data.put("title", title[0]);
        }
        return this;
    }

    // Config methods

    public ActionBuilder withId(String id) {
        if (data != null) data.put("id", id);
        return this;
    }

    public ActionBuilder withTitle(String title) {
        if (data != null) data.put("title", title);
        return this;
    }

    public ActionBuilder withIconUrl(String iconUrl) {
        if (data != null) data.put("iconUrl", iconUrl);
        return this;
    }

    public ActionBuilder withStyle(ActionStyle style) {
        if (data != null) data.put("style", style.getValue());
        return this;
    }

    public ActionBuilder withIsEnabled(boolean isEnabled) {
        if (data != null) data.put("isEnabled", isEnabled);
        return this;
    }

    public ActionBuilder withTooltip(String tooltip) {
        if (data != null) data.put("tooltip", tooltip);
        return this;
    }

    // Data methods

    public ActionBuilder withData(Object dataValue) {
        if (data != null) {
            String type = (String) data.get("type");
            if ("Action.Submit".equals(type) || "Action.Execute".equals(type)) {
                data.put("data", dataValue);
            }
        }
        return this;
    }

    public ActionBuilder withAssociatedInputs(AssociatedInputs associatedInputs) {
        if (data != null) {
            String type = (String) data.get("type");
            if ("Action.Submit".equals(type) || "Action.Execute".equals(type)) {
                data.put("associatedInputs", associatedInputs.getValue());
            }
        }
        return this;
    }

    public ActionBuilder withVerb(String verb) {
        if (data != null) {
            String type = (String) data.get("type");
            if ("Action.Execute".equals(type)) {
                data.put("verb", verb);
            }
        }
        return this;
    }

    public ActionBuilder withCard(Map<String, Object> card) {
        if (data != null) {
            String type = (String) data.get("type");
            if ("Action.ShowCard".equals(type)) {
                data.put("card", card);
            }
        }
        return this;
    }

    @SuppressWarnings("unchecked")
    public ActionBuilder addTargetElement(String elementId, Boolean isVisible) {
        if (data != null) {
            List<Object> targets = (List<Object>) data.get("targetElements");
            if (targets == null) {
                targets = new ArrayList<>();
                data.put("targetElements", targets);
            }
            if (isVisible == null) {
                targets.add(elementId);
            } else {
                LinkedHashMap<String, Object> target = new LinkedHashMap<>();
                target.put("elementId", elementId);
                target.put("isVisible", isVisible);
                targets.add(target);
            }
        }
        return this;
    }

    public Map<String, Object> build() {
        if (data == null) {
            throw new IllegalStateException("Action type not set. Call openUrl(), submit(), showCard(), toggleVisibility(), or execute() before build().");
        }
        return data;
    }
}
