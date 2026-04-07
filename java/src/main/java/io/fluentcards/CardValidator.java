package io.fluentcards;

import java.net.URI;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

/**
 * Validates Adaptive Cards for structural and semantic issues.
 */
public final class CardValidator {
    private CardValidator() {}

    private static final Set<String> KNOWN_VERSIONS = AdaptiveCardBuilder.KNOWN_VERSIONS;

    private static final Map<String, Integer> ELEMENT_VERSIONS = Map.ofEntries(
        Map.entry("TextBlock", 0), Map.entry("Image", 0),
        Map.entry("Container", 0), Map.entry("ColumnSet", 0),
        Map.entry("FactSet", 0), Map.entry("ImageSet", 0),
        Map.entry("Column", 0), Map.entry("Fact", 0), Map.entry("Choice", 0),
        Map.entry("Action.OpenUrl", 0), Map.entry("Action.Submit", 0),
        Map.entry("Action.ShowCard", 0),
        Map.entry("Input.Text", 0), Map.entry("Input.Number", 0),
        Map.entry("Input.Date", 0), Map.entry("Input.Time", 0),
        Map.entry("Input.Toggle", 0), Map.entry("Input.ChoiceSet", 0),
        Map.entry("Media", 1),
        Map.entry("RichTextBlock", 2), Map.entry("ActionSet", 2),
        Map.entry("Action.ToggleVisibility", 2),
        Map.entry("Action.Execute", 4),
        Map.entry("Table", 5)
    );

    private static final Map<String, Integer> CARD_PROPERTY_VERSIONS = Map.of(
        "selectAction", 1,
        "minHeight", 2,
        "verticalContentAlignment", 2,
        "backgroundImage", 2,
        "refresh", 4,
        "authentication", 4,
        "rtl", 5,
        "metadata", 6
    );

    /**
     * Validates an Adaptive Card and returns a list of issues found.
     * The list may be empty if the card is valid.
     */
    public static List<ValidationIssue> validate(Map<String, Object> card) {
        List<ValidationIssue> issues = new ArrayList<>();
        Map<String, Boolean> ids = new HashMap<>();
        validateCard(card, issues, ids);
        String version = getStr(card, "version");
        if (!version.isEmpty() && KNOWN_VERSIONS.contains(version)) {
            validateVersionMismatch(card, version, issues);
        }
        return issues;
    }

    /**
     * Validates the card and throws {@link AdaptiveCardValidationException}
     * if any ERROR-severity issues are found.
     */
    public static void validateAndThrow(Map<String, Object> card) {
        List<ValidationIssue> issues = validate(card);
        List<ValidationIssue> errors = issues.stream()
                .filter(i -> i.getSeverity() == ValidationSeverity.ERROR)
                .toList();
        if (!errors.isEmpty()) {
            throw new AdaptiveCardValidationException(errors);
        }
    }

    // ---- helpers ----

    private static void addIssue(List<ValidationIssue> issues,
                                 ValidationSeverity severity,
                                 String path, String code, String message) {
        issues.add(new ValidationIssue(severity, path, code, message));
    }

    private static String getStr(Map<String, Object> map, String key) {
        Object val = map.get(key);
        return val instanceof String s ? s : "";
    }

    @SuppressWarnings("unchecked")
    private static List<Object> getList(Map<String, Object> map, String key) {
        Object val = map.get(key);
        return val instanceof List ? (List<Object>) val : List.of();
    }

    @SuppressWarnings("unchecked")
    private static Map<String, Object> getMap(Object obj) {
        return obj instanceof Map ? (Map<String, Object>) obj : null;
    }

    private static void trackId(String id, String path,
                                List<ValidationIssue> issues,
                                Map<String, Boolean> ids) {
        if (id.isEmpty()) return;
        if (Boolean.TRUE.equals(ids.get(id))) {
            addIssue(issues, ValidationSeverity.WARNING, path, "DUPLICATE_ID",
                    String.format("Duplicate id '%s' found. Element IDs should be unique within a card.", id));
        } else {
            ids.put(id, true);
        }
    }

    private static boolean isAbsoluteUrl(String rawUrl) {
        try {
            URI uri = new URI(rawUrl);
            return uri.getScheme() != null && !uri.getScheme().isEmpty()
                    && uri.getHost() != null && !uri.getHost().isEmpty();
        } catch (Exception e) {
            return false;
        }
    }

    // ---- card-level validation ----

