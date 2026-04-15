package io.fluentcards;

import java.util.*;
import java.util.function.Consumer;

public class AdaptiveCardBuilder {
    static final Map<String, String> SCHEMA_URLS;
    static final Set<String> KNOWN_VERSIONS = Set.of("1.0", "1.1", "1.2", "1.3", "1.4", "1.5", "1.6");

    static {
        Map<String, String> urls = new LinkedHashMap<>();
        urls.put("1.0", "https://adaptivecards.io/schemas/1.0.0/adaptive-card.json");
        urls.put("1.1", "https://adaptivecards.io/schemas/1.1.0/adaptive-card.json");
        urls.put("1.2", "https://adaptivecards.io/schemas/1.2.0/adaptive-card.json");
        urls.put("1.3", "https://adaptivecards.io/schemas/1.3.0/adaptive-card.json");
        urls.put("1.4", "https://adaptivecards.io/schemas/1.4.0/adaptive-card.json");
        urls.put("1.5", "https://adaptivecards.io/schemas/1.5.0/adaptive-card.json");
        urls.put("1.6", "https://adaptivecards.io/schemas/1.6.0/adaptive-card.json");
        SCHEMA_URLS = Collections.unmodifiableMap(urls);
    }

    private final LinkedHashMap<String, Object> data;

    public AdaptiveCardBuilder() {
        data = new LinkedHashMap<>();
        data.put("type", "AdaptiveCard");
        data.put("version", "1.5");
        data.put("$schema", SCHEMA_URLS.get("1.5"));
    }

    public static AdaptiveCardBuilder create() {
        return new AdaptiveCardBuilder();
    }

    public AdaptiveCardBuilder withVersion(String version) {
        data.put("version", version);
        String schema = SCHEMA_URLS.get(version);
        if (schema != null) {
            data.put("$schema", schema);
        }
        return this;
    }

    public AdaptiveCardBuilder withSchema(String schema) {
        data.put("$schema", schema);
        return this;
    }

    public AdaptiveCardBuilder withFallbackText(String fallbackText) {
        data.put("fallbackText", fallbackText);
        return this;
    }

    public AdaptiveCardBuilder withSpeak(String speak) {
        data.put("speak", speak);
        return this;
    }

    public AdaptiveCardBuilder withLang(String lang) {
        data.put("lang", lang);
        return this;
    }

    public AdaptiveCardBuilder withRtl(boolean rtl) {
        data.put("rtl", rtl);
        return this;
    }

    public AdaptiveCardBuilder withMinHeight(String minHeight) {
        data.put("minHeight", minHeight);
        return this;
    }

    public AdaptiveCardBuilder withVerticalContentAlignment(VerticalAlignment alignment) {
        data.put("verticalContentAlignment", alignment.getValue());
        return this;
    }

    public AdaptiveCardBuilder withBackgroundImage(Consumer<BackgroundImageBuilder> configure) {
        BackgroundImageBuilder builder = new BackgroundImageBuilder();
        configure.accept(builder);
        data.put("backgroundImage", builder.build());
        return this;
    }

    public AdaptiveCardBuilder withSelectAction(Consumer<ActionBuilder> configure) {
        ActionBuilder builder = new ActionBuilder();
        configure.accept(builder);
        data.put("selectAction", builder.build());
        return this;
    }

    public AdaptiveCardBuilder withMetadata(String webUrl) {
        LinkedHashMap<String, Object> metadata = new LinkedHashMap<>();
        metadata.put("webUrl", webUrl);
        data.put("metadata", metadata);
        return this;
    }

    // Body element adders

    public AdaptiveCardBuilder addTextBlock(Consumer<TextBlockBuilder> configure) {
        TextBlockBuilder builder = new TextBlockBuilder();
        configure.accept(builder);
        pushBody(builder.build());
        return this;
    }

    public AdaptiveCardBuilder addImage(Consumer<ImageBuilder> configure) {
        ImageBuilder builder = new ImageBuilder();
        configure.accept(builder);
        pushBody(builder.build());
        return this;
    }

    public AdaptiveCardBuilder addContainer(Consumer<ContainerBuilder> configure) {
        ContainerBuilder builder = new ContainerBuilder();
        configure.accept(builder);
        pushBody(builder.build());
        return this;
    }

    public AdaptiveCardBuilder addColumnSet(Consumer<ColumnSetBuilder> configure) {
        ColumnSetBuilder builder = new ColumnSetBuilder();
        configure.accept(builder);
        pushBody(builder.build());
        return this;
    }

