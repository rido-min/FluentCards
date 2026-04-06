package fluentcards

import (
	"fmt"
	"net/url"
	"strings"
)

// ValidationIssue represents a single validation finding for an Adaptive Card.
type ValidationIssue struct {
	Severity ValidationSeverity
	Path     string
	Code     string
	Message  string
}

// AdaptiveCardValidationError is returned by ValidateAndPanic when error-severity issues are found.
type AdaptiveCardValidationError struct {
	Issues []ValidationIssue
}

func (e *AdaptiveCardValidationError) Error() string {
	errors := make([]ValidationIssue, 0, len(e.Issues))
	for _, i := range e.Issues {
		if i.Severity == ValidationSeverityError {
			errors = append(errors, i)
		}
	}
	if len(errors) == 1 {
		return "Adaptive Card validation failed: " + errors[0].Message
	}
	lines := make([]string, len(errors))
	for i, e := range errors {
		lines[i] = fmt.Sprintf("  - [%s] %s", e.Path, e.Message)
	}
	return fmt.Sprintf("Adaptive Card validation failed with %d errors:\n%s",
		len(errors), strings.Join(lines, "\n"))
}

// Validate checks an Adaptive Card for structural and semantic issues.
// It returns a slice of ValidationIssue values (may be empty if the card is valid).
func Validate(card Card) []ValidationIssue {
	var issues []ValidationIssue
	ids := map[string]bool{}
	validateCard(card, &issues, ids)
	if version, ok := card["version"].(string); ok && knownVersions[version] {
		validateVersionMismatch(card, version, &issues)
	}
	return issues
}

// ValidateAndPanic validates the card and panics with an *AdaptiveCardValidationError
// if any Error-severity issues are found.
func ValidateAndPanic(card Card) {
	issues := Validate(card)
	var errors []ValidationIssue
	for _, i := range issues {
		if i.Severity == ValidationSeverityError {
			errors = append(errors, i)
		}
	}
	if len(errors) > 0 {
		panic(&AdaptiveCardValidationError{Issues: errors})
	}
}

func addIssue(issues *[]ValidationIssue, severity ValidationSeverity, path, code, message string) {
	*issues = append(*issues, ValidationIssue{
		Severity: severity,
		Path:     path,
		Code:     code,
		Message:  message,
	})
}

func trackID(idVal, path string, issues *[]ValidationIssue, ids map[string]bool) {
	if idVal == "" {
		return
	}
	if ids[idVal] {
		addIssue(issues, ValidationSeverityWarning, path, "DUPLICATE_ID",
			fmt.Sprintf("Duplicate id '%s' found. Element IDs should be unique within a card.", idVal))
	} else {
		ids[idVal] = true
	}
}

func isAbsoluteURL(rawURL string) bool {
	u, err := url.Parse(rawURL)
	return err == nil && u.Scheme != "" && u.Host != ""
}

func validateCard(card map[string]any, issues *[]ValidationIssue, ids map[string]bool) {
	schema, _ := card["$schema"].(string)
	if schema == "" {
		addIssue(issues, ValidationSeverityWarning, "$schema", "MISSING_SCHEMA",
			"The '$schema' property is missing. While optional, including it enables better tooling support.")
	}
	version, _ := card["version"].(string)
	if version == "" {
		addIssue(issues, ValidationSeverityError, "version", "MISSING_VERSION",
			"The 'version' property is required. Use a value like '1.5' to specify the schema version.")
	} else if !knownVersions[version] {
		known := "1.0, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6"
		addIssue(issues, ValidationSeverityWarning, "version", "UNKNOWN_VERSION",
			fmt.Sprintf("The version '%s' is not a known Adaptive Cards version. Known versions: %s.", version, known))
	}

	body, _ := card["body"].([]any)
	actions, _ := card["actions"].([]any)
	if len(body) == 0 && len(actions) == 0 {
		addIssue(issues, ValidationSeverityWarning, "", "EMPTY_CARD",
			"The card has no body elements and no actions. It will render as empty.")
	}

	if len(body) > 0 {
		validateElements(body, issues, "body", ids)
	}
	if len(actions) > 0 {
		validateActions(actions, issues, "actions", ids)
		if len(actions) > 5 {
			addIssue(issues, ValidationSeverityWarning, "actions", "TOO_MANY_ACTIONS",
				fmt.Sprintf("The card has %d actions. Some hosts limit the number of visible actions to 5.", len(actions)))
		}
	}
	validateSelectAction(card["selectAction"], issues, "selectAction")
}