    private static void validateCard(Map<String, Object> card,
                                     List<ValidationIssue> issues,
                                     Map<String, Boolean> ids) {
        String schema = getStr(card, "$schema");
        if (schema.isEmpty()) {
            addIssue(issues, ValidationSeverity.WARNING, "$schema", "MISSING_SCHEMA",
                    "The '$schema' property is missing. While optional, including it enables better tooling support.");
        }

        String version = getStr(card, "version");
        if (version.isEmpty()) {
            addIssue(issues, ValidationSeverity.ERROR, "version", "MISSING_VERSION",
                    "The 'version' property is required. Use a value like '1.5' to specify the schema version.");
        } else if (!KNOWN_VERSIONS.contains(version)) {
            addIssue(issues, ValidationSeverity.WARNING, "version", "UNKNOWN_VERSION",
                    String.format("The version '%s' is not a known Adaptive Cards version. Known versions: 1.0, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6.", version));
        }

        List<Object> body = getList(card, "body");
        List<Object> actions = getList(card, "actions");
        if (body.isEmpty() && actions.isEmpty()) {
            addIssue(issues, ValidationSeverity.WARNING, "", "EMPTY_CARD",
                    "The card has no body elements and no actions. It will render as empty.");
        }

        if (!body.isEmpty()) {
            validateElements(body, issues, "body", ids);
        }
        if (!actions.isEmpty()) {
            validateActions(actions, issues, "actions", ids);
            if (actions.size() > 5) {
                addIssue(issues, ValidationSeverity.WARNING, "actions", "TOO_MANY_ACTIONS",
                        String.format("The card has %d actions. Some hosts limit the number of visible actions to 5.", actions.size()));
            }
        }
        validateSelectAction(card.get("selectAction"), issues, "selectAction");
    }

    // ---- element validation ----

    private static void validateElements(List<Object> elements,
                                         List<ValidationIssue> issues,
                                         String path,
                                         Map<String, Boolean> ids) {
        for (int i = 0; i < elements.size(); i++) {
            Map<String, Object> elMap = getMap(elements.get(i));
            if (elMap == null) continue;
            String elPath = String.format("%s[%d]", path, i);
            String id = getStr(elMap, "id");
            if (!id.isEmpty()) {
                trackId(id, elPath, issues, ids);
            }
            validateElement(elMap, issues, elPath, ids);
        }
    }

