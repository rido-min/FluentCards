package io.fluentcards;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.reflect.TypeToken;
import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Utility class for serializing and deserializing Adaptive Cards to/from JSON.
 */
public final class CardSerializer {
    private CardSerializer() {}

    private static final Type MAP_TYPE = new TypeToken<Map<String, Object>>() {}.getType();

    /**
     * Serializes an Adaptive Card to a JSON string with 2-space indentation.
     */
    public static String toJson(Map<String, Object> card) {
        return toJson(card, 2);
    }

    /**
     * Serializes an Adaptive Card to a JSON string with the given indentation width.
     * Pass 0 for compact output.
     */
    public static String toJson(Map<String, Object> card, int indent) {
        Map<String, Object> clean = stripNulls(card);
        if (indent > 0) {
            Gson gson = new GsonBuilder()
                .setPrettyPrinting()
                .disableHtmlEscaping()
                .create();
            return gson.toJson(clean, MAP_TYPE);
        }
        Gson gson = new GsonBuilder()
            .disableHtmlEscaping()
            .create();
        return gson.toJson(clean, MAP_TYPE);
    }

    /**
     * Parses a JSON string and returns the Adaptive Card if the root object
     * has type "AdaptiveCard". Returns null if parsing fails or the root type is wrong.
     */
    public static Map<String, Object> fromJson(String json) {
        try {
            Gson gson = new Gson();
            Map<String, Object> parsed = gson.fromJson(json, MAP_TYPE);
            if (parsed == null) return null;
            Object type = parsed.get("type");
            if (!"AdaptiveCard".equals(type)) return null;
            return parsed;
        } catch (Exception e) {
            return null;
        }
    }

    @SuppressWarnings("unchecked")
    private static Map<String, Object> stripNulls(Map<String, Object> map) {
        var result = new LinkedHashMap<String, Object>();
        for (var entry : map.entrySet()) {
            Object value = entry.getValue();
            if (value == null) continue;
            if (value instanceof Map) {
                result.put(entry.getKey(), stripNulls((Map<String, Object>) value));
            } else if (value instanceof List) {
                result.put(entry.getKey(), stripNullsList((List<Object>) value));
            } else {
                result.put(entry.getKey(), value);
            }
        }
        return result;
    }

    @SuppressWarnings("unchecked")
    private static List<Object> stripNullsList(List<Object> list) {
        var result = new ArrayList<Object>(list.size());
        for (Object item : list) {
            if (item instanceof Map) {
                result.add(stripNulls((Map<String, Object>) item));
            } else if (item instanceof List) {
                result.add(stripNullsList((List<Object>) item));
            } else {
                result.add(item);
            }
        }
        return result;
    }
}