    public AdaptiveCardBuilder addFactSet(Consumer<FactSetBuilder> configure) {
        FactSetBuilder builder = new FactSetBuilder();
        configure.accept(builder);
        pushBody(builder.build());
        return this;
    }

    public AdaptiveCardBuilder addRichTextBlock(Consumer<RichTextBlockBuilder> configure) {
        RichTextBlockBuilder builder = new RichTextBlockBuilder();
        configure.accept(builder);
        pushBody(builder.build());
        return this;
    }

    public AdaptiveCardBuilder addActionSet(Consumer<ActionSetBuilder> configure) {
        ActionSetBuilder builder = new ActionSetBuilder();
        configure.accept(builder);
        pushBody(builder.build());
        return this;
    }

    public AdaptiveCardBuilder addMedia(Consumer<MediaBuilder> configure) {
        MediaBuilder builder = new MediaBuilder();
        configure.accept(builder);
        pushBody(builder.build());
        return this;
    }

    public AdaptiveCardBuilder addImageSet(Consumer<ImageSetBuilder> configure) {
        ImageSetBuilder builder = new ImageSetBuilder();
        configure.accept(builder);
        pushBody(builder.build());
        return this;
    }

    public AdaptiveCardBuilder addTable(Consumer<TableBuilder> configure) {
        TableBuilder builder = new TableBuilder();
        configure.accept(builder);
        pushBody(builder.build());
        return this;
    }

    public AdaptiveCardBuilder addElement(Map<String, Object> element) {
        pushBody(element);
        return this;
    }

    // Input adders

    public AdaptiveCardBuilder addInputText(Consumer<InputTextBuilder> configure) {
        InputTextBuilder builder = new InputTextBuilder();
        configure.accept(builder);
        pushBody(builder.build());
        return this;
    }

    public AdaptiveCardBuilder addInputNumber(Consumer<InputNumberBuilder> configure) {
        InputNumberBuilder builder = new InputNumberBuilder();
        configure.accept(builder);
        pushBody(builder.build());
        return this;
    }

    public AdaptiveCardBuilder addInputDate(Consumer<InputDateBuilder> configure) {
        InputDateBuilder builder = new InputDateBuilder();
        configure.accept(builder);
        pushBody(builder.build());
        return this;
    }

    public AdaptiveCardBuilder addInputTime(Consumer<InputTimeBuilder> configure) {
        InputTimeBuilder builder = new InputTimeBuilder();
        configure.accept(builder);
        pushBody(builder.build());
        return this;
    }

    public AdaptiveCardBuilder addInputToggle(Consumer<InputToggleBuilder> configure) {
        InputToggleBuilder builder = new InputToggleBuilder();
        configure.accept(builder);
        pushBody(builder.build());
        return this;
    }

    public AdaptiveCardBuilder addInputChoiceSet(Consumer<InputChoiceSetBuilder> configure) {
        InputChoiceSetBuilder builder = new InputChoiceSetBuilder();
        configure.accept(builder);
        pushBody(builder.build());
        return this;
    }

    // Actions

    @SuppressWarnings("unchecked")
    public AdaptiveCardBuilder addAction(Consumer<ActionBuilder> configure) {
        ActionBuilder builder = new ActionBuilder();
        configure.accept(builder);
        List<Object> actions = (List<Object>) data.get("actions");
        if (actions == null) {
            actions = new ArrayList<>();
            data.put("actions", actions);
        }
        actions.add(builder.build());
        return this;
    }

    // Refresh & Authentication

    public AdaptiveCardBuilder withRefresh(Consumer<RefreshBuilder> configure) {
        RefreshBuilder builder = new RefreshBuilder();
        configure.accept(builder);
        data.put("refresh", builder.build());
        return this;
    }

    public AdaptiveCardBuilder withAuthentication(Consumer<AuthenticationBuilder> configure) {
        AuthenticationBuilder builder = new AuthenticationBuilder();
        configure.accept(builder);
        data.put("authentication", builder.build());
        return this;
    }

    public Map<String, Object> build() {
        return data;
    }

    @SuppressWarnings("unchecked")
    private void pushBody(Map<String, Object> element) {
        List<Object> body = (List<Object>) data.get("body");
        if (body == null) {
            body = new ArrayList<>();
            data.put("body", body);
        }
        body.add(element);
    }
}