    @SuppressWarnings("unchecked")
    private static void validateElement(Map<String, Object> element,
                                        List<ValidationIssue> issues,
                                        String path,
                                        Map<String, Boolean> ids) {
        String t = getStr(element, "type");
        switch (t) {
            case "TextBlock" -> {
                if (getStr(element, "text").isEmpty()) {
                    addIssue(issues, ValidationSeverity.ERROR, path + ".text", "MISSING_TEXT",
                            "TextBlock is missing the required 'text' property.");
                }
            }
            case "Image" -> {
                String rawUrl = getStr(element, "url");
                if (rawUrl.isEmpty()) {
                    addIssue(issues, ValidationSeverity.ERROR, path + ".url", "MISSING_IMAGE_URL",
                            "Image element is missing the required 'url' property.");
                } else if (!isAbsoluteUrl(rawUrl)) {
                    addIssue(issues, ValidationSeverity.WARNING, path + ".url", "INVALID_IMAGE_URL",
                            String.format("Image URL '%s' is not a valid absolute URL.", rawUrl));
                }
                validateSelectAction(element.get("selectAction"), issues, path + ".selectAction");
            }
            case "ImageSet" -> {
                List<Object> images = getList(element, "images");
                if (images.isEmpty()) {
                    addIssue(issues, ValidationSeverity.ERROR, path + ".images", "MISSING_IMAGES",
                            "ImageSet is missing the required 'images' property.");
                } else {
                    for (int i = 0; i < images.size(); i++) {
                        Map<String, Object> imgMap = getMap(images.get(i));
                        if (imgMap == null) continue;
                        if (getStr(imgMap, "url").isEmpty()) {
                            addIssue(issues, ValidationSeverity.ERROR,
                                    String.format("%s.images[%d].url", path, i), "MISSING_IMAGE_URL",
                                    "Image element is missing the required 'url' property.");
                        }
                    }
                }
            }
            case "FactSet" -> {
                if (getList(element, "facts").isEmpty()) {
                    addIssue(issues, ValidationSeverity.ERROR, path + ".facts", "MISSING_FACTS",
                            "FactSet is missing the required 'facts' property.");
                }
            }
            case "ActionSet" -> {
                List<Object> actions = getList(element, "actions");
                if (actions.isEmpty()) {
                    addIssue(issues, ValidationSeverity.ERROR, path + ".actions", "MISSING_ACTIONSET_ACTIONS",
                            "ActionSet is missing the required 'actions' property.");
                } else {
                    validateActions(actions, issues, path + ".actions", ids);
                }
            }
            case "RichTextBlock" -> {
                if (getList(element, "inlines").isEmpty()) {
                    addIssue(issues, ValidationSeverity.ERROR, path + ".inlines", "MISSING_INLINES",
                            "RichTextBlock is missing the required 'inlines' property.");
                }
            }
            case "Media" -> {
                if (getList(element, "sources").isEmpty()) {
                    addIssue(issues, ValidationSeverity.ERROR, path + ".sources", "MISSING_MEDIA_SOURCES",
                            "Media is missing the required 'sources' property.");
                }
            }
            case "Input.Text", "Input.Number", "Input.Date", "Input.Time",
                 "Input.Toggle", "Input.ChoiceSet" -> {
                String id = getStr(element, "id");
                if (id.isEmpty()) {
                    addIssue(issues, ValidationSeverity.ERROR, path + ".id", "MISSING_INPUT_ID",
                            "Input element is missing the required 'id' property. Inputs cannot be submitted without an id.");
                }
                validateInputElement(element, issues, path);
            }
            case "Container" -> {
                List<Object> items = getList(element, "items");
                if (items.isEmpty()) {
                    addIssue(issues, ValidationSeverity.WARNING, path + ".items", "EMPTY_CONTAINER",
                            "Container has no items. It will render as empty.");
                } else {
                    validateElements(items, issues, path + ".items", ids);
                }
                validateSelectAction(element.get("selectAction"), issues, path + ".selectAction");
            }
            case "ColumnSet" -> {
                List<Object> columns = getList(element, "columns");
                for (int i = 0; i < columns.size(); i++) {
                    Map<String, Object> colMap = getMap(columns.get(i));
                    if (colMap == null) continue;
                    String colPath = String.format("%s.columns[%d]", path, i);
                    String colId = getStr(colMap, "id");
                    if (!colId.isEmpty()) {
                        trackId(colId, colPath, issues, ids);
                    }
                    List<Object> items = getList(colMap, "items");
                    if (!items.isEmpty()) {
                        validateElements(items, issues, colPath + ".items", ids);
                    }
                    validateSelectAction(colMap.get("selectAction"), issues, colPath + ".selectAction");
                }
                validateSelectAction(element.get("selectAction"), issues, path + ".selectAction");
            }
            case "Table" -> {
                List<Object> rows = getList(element, "rows");
                for (int r = 0; r < rows.size(); r++) {
                    Map<String, Object> rowMap = getMap(rows.get(r));
                    if (rowMap == null) continue;
                    List<Object> cells = getList(rowMap, "cells");
                    for (int c = 0; c < cells.size(); c++) {
                        Map<String, Object> cellMap = getMap(cells.get(c));
                        if (cellMap == null) continue;
                        List<Object> items = getList(cellMap, "items");
                        if (!items.isEmpty()) {
                            validateElements(items, issues,
                                    String.format("%s.rows[%d].cells[%d].items", path, r, c), ids);
                        }
                        validateSelectAction(cellMap.get("selectAction"), issues,
                                String.format("%s.rows[%d].cells[%d].selectAction", path, r, c));
                    }
                }
            }
        }
    }

    // ---- input validation ----

    private static void validateInputElement(Map<String, Object> element,
                                             List<ValidationIssue> issues,
                                             String path) {
        String t = getStr(element, "type");
        switch (t) {
            case "Input.Number" -> {
                Object min = element.get("min");
                Object max = element.get("max");
                if (min != null && max != null) {
                    double minF = toDouble(min);
                    double maxF = toDouble(max);
                    if (!Double.isNaN(minF) && !Double.isNaN(maxF) && minF > maxF) {
                        addIssue(issues, ValidationSeverity.ERROR, path, "MIN_GREATER_THAN_MAX",
                                String.format("Input.Number 'min' (%s) is greater than 'max' (%s).", min, max));
                    }
                }
            }
            case "Input.Date", "Input.Time" -> {
                String minS = getStr(element, "min");
                String maxS = getStr(element, "max");
                if (!minS.isEmpty() && !maxS.isEmpty() && minS.compareTo(maxS) > 0) {
                    addIssue(issues, ValidationSeverity.ERROR, path, "MIN_GREATER_THAN_MAX",
                            String.format("%s 'min' (%s) is greater than 'max' (%s).", t, minS, maxS));
                }
            }
            case "Input.Toggle" -> {
                if (getStr(element, "title").isEmpty()) {
                    addIssue(issues, ValidationSeverity.ERROR, path + ".title", "MISSING_TOGGLE_TITLE",
                            "Input.Toggle is missing the required 'title' property.");
                }
            }
        }
    }