func validateElements(elements []any, issues *[]ValidationIssue, path string, ids map[string]bool) {
	for i, el := range elements {
		elMap, ok := el.(map[string]any)
		if !ok {
			continue
		}
		elPath := fmt.Sprintf("%s[%d]", path, i)
		if id, _ := elMap["id"].(string); id != "" {
			trackID(id, elPath, issues, ids)
		}
		validateElement(elMap, issues, elPath, ids)
	}
}

func validateElement(element map[string]any, issues *[]ValidationIssue, path string, ids map[string]bool) {
	t, _ := element["type"].(string)
	switch t {
	case "TextBlock":
		if text, _ := element["text"].(string); text == "" {
			addIssue(issues, ValidationSeverityError, path+".text", "MISSING_TEXT",
				"TextBlock is missing the required 'text' property.")
		}
	case "Image":
		rawURL, _ := element["url"].(string)
		if rawURL == "" {
			addIssue(issues, ValidationSeverityError, path+".url", "MISSING_IMAGE_URL",
				"Image element is missing the required 'url' property.")
		} else if !isAbsoluteURL(rawURL) {
			addIssue(issues, ValidationSeverityWarning, path+".url", "INVALID_IMAGE_URL",
				fmt.Sprintf("Image URL '%s' is not a valid absolute URL.", rawURL))
		}
		validateSelectAction(element["selectAction"], issues, path+".selectAction")
	case "ImageSet":
		images, _ := element["images"].([]any)
		if len(images) == 0 {
			addIssue(issues, ValidationSeverityError, path+".images", "MISSING_IMAGES",
				"ImageSet is missing the required 'images' property.")
		} else {
			for i, img := range images {
				imgMap, _ := img.(map[string]any)
				if imgMap == nil {
					continue
				}
				if u, _ := imgMap["url"].(string); u == "" {
					addIssue(issues, ValidationSeverityError,
						fmt.Sprintf("%s.images[%d].url", path, i), "MISSING_IMAGE_URL",
						"Image element is missing the required 'url' property.")
				}
			}
		}
	case "FactSet":
		if facts, _ := element["facts"].([]any); len(facts) == 0 {
			addIssue(issues, ValidationSeverityError, path+".facts", "MISSING_FACTS",
				"FactSet is missing the required 'facts' property.")
		}
	case "ActionSet":
		actions, _ := element["actions"].([]any)
		if len(actions) == 0 {
			addIssue(issues, ValidationSeverityError, path+".actions", "MISSING_ACTIONSET_ACTIONS",
				"ActionSet is missing the required 'actions' property.")
		} else {
			validateActions(actions, issues, path+".actions", ids)
		}
	case "RichTextBlock":
		if inlines, _ := element["inlines"].([]any); len(inlines) == 0 {
			addIssue(issues, ValidationSeverityError, path+".inlines", "MISSING_INLINES",
				"RichTextBlock is missing the required 'inlines' property.")
		}
	case "Media":
		if sources, _ := element["sources"].([]any); len(sources) == 0 {
			addIssue(issues, ValidationSeverityError, path+".sources", "MISSING_MEDIA_SOURCES",
				"Media is missing the required 'sources' property.")
		}
	case "Input.Text", "Input.Number", "Input.Date", "Input.Time", "Input.Toggle", "Input.ChoiceSet":
		id, _ := element["id"].(string)
		if id == "" {
			addIssue(issues, ValidationSeverityError, path+".id", "MISSING_INPUT_ID",
				"Input element is missing the required 'id' property. Inputs cannot be submitted without an id.")
		} else {
			trackID(id, path, issues, ids)
		}
		validateInputElement(element, issues, path)
	case "Container":
		items, _ := element["items"].([]any)
		if len(items) == 0 {
			addIssue(issues, ValidationSeverityWarning, path+".items", "EMPTY_CONTAINER",
				"Container has no items. It will render as empty.")
		} else {
			validateElements(items, issues, path+".items", ids)
		}
		validateSelectAction(element["selectAction"], issues, path+".selectAction")
	case "ColumnSet":
		columns, _ := element["columns"].([]any)
		for i, col := range columns {
			colMap, _ := col.(map[string]any)
			if colMap == nil {
				continue
			}
			colPath := fmt.Sprintf("%s.columns[%d]", path, i)
			if id, _ := colMap["id"].(string); id != "" {
				trackID(id, colPath, issues, ids)
			}
			if items, _ := colMap["items"].([]any); len(items) > 0 {
				validateElements(items, issues, colPath+".items", ids)
			}
			validateSelectAction(colMap["selectAction"], issues, colPath+".selectAction")
		}
		validateSelectAction(element["selectAction"], issues, path+".selectAction")
	case "Table":
		rows, _ := element["rows"].([]any)
		for r, row := range rows {
			rowMap, _ := row.(map[string]any)
			if rowMap == nil {
				continue
			}
			cells, _ := rowMap["cells"].([]any)
			for c, cell := range cells {
				cellMap, _ := cell.(map[string]any)
				if cellMap == nil {
					continue
				}
				if items, _ := cellMap["items"].([]any); len(items) > 0 {
					validateElements(items, issues, fmt.Sprintf("%s.rows[%d].cells[%d].items", path, r, c), ids)
				}
				validateSelectAction(cellMap["selectAction"], issues,
					fmt.Sprintf("%s.rows[%d].cells[%d].selectAction", path, r, c))
			}
		}
	}
}

