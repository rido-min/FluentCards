import Foundation

/// ValidationIssue represents a single validation finding for an Adaptive Card.
public struct ValidationIssue {
    public let severity: ValidationSeverity
    public let path: String
    public let code: String
    public let message: String
}

/// AdaptiveCardValidationError is returned by validateAndThrow when error-severity issues are found.
public struct AdaptiveCardValidationError: Error {
    public let issues: [ValidationIssue]

    public var description: String {
        if issues.count == 1 {
            return "Adaptive Card validation failed: \(issues[0].message)"
        }
        let lines = issues.map { "  - [\($0.path)] \($0.message)" }
        return "Adaptive Card validation failed with \(issues.count) errors:\n\(lines.joined(separator: "\n"))"
    }
}

/// Validates a card, returning all issues (may be empty if the card is valid).
public func validate(_ card: Card) -> [ValidationIssue] {
    var issues: [ValidationIssue] = []
    var ids: Set<String> = []
    validateCard(card, issues: &issues, ids: &ids)
    if let version = card["version"] as? String, knownVersions.contains(version) {
        validateVersionMismatch(card, cardVersion: version, issues: &issues)
    }
    return issues
}

/// Validates the card and throws AdaptiveCardValidationError if any Error-severity issues are found.
public func validateAndThrow(_ card: Card) throws {
    let allIssues = validate(card)
    let errors = allIssues.filter { $0.severity == .error }
    if !errors.isEmpty {
        throw AdaptiveCardValidationError(issues: errors)
    }
}

// MARK: - Internal helpers

private func addIssue(_ issues: inout [ValidationIssue], severity: ValidationSeverity, path: String, code: String, message: String) {
    issues.append(ValidationIssue(severity: severity, path: path, code: code, message: message))
}

private func trackID(_ idVal: String, path: String, issues: inout [ValidationIssue], ids: inout Set<String>) {
    guard !idVal.isEmpty else { return }
    if ids.contains(idVal) {
        addIssue(&issues, severity: .warning, path: path, code: "DUPLICATE_ID",
                 message: "Duplicate id '\(idVal)' found. Element IDs should be unique within a card.")
    } else {
        ids.insert(idVal)
    }
}

private func isAbsoluteURL(_ rawURL: String) -> Bool {
    guard let url = URL(string: rawURL) else { return false }
    return url.scheme != nil && url.host != nil
}

private func validateCard(_ card: [String: Any], issues: inout [ValidationIssue], ids: inout Set<String>) {
    let schema = card["$schema"] as? String ?? ""
    if schema.isEmpty {
        addIssue(&issues, severity: .warning, path: "$schema", code: "MISSING_SCHEMA",
                 message: "The '$schema' property is missing. While optional, including it enables better tooling support.")
    }

    let version = card["version"] as? String ?? ""
    if version.isEmpty {
        addIssue(&issues, severity: .error, path: "version", code: "MISSING_VERSION",
                 message: "The 'version' property is required. Use a value like '1.5' to specify the schema version.")
    } else if !knownVersions.contains(version) {
        let known = "1.0, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6"
        addIssue(&issues, severity: .warning, path: "version", code: "UNKNOWN_VERSION",
                 message: "The version '\(version)' is not a known Adaptive Cards version. Known versions: \(known).")
    }

    let body = card["body"] as? [Any] ?? []
    let actions = card["actions"] as? [Any] ?? []
    if body.isEmpty && actions.isEmpty {
        addIssue(&issues, severity: .warning, path: "", code: "EMPTY_CARD",
                 message: "The card has no body elements and no actions. It will render as empty.")
    }

    if !body.isEmpty {
        validateElements(body, issues: &issues, path: "body", ids: &ids)
    }
    if !actions.isEmpty {
        validateActions(actions, issues: &issues, path: "actions", ids: &ids)
        if actions.count > 5 {
            addIssue(&issues, severity: .warning, path: "actions", code: "TOO_MANY_ACTIONS",
                     message: "The card has \(actions.count) actions. Some hosts limit the number of visible actions to 5.")
        }
    }
    validateSelectAction(card["selectAction"], issues: &issues, path: "selectAction")
}