    private static double toDouble(Object v) {
        if (v instanceof Number n) return n.doubleValue();
        return Double.NaN;
    }

    // ---- select-action validation ----

    private static void validateSelectAction(Object action,
                                             List<ValidationIssue> issues,
                                             String path) {
        if (action == null) return;
        Map<String, Object> actionMap = getMap(action);
        if (actionMap == null) return;
        if ("Action.ShowCard".equals(getStr(actionMap, "type"))) {
            addIssue(issues, ValidationSeverity.ERROR, path, "INVALID_SELECT_ACTION",
                    "Action.ShowCard is not allowed as a selectAction. Use Action.OpenUrl, Action.Submit, " +
                            "Action.Execute, or Action.ToggleVisibility.");
        }
    }

    // ---- action validation ----

    private static void validateActions(List<Object> actions,
                                        List<ValidationIssue> issues,
                                        String path,
                                        Map<String, Boolean> ids) {
        for (int i = 0; i < actions.size(); i++) {
            Map<String, Object> actionMap = getMap(actions.get(i));
            if (actionMap == null) continue;
            String actionPath = String.format("%s[%d]", path, i);
            String id = getStr(actionMap, "id");
            if (!id.isEmpty()) {
                trackId(id, actionPath, issues, ids);
            }
            validateAction(actionMap, issues, actionPath, ids);
        }
    }

    private static void validateAction(Map<String, Object> action,
                                       List<ValidationIssue> issues,
                                       String path,
                                       Map<String, Boolean> ids) {
        String t = getStr(action, "type");
        switch (t) {
            case "Action.OpenUrl" -> {
                String rawUrl = getStr(action, "url");
                if (rawUrl.isEmpty()) {
                    addIssue(issues, ValidationSeverity.ERROR, path + ".url", "MISSING_ACTION_URL",
                            "Action.OpenUrl is missing the required 'url' property.");
                } else if (!isAbsoluteUrl(rawUrl)) {
                    addIssue(issues, ValidationSeverity.WARNING, path + ".url", "INVALID_ACTION_URL",
                            String.format("Action.OpenUrl URL '%s' is not a valid absolute URL.", rawUrl));
                }
            }
            case "Action.ShowCard" -> {
                Object card = action.get("card");
                if (card == null) {
                    addIssue(issues, ValidationSeverity.ERROR, path + ".card", "MISSING_SHOWCARD",
                            "Action.ShowCard is missing the required 'card' property.");
                } else {
                    Map<String, Object> cardMap = getMap(card);
                    if (cardMap != null) {
                        validateCard(cardMap, issues, ids);
                    }
                }
            }
            case "Action.ToggleVisibility" -> {
                if (getList(action, "targetElements").isEmpty()) {
                    addIssue(issues, ValidationSeverity.ERROR, path + ".targetElements", "MISSING_TARGET_ELEMENTS",
                            "Action.ToggleVisibility is missing the required 'targetElements' property.");
                }
            }
        }
    }

    // ---- version-aware validation ----

    private static int versionMinor(String v) {
        String[] parts = v.split("\\.");
        if (parts.length > 1) {
            int n = 0;
            for (char c : parts[1].toCharArray()) {
                if (c >= '0' && c <= '9') {
                    n = n * 10 + (c - '0');
                }
            }
            return n;
        }
        return 0;
    }

    private static void versionMismatch(List<ValidationIssue> issues,
                                        String path, String featureName,
                                        String requiredVersion, String cardVersion) {
        addIssue(issues, ValidationSeverity.WARNING, path, "VERSION_MISMATCH",
                String.format("'%s' requires Adaptive Cards %s but card version is %s.",
                        featureName, requiredVersion, cardVersion));
    }

    private static void checkElementVersion(String typeStr, String cardVersion,
                                            List<ValidationIssue> issues, String path) {
        Integer required = ELEMENT_VERSIONS.get(typeStr);
        if (required == null) return;
        if (required > versionMinor(cardVersion)) {
            versionMismatch(issues, path, typeStr,
                    String.format("1.%d", required), cardVersion);
        }
    }

