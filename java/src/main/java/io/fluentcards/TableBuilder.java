package io.fluentcards;

import java.util.*;

public class TableBuilder {
    private final LinkedHashMap<String, Object> data;

    public TableBuilder() {
        data = new LinkedHashMap<>();
        data.put("type", "Table");
    }

    public TableBuilder withId(String id) {
        data.put("id", id);
        return this;
    }

    public TableBuilder withFirstRowAsHeader(boolean firstRowAsHeader) {
        data.put("firstRowAsHeader", firstRowAsHeader);
        return this;
    }

    public TableBuilder withShowGridLines(boolean showGridLines) {
        data.put("showGridLines", showGridLines);
        return this;
    }

    public TableBuilder withGridStyle(ContainerStyle style) {
        data.put("gridStyle", style.getValue());
        return this;
    }

    public TableBuilder withHorizontalCellContentAlignment(HorizontalAlignment alignment) {
        data.put("horizontalCellContentAlignment", alignment.getValue());
        return this;
    }

    public TableBuilder withVerticalCellContentAlignment(VerticalAlignment alignment) {
        data.put("verticalCellContentAlignment", alignment.getValue());
        return this;
    }

    public TableBuilder withSpacing(Spacing spacing) {
        data.put("spacing", spacing.getValue());
        return this;
    }

    @SuppressWarnings("unchecked")
    public TableBuilder addColumn(Map<String, Object> column) {
        List<Object> columns = (List<Object>) data.get("columns");
        if (columns == null) {
            columns = new ArrayList<>();
            data.put("columns", columns);
        }
        columns.add(column);
        return this;
    }

    @SuppressWarnings("unchecked")
    public TableBuilder addRow(Map<String, Object> row) {
        List<Object> rows = (List<Object>) data.get("rows");
        if (rows == null) {
            rows = new ArrayList<>();
            data.put("rows", rows);
        }
        rows.add(row);
        return this;
    }

    public Map<String, Object> build() {
        return data;
    }
}