private func validateElements(_ elements: [Any], issues: inout [ValidationIssue], path: String, ids: inout Set<String>) {
    for (i, el) in elements.enumerated() {
        guard let elMap = el as? [String: Any] else { continue }
        let elPath = "\(path)[\(i)]"
        if let id = elMap["id"] as? String, !id.isEmpty {
            trackID(id, path: elPath, issues: &issues, ids: &ids)
        }
        validateElement(elMap, issues: &issues, path: elPath, ids: &ids)
    }
}

private func validateElement(_ element: [String: Any], issues: inout [ValidationIssue], path: String, ids: inout Set<String>) {
    let t = element["type"] as? String ?? ""
    switch t {
    case "TextBlock":
        let text = element["text"] as? String ?? ""
        if text.isEmpty {
            addIssue(&issues, severity: .error, path: path + ".text", code: "MISSING_TEXT",
                     message: "TextBlock is missing the required 'text' property.")
        }
    case "Image":
        let rawURL = element["url"] as? String ?? ""
        if rawURL.isEmpty {
            addIssue(&issues, severity: .error, path: path + ".url", code: "MISSING_IMAGE_URL",
                     message: "Image element is missing the required 'url' property.")
        } else if !isAbsoluteURL(rawURL) {
            addIssue(&issues, severity: .warning, path: path + ".url", code: "INVALID_IMAGE_URL",
                     message: "Image URL '\(rawURL)' is not a valid absolute URL.")
        }
        validateSelectAction(element["selectAction"], issues: &issues, path: path + ".selectAction")
    case "ImageSet":
        let images = element["images"] as? [Any] ?? []
        if images.isEmpty {
            addIssue(&issues, severity: .error, path: path + ".images", code: "MISSING_IMAGES",
                     message: "ImageSet is missing the required 'images' property.")
        } else {
            for (i, img) in images.enumerated() {
                guard let imgMap = img as? [String: Any] else { continue }
                let u = imgMap["url"] as? String ?? ""
                if u.isEmpty {
                    addIssue(&issues, severity: .error, path: "\(path).images[\(i)].url", code: "MISSING_IMAGE_URL",
                             message: "Image element is missing the required 'url' property.")
                }
            }
        }
    case "FactSet":
        let facts = element["facts"] as? [Any] ?? []
        if facts.isEmpty {
            addIssue(&issues, severity: .error, path: path + ".facts", code: "MISSING_FACTS",
                     message: "FactSet is missing the required 'facts' property.")
        }
    case "ActionSet":
        let actions = element["actions"] as? [Any] ?? []
        if actions.isEmpty {
            addIssue(&issues, severity: .error, path: path + ".actions", code: "MISSING_ACTIONSET_ACTIONS",
                     message: "ActionSet is missing the required 'actions' property.")
        } else {
            validateActions(actions, issues: &issues, path: path + ".actions", ids: &ids)
        }
    case "RichTextBlock":
        let inlines = element["inlines"] as? [Any] ?? []
        if inlines.isEmpty {
            addIssue(&issues, severity: .error, path: path + ".inlines", code: "MISSING_INLINES",
                     message: "RichTextBlock is missing the required 'inlines' property.")
        }
    case "Media":
        let sources = element["sources"] as? [Any] ?? []
        if sources.isEmpty {
            addIssue(&issues, severity: .error, path: path + ".sources", code: "MISSING_MEDIA_SOURCES",
                     message: "Media is missing the required 'sources' property.")
        }
    case "Input.Text", "Input.Number", "Input.Date", "Input.Time", "Input.Toggle", "Input.ChoiceSet":
        let id = element["id"] as? String ?? ""
        if id.isEmpty {
            addIssue(&issues, severity: .error, path: path + ".id", code: "MISSING_INPUT_ID",
                     message: "Input element is missing the required 'id' property. Inputs cannot be submitted without an id.")
        } else {
            trackID(id, path: path, issues: &issues, ids: &ids)
        }
        validateInputElement(element, issues: &issues, path: path)
    case "Container":
        let items = element["items"] as? [Any] ?? []
        if items.isEmpty {
            addIssue(&issues, severity: .warning, path: path + ".items", code: "EMPTY_CONTAINER",
                     message: "Container has no items. It will render as empty.")
        } else {
            validateElements(items, issues: &issues, path: path + ".items", ids: &ids)
        }
        validateSelectAction(element["selectAction"], issues: &issues, path: path + ".selectAction")
    case "ColumnSet":
        let columns = element["columns"] as? [Any] ?? []
        for (i, col) in columns.enumerated() {
            guard let colMap = col as? [String: Any] else { continue }
            let colPath = "\(path).columns[\(i)]"
            if let id = colMap["id"] as? String, !id.isEmpty {
                trackID(id, path: colPath, issues: &issues, ids: &ids)
            }
            let items = colMap["items"] as? [Any] ?? []
            if !items.isEmpty {
                validateElements(items, issues: &issues, path: colPath + ".items", ids: &ids)
            }
            validateSelectAction(colMap["selectAction"], issues: &issues, path: colPath + ".selectAction")
        }
        validateSelectAction(element["selectAction"], issues: &issues, path: path + ".selectAction")
    case "Table":
        let rows = element["rows"] as? [Any] ?? []
        for (r, row) in rows.enumerated() {
            guard let rowMap = row as? [String: Any] else { continue }
            let cells = rowMap["cells"] as? [Any] ?? []
            for (c, cell) in cells.enumerated() {
                guard let cellMap = cell as? [String: Any] else { continue }
                let items = cellMap["items"] as? [Any] ?? []
                if !items.isEmpty {
                    validateElements(items, issues: &issues, path: "\(path).rows[\(r)].cells[\(c)].items", ids: &ids)
                }
                validateSelectAction(cellMap["selectAction"], issues: &issues,
                                     path: "\(path).rows[\(r)].cells[\(c)].selectAction")
            }
        }
    default:
        break
    }
}