    private static void checkCardPropertyVersion(String prop, String cardVersion,
                                                 List<ValidationIssue> issues) {
        Integer required = CARD_PROPERTY_VERSIONS.get(prop);
        if (required == null) return;
        if (required > versionMinor(cardVersion)) {
            versionMismatch(issues, prop, prop,
                    String.format("1.%d", required), cardVersion);
        }
    }

    private static void validateVersionMismatch(Map<String, Object> card,
                                                String cardVersion,
                                                List<ValidationIssue> issues) {
        for (String prop : new String[]{
                "selectAction", "minHeight", "verticalContentAlignment",
                "backgroundImage", "refresh", "authentication", "metadata"}) {
            if (card.get(prop) != null) {
                checkCardPropertyVersion(prop, cardVersion, issues);
            }
        }
        if (card.get("rtl") != null) {
            checkCardPropertyVersion("rtl", cardVersion, issues);
        }
        List<Object> body = getList(card, "body");
        if (!body.isEmpty()) {
            checkElementVersionsInList(body, cardVersion, issues, "body");
        }
        List<Object> actions = getList(card, "actions");
        if (!actions.isEmpty()) {
            checkActionVersionsInList(actions, cardVersion, issues, "actions");
        }
    }

    private static void checkElementVersionsInList(List<Object> elements,
                                                   String cardVersion,
                                                   List<ValidationIssue> issues,
                                                   String path) {
        for (int i = 0; i < elements.size(); i++) {
            Map<String, Object> elMap = getMap(elements.get(i));
            if (elMap == null) continue;
            String p = String.format("%s[%d]", path, i);
            String t = getStr(elMap, "type");
            checkElementVersion(t, cardVersion, issues, p);
            switch (t) {
                case "Container" -> {
                    List<Object> items = getList(elMap, "items");
                    if (!items.isEmpty()) {
                        checkElementVersionsInList(items, cardVersion, issues, p + ".items");
                    }
                }
                case "ColumnSet" -> {
                    List<Object> cols = getList(elMap, "columns");
                    for (int ci = 0; ci < cols.size(); ci++) {
                        Map<String, Object> colMap = getMap(cols.get(ci));
                        if (colMap == null) continue;
                        List<Object> items = getList(colMap, "items");
                        if (!items.isEmpty()) {
                            checkElementVersionsInList(items, cardVersion, issues,
                                    String.format("%s.columns[%d].items", p, ci));
                        }
                    }
                }
                case "ActionSet" -> {
                    List<Object> actions = getList(elMap, "actions");
                    if (!actions.isEmpty()) {
                        checkActionVersionsInList(actions, cardVersion, issues, p + ".actions");
                    }
                }
                case "Table" -> {
                    List<Object> rows = getList(elMap, "rows");
                    for (int r = 0; r < rows.size(); r++) {
                        Map<String, Object> rowMap = getMap(rows.get(r));
                        if (rowMap == null) continue;
                        List<Object> cells = getList(rowMap, "cells");
                        for (int c = 0; c < cells.size(); c++) {
                            Map<String, Object> cellMap = getMap(cells.get(c));
                            if (cellMap == null) continue;
                            List<Object> items = getList(cellMap, "items");
                            if (!items.isEmpty()) {
                                checkElementVersionsInList(items, cardVersion, issues,
                                        String.format("%s.rows[%d].cells[%d].items", p, r, c));
                            }
                        }
                    }
                }
            }
        }
    }

    @SuppressWarnings("unchecked")
    private static void checkActionVersionsInList(List<Object> actions,
                                                  String cardVersion,
                                                  List<ValidationIssue> issues,
                                                  String path) {
        for (int i = 0; i < actions.size(); i++) {
            Map<String, Object> actionMap = getMap(actions.get(i));
            if (actionMap == null) continue;
            String p = String.format("%s[%d]", path, i);
            String t = getStr(actionMap, "type");
            checkElementVersion(t, cardVersion, issues, p);
            if ("Action.ShowCard".equals(t)) {
                Map<String, Object> inner = getMap(actionMap.get("card"));
                if (inner != null) {
                    List<Object> body = getList(inner, "body");
                    if (!body.isEmpty()) {
                        checkElementVersionsInList(body, cardVersion, issues, p + ".card.body");
                    }
                    List<Object> innerActions = getList(inner, "actions");
                    if (!innerActions.isEmpty()) {
                        checkActionVersionsInList(innerActions, cardVersion, issues, p + ".card.actions");
                    }
                }
            }
        }
    }
}