func validateInputElement(element map[string]any, issues *[]ValidationIssue, path string) {
	t, _ := element["type"].(string)
	switch t {
	case "Input.Number":
		min := element["min"]
		max := element["max"]
		if min != nil && max != nil {
			minF, minOk := toFloat64(min)
			maxF, maxOk := toFloat64(max)
			if minOk && maxOk && minF > maxF {
				addIssue(issues, ValidationSeverityError, path, "MIN_GREATER_THAN_MAX",
					fmt.Sprintf("Input.Number 'min' (%v) is greater than 'max' (%v).", min, max))
			}
		}
	case "Input.Date", "Input.Time":
		minS, _ := element["min"].(string)
		maxS, _ := element["max"].(string)
		if minS != "" && maxS != "" && minS > maxS {
			addIssue(issues, ValidationSeverityError, path, "MIN_GREATER_THAN_MAX",
				fmt.Sprintf("%s 'min' (%s) is greater than 'max' (%s).", t, minS, maxS))
		}
	case "Input.Toggle":
		if title, _ := element["title"].(string); title == "" {
			addIssue(issues, ValidationSeverityError, path+".title", "MISSING_TOGGLE_TITLE",
				"Input.Toggle is missing the required 'title' property.")
		}
	}
}

func toFloat64(v any) (float64, bool) {
	switch n := v.(type) {
	case float64:
		return n, true
	case float32:
		return float64(n), true
	case int:
		return float64(n), true
	case int64:
		return float64(n), true
	default:
		return 0, false
	}
}

func validateSelectAction(action any, issues *[]ValidationIssue, path string) {
	if action == nil {
		return
	}
	actionMap, ok := action.(map[string]any)
	if !ok {
		return
	}
	if t, _ := actionMap["type"].(string); t == "Action.ShowCard" {
		addIssue(issues, ValidationSeverityError, path, "INVALID_SELECT_ACTION",
			"Action.ShowCard is not allowed as a selectAction. Use Action.OpenUrl, Action.Submit, "+
				"Action.Execute, or Action.ToggleVisibility.")
	}
}