private func validateInputElement(_ element: [String: Any], issues: inout [ValidationIssue], path: String) {
    let t = element["type"] as? String ?? ""
    switch t {
    case "Input.Number":
        let minVal = element["min"]
        let maxVal = element["max"]
        if let mn = minVal, let mx = maxVal {
            if let minF = toDouble(mn), let maxF = toDouble(mx), minF > maxF {
                addIssue(&issues, severity: .error, path: path, code: "MIN_GREATER_THAN_MAX",
                         message: "Input.Number 'min' (\(mn)) is greater than 'max' (\(mx)).")
            }
        }
    case "Input.Date", "Input.Time":
        let minS = element["min"] as? String ?? ""
        let maxS = element["max"] as? String ?? ""
        if !minS.isEmpty && !maxS.isEmpty && minS > maxS {
            addIssue(&issues, severity: .error, path: path, code: "MIN_GREATER_THAN_MAX",
                     message: "\(t) 'min' (\(minS)) is greater than 'max' (\(maxS)).")
        }
    case "Input.Toggle":
        let title = element["title"] as? String ?? ""
        if title.isEmpty {
            addIssue(&issues, severity: .error, path: path + ".title", code: "MISSING_TOGGLE_TITLE",
                     message: "Input.Toggle is missing the required 'title' property.")
        }
    default:
        break
    }
}

private func toDouble(_ v: Any) -> Double? {
    switch v {
    case let n as Double: return n
    case let n as Float: return Double(n)
    case let n as Int: return Double(n)
    case let n as Int64: return Double(n)
    default: return nil
    }
}

private func validateSelectAction(_ action: Any?, issues: inout [ValidationIssue], path: String) {
    guard let action = action,
          let actionMap = action as? [String: Any] else { return }
    if let t = actionMap["type"] as? String, t == "Action.ShowCard" {
        addIssue(&issues, severity: .error, path: path, code: "INVALID_SELECT_ACTION",
                 message: "Action.ShowCard is not allowed as a selectAction. Use Action.OpenUrl, Action.Submit, Action.Execute, or Action.ToggleVisibility.")
    }
}