func validateActions(actions []any, issues *[]ValidationIssue, path string, ids map[string]bool) {
	for i, action := range actions {
		actionMap, ok := action.(map[string]any)
		if !ok {
			continue
		}
		actionPath := fmt.Sprintf("%s[%d]", path, i)
		if id, _ := actionMap["id"].(string); id != "" {
			trackID(id, actionPath, issues, ids)
		}
		validateAction(actionMap, issues, actionPath, ids)
	}
}

func validateAction(action map[string]any, issues *[]ValidationIssue, path string, ids map[string]bool) {
	t, _ := action["type"].(string)
	switch t {
	case "Action.OpenUrl":
		rawURL, _ := action["url"].(string)
		if rawURL == "" {
			addIssue(issues, ValidationSeverityError, path+".url", "MISSING_ACTION_URL",
				"Action.OpenUrl is missing the required 'url' property.")
		} else if !isAbsoluteURL(rawURL) {
			addIssue(issues, ValidationSeverityWarning, path+".url", "INVALID_ACTION_URL",
				fmt.Sprintf("Action.OpenUrl URL '%s' is not a valid absolute URL.", rawURL))
		}
	case "Action.ShowCard":
		card := action["card"]
		if card == nil {
			addIssue(issues, ValidationSeverityError, path+".card", "MISSING_SHOWCARD",
				"Action.ShowCard is missing the required 'card' property.")
		} else if cardMap, ok := card.(map[string]any); ok {
			validateCard(cardMap, issues, ids)
		}
	case "Action.ToggleVisibility":
		if targets, _ := action["targetElements"].([]any); len(targets) == 0 {
			addIssue(issues, ValidationSeverityError, path+".targetElements", "MISSING_TARGET_ELEMENTS",
				"Action.ToggleVisibility is missing the required 'targetElements' property.")
		}
	}
}

// Version-aware validation

var elementVersions = map[string]int{
	"TextBlock": 0, "Image": 0, "Container": 0, "ColumnSet": 0,
	"FactSet": 0, "ImageSet": 0, "Column": 0, "Fact": 0, "Choice": 0,
	"Action.OpenUrl": 0, "Action.Submit": 0, "Action.ShowCard": 0,
	"Input.Text": 0, "Input.Number": 0, "Input.Date": 0,
	"Input.Time": 0, "Input.Toggle": 0, "Input.ChoiceSet": 0,
	"Media":                    1,
	"RichTextBlock":             2,
	"ActionSet":                 2,
	"Action.ToggleVisibility":   2,
	"Action.Execute":            4,
	"Table":                     5,
}

var cardPropertyVersions = map[string]int{
	"selectAction":             1,
	"minHeight":                2,
	"verticalContentAlignment": 2,
	"backgroundImage":          2,
	"refresh":                  4,
	"authentication":           4,
	"rtl":                      5,
	"metadata":                 6,
}

func versionMinor(v string) int {
	parts := strings.Split(v, ".")
	if len(parts) > 1 {
		n := 0
		for _, c := range parts[1] {
			if c >= '0' && c <= '9' {
				n = n*10 + int(c-'0')
			}
		}
		return n
	}
	return 0
}

func versionMismatch(issues *[]ValidationIssue, path, featureName, requiredVersion, cardVersion string) {
	addIssue(issues, ValidationSeverityWarning, path, "VERSION_MISMATCH",
		fmt.Sprintf("'%s' requires Adaptive Cards %s but card version is %s.", featureName, requiredVersion, cardVersion))
}

func checkElementVersion(typeStr, cardVersion string, issues *[]ValidationIssue, path string) {
	required, ok := elementVersions[typeStr]
	if !ok {
		return
	}
	if required > versionMinor(cardVersion) {
		versionMismatch(issues, path, typeStr, fmt.Sprintf("1.%d", required), cardVersion)
	}
}