private func validateActions(_ actions: [Any], issues: inout [ValidationIssue], path: String, ids: inout Set<String>) {
    for (i, action) in actions.enumerated() {
        guard let actionMap = action as? [String: Any] else { continue }
        let actionPath = "\(path)[\(i)]"
        if let id = actionMap["id"] as? String, !id.isEmpty {
            trackID(id, path: actionPath, issues: &issues, ids: &ids)
        }
        validateAction(actionMap, issues: &issues, path: actionPath, ids: &ids)
    }
}

private func validateAction(_ action: [String: Any], issues: inout [ValidationIssue], path: String, ids: inout Set<String>) {
    let t = action["type"] as? String ?? ""
    switch t {
    case "Action.OpenUrl":
        let rawURL = action["url"] as? String ?? ""
        if rawURL.isEmpty {
            addIssue(&issues, severity: .error, path: path + ".url", code: "MISSING_ACTION_URL",
                     message: "Action.OpenUrl is missing the required 'url' property.")
        } else if !isAbsoluteURL(rawURL) {
            addIssue(&issues, severity: .warning, path: path + ".url", code: "INVALID_ACTION_URL",
                     message: "Action.OpenUrl URL '\(rawURL)' is not a valid absolute URL.")
        }
    case "Action.ShowCard":
        if let card = action["card"] {
            if let cardMap = card as? [String: Any] {
                validateCard(cardMap, issues: &issues, ids: &ids)
            }
        } else {
            addIssue(&issues, severity: .error, path: path + ".card", code: "MISSING_SHOWCARD",
                     message: "Action.ShowCard is missing the required 'card' property.")
        }
    case "Action.ToggleVisibility":
        let targets = action["targetElements"] as? [Any] ?? []
        if targets.isEmpty {
            addIssue(&issues, severity: .error, path: path + ".targetElements", code: "MISSING_TARGET_ELEMENTS",
                     message: "Action.ToggleVisibility is missing the required 'targetElements' property.")
        }
    default:
        break
    }
}

// MARK: - Version-aware validation

private let elementVersions: [String: Int] = [
    "TextBlock": 0, "Image": 0, "Container": 0, "ColumnSet": 0,
    "FactSet": 0, "ImageSet": 0, "Column": 0, "Fact": 0, "Choice": 0,
    "Action.OpenUrl": 0, "Action.Submit": 0, "Action.ShowCard": 0,
    "Input.Text": 0, "Input.Number": 0, "Input.Date": 0,
    "Input.Time": 0, "Input.Toggle": 0, "Input.ChoiceSet": 0,
    "Media": 1,
    "RichTextBlock": 2,
    "ActionSet": 2,
    "Action.ToggleVisibility": 2,
    "Action.Execute": 4,
    "Table": 5,
]

private let cardPropertyVersions: [String: Int] = [
    "selectAction": 1,
    "minHeight": 2,
    "verticalContentAlignment": 2,
    "backgroundImage": 2,
    "refresh": 4,
    "authentication": 4,
    "rtl": 5,
    "metadata": 6,
]

private func versionMinor(_ v: String) -> Int {
    let parts = v.split(separator: ".")
    guard parts.count > 1, let n = Int(parts[1]) else { return 0 }
    return n
}

private func versionMismatch(_ issues: inout [ValidationIssue], path: String, featureName: String, requiredVersion: String, cardVersion: String) {
    addIssue(&issues, severity: .warning, path: path, code: "VERSION_MISMATCH",
             message: "'\(featureName)' requires Adaptive Cards \(requiredVersion) but card version is \(cardVersion).")
}

private func checkElementVersion(_ typeStr: String, cardVersion: String, issues: inout [ValidationIssue], path: String) {
    guard let required = elementVersions[typeStr] else { return }
    if required > versionMinor(cardVersion) {
        versionMismatch(&issues, path: path, featureName: typeStr,
                        requiredVersion: "1.\(required)", cardVersion: cardVersion)
    }
}