func checkCardPropertyVersion(prop, cardVersion string, issues *[]ValidationIssue) {
	required, ok := cardPropertyVersions[prop]
	if !ok {
		return
	}
	if required > versionMinor(cardVersion) {
		versionMismatch(issues, prop, prop, fmt.Sprintf("1.%d", required), cardVersion)
	}
}

func validateVersionMismatch(card map[string]any, cardVersion string, issues *[]ValidationIssue) {
	for _, prop := range []string{"selectAction", "minHeight", "verticalContentAlignment",
		"backgroundImage", "refresh", "authentication", "metadata"} {
		if card[prop] != nil {
			checkCardPropertyVersion(prop, cardVersion, issues)
		}
	}
	if card["rtl"] != nil {
		checkCardPropertyVersion("rtl", cardVersion, issues)
	}
	if body, _ := card["body"].([]any); len(body) > 0 {
		checkElementVersionsInList(body, cardVersion, issues, "body")
	}
	if actions, _ := card["actions"].([]any); len(actions) > 0 {
		checkActionVersionsInList(actions, cardVersion, issues, "actions")
	}
}

func checkElementVersionsInList(elements []any, cardVersion string, issues *[]ValidationIssue, path string) {
	for i, el := range elements {
		elMap, ok := el.(map[string]any)
		if !ok {
			continue
		}
		p := fmt.Sprintf("%s[%d]", path, i)
		t, _ := elMap["type"].(string)
		checkElementVersion(t, cardVersion, issues, p)
		switch t {
		case "Container":
			if items, _ := elMap["items"].([]any); len(items) > 0 {
				checkElementVersionsInList(items, cardVersion, issues, p+".items")
			}
		case "ColumnSet":
			cols, _ := elMap["columns"].([]any)
			for ci, col := range cols {
				colMap, _ := col.(map[string]any)
				if colMap == nil {
					continue
				}
				if items, _ := colMap["items"].([]any); len(items) > 0 {
					checkElementVersionsInList(items, cardVersion, issues,
						fmt.Sprintf("%s.columns[%d].items", p, ci))
				}
			}
		case "ActionSet":
			if actions, _ := elMap["actions"].([]any); len(actions) > 0 {
				checkActionVersionsInList(actions, cardVersion, issues, p+".actions")
			}
		case "Table":
			rows, _ := elMap["rows"].([]any)
			for r, row := range rows {
				rowMap, _ := row.(map[string]any)
				if rowMap == nil {
					continue
				}
				cells, _ := rowMap["cells"].([]any)
				for c, cell := range cells {
					cellMap, _ := cell.(map[string]any)
					if cellMap == nil {
						continue
					}
					if items, _ := cellMap["items"].([]any); len(items) > 0 {
						checkElementVersionsInList(items, cardVersion, issues,
							fmt.Sprintf("%s.rows[%d].cells[%d].items", p, r, c))
					}
				}
			}
		}
	}
}

func checkActionVersionsInList(actions []any, cardVersion string, issues *[]ValidationIssue, path string) {
	for i, action := range actions {
		actionMap, ok := action.(map[string]any)
		if !ok {
			continue
		}
		p := fmt.Sprintf("%s[%d]", path, i)
		t, _ := actionMap["type"].(string)
		checkElementVersion(t, cardVersion, issues, p)
		if t == "Action.ShowCard" {
			if inner, ok := actionMap["card"].(map[string]any); ok {
				if body, _ := inner["body"].([]any); len(body) > 0 {
					checkElementVersionsInList(body, cardVersion, issues, p+".card.body")
				}
				if innerActions, _ := inner["actions"].([]any); len(innerActions) > 0 {
					checkActionVersionsInList(innerActions, cardVersion, issues, p+".card.actions")
				}
			}
		}
	}
}