private func checkCardPropertyVersion(_ prop: String, cardVersion: String, issues: inout [ValidationIssue]) {
    guard let required = cardPropertyVersions[prop] else { return }
    if required > versionMinor(cardVersion) {
        versionMismatch(&issues, path: prop, featureName: prop,
                        requiredVersion: "1.\(required)", cardVersion: cardVersion)
    }
}

private func validateVersionMismatch(_ card: [String: Any], cardVersion: String, issues: inout [ValidationIssue]) {
    for prop in ["selectAction", "minHeight", "verticalContentAlignment",
                 "backgroundImage", "refresh", "authentication", "metadata"] {
        if card[prop] != nil {
            checkCardPropertyVersion(prop, cardVersion: cardVersion, issues: &issues)
        }
    }
    if card["rtl"] != nil {
        checkCardPropertyVersion("rtl", cardVersion: cardVersion, issues: &issues)
    }
    let body = card["body"] as? [Any] ?? []
    if !body.isEmpty {
        checkElementVersionsInList(body, cardVersion: cardVersion, issues: &issues, path: "body")
    }
    let actions = card["actions"] as? [Any] ?? []
    if !actions.isEmpty {
        checkActionVersionsInList(actions, cardVersion: cardVersion, issues: &issues, path: "actions")
    }
}

private func checkElementVersionsInList(_ elements: [Any], cardVersion: String, issues: inout [ValidationIssue], path: String) {
    for (i, el) in elements.enumerated() {
        guard let elMap = el as? [String: Any] else { continue }
        let p = "\(path)[\(i)]"
        let t = elMap["type"] as? String ?? ""
        checkElementVersion(t, cardVersion: cardVersion, issues: &issues, path: p)
        switch t {
        case "Container":
            let items = elMap["items"] as? [Any] ?? []
            if !items.isEmpty {
                checkElementVersionsInList(items, cardVersion: cardVersion, issues: &issues, path: p + ".items")
            }
        case "ColumnSet":
            let cols = elMap["columns"] as? [Any] ?? []
            for (ci, col) in cols.enumerated() {
                guard let colMap = col as? [String: Any] else { continue }
                let items = colMap["items"] as? [Any] ?? []
                if !items.isEmpty {
                    checkElementVersionsInList(items, cardVersion: cardVersion, issues: &issues,
                                               path: "\(p).columns[\(ci)].items")
                }
            }
        case "ActionSet":
            let actions = elMap["actions"] as? [Any] ?? []
            if !actions.isEmpty {
                checkActionVersionsInList(actions, cardVersion: cardVersion, issues: &issues, path: p + ".actions")
            }
        case "Table":
            let rows = elMap["rows"] as? [Any] ?? []
            for (r, row) in rows.enumerated() {
                guard let rowMap = row as? [String: Any] else { continue }
                let cells = rowMap["cells"] as? [Any] ?? []
                for (c, cell) in cells.enumerated() {
                    guard let cellMap = cell as? [String: Any] else { continue }
                    let items = cellMap["items"] as? [Any] ?? []
                    if !items.isEmpty {
                        checkElementVersionsInList(items, cardVersion: cardVersion, issues: &issues,
                                                   path: "\(p).rows[\(r)].cells[\(c)].items")
                    }
                }
            }
        default:
            break
        }
    }
}

private func checkActionVersionsInList(_ actions: [Any], cardVersion: String, issues: inout [ValidationIssue], path: String) {
    for (i, action) in actions.enumerated() {
        guard let actionMap = action as? [String: Any] else { continue }
        let p = "\(path)[\(i)]"
        let t = actionMap["type"] as? String ?? ""
        checkElementVersion(t, cardVersion: cardVersion, issues: &issues, path: p)
        if t == "Action.ShowCard", let inner = actionMap["card"] as? [String: Any] {
            let body = inner["body"] as? [Any] ?? []
            if !body.isEmpty {
                checkElementVersionsInList(body, cardVersion: cardVersion, issues: &issues, path: p + ".card.body")
            }
            let innerActions = inner["actions"] as? [Any] ?? []
            if !innerActions.isEmpty {
                checkActionVersionsInList(innerActions, cardVersion: cardVersion, issues: &issues, path: p + ".card.actions")
            }
        }
    }
}
