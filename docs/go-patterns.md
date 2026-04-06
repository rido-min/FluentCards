# Go-Specific Patterns

This guide explains the Go-specific patterns used in the FluentCards Go SDK for developers familiar with the Python or C# versions.

## Builder pattern: `func(*Builder)` vs Python lambdas

In Python you write:

```python
builder.add_text_block(lambda tb: tb.with_text("Hello").with_size(TextSize.LARGE))
```

In Go, lambdas with closures don't exist in that form. The equivalent uses an anonymous function:

```go
builder.AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
    tb.WithText("Hello").WithSize(fluentcards.TextSizeLarge)
})
```

Both patterns create a new builder, pass it to your function for configuration, then call `Build()` internally.

## Enum naming: `TypeSizeLarge` not `TextSize.Large`

Go has no enum type or namespace within a package. All names live in one flat namespace. To avoid collisions between `TextSize.Large`, `ImageSize.Large`, `Spacing.Large`, etc., enum constants use a `TypeName + ValueName` prefix:

| Python | Go |
|--------|-----|
| `TextSize.LARGE` | `fluentcards.TextSizeLarge` |
| `TextWeight.BOLDER` | `fluentcards.TextWeightBolder` |
| `TextColor.ATTENTION` | `fluentcards.TextColorAttention` |
| `Spacing.NONE` | `fluentcards.SpacingNone` |
| `ActionStyle.POSITIVE` | `fluentcards.ActionStylePositive` |
| `ContainerStyle.EMPHASIS` | `fluentcards.ContainerStyleEmphasis` |

This mirrors Go standard library conventions (`http.MethodGet`, `os.ModeDir`).

## Error handling

Go functions that can fail return `(value, error)`:

```go
json, err := fluentcards.ToJSON(card)
if err != nil {
    // handle error
}
```

`Validate` never fails — it returns a slice of issues (empty if the card is valid):

```go
issues := fluentcards.Validate(card)
for _, issue := range issues {
    if issue.Severity == fluentcards.ValidationSeverityError {
        fmt.Printf("Error [%s] %s: %s\n", issue.Code, issue.Path, issue.Message)
    }
}
```

`ValidateAndPanic` panics with `*AdaptiveCardValidationError` if any Error-severity issues exist. This mirrors `validate_and_throw` in Python:

```go
// Recoverable panic:
defer func() {
    if r := recover(); r != nil {
        if err, ok := r.(*fluentcards.AdaptiveCardValidationError); ok {
            fmt.Println(err.Error())
        }
    }
}()
fluentcards.ValidateAndPanic(card)
```

## The `Card` type

`Card` is a type alias for `map[string]any`. Every `Build()` call returns a `Card`:

```go
type Card = map[string]any
```

You can read fields directly if needed:

```go
card := builder.Build()
version := card["version"].(string)  // type assertion required
body := card["body"].([]any)
```

You can also construct elements manually and pass them in:

```go
prebuilt := map[string]any{"type": "TextBlock", "text": "Custom"}
card := fluentcards.NewAdaptiveCardBuilder().AddElement(prebuilt).Build()
```

## Go module versioning

Unlike .NET (NuGet) or Python (PyPI), Go modules don't require uploading to a registry. Publishing happens via git tags. The tag format for modules in a monorepo subdirectory is:

```
go/v0.1.0
```

Users install with:

```bash
go get github.com/rido-min/FluentCards/go@go/v0.1.0
```

Or the latest version on the main branch:

```bash
go get github.com/rido-min/FluentCards/go@main
```

Documentation is automatically available at [pkg.go.dev](https://pkg.go.dev/github.com/rido-min/FluentCards/go/fluentcards) once the module is tagged.

## Why a single package?

The Python SDK uses sub-directories (`builders/`, `builders/inputs/`) to organize files. Go packages have different granularity rules — splitting into `fluentcards/builders`, `fluentcards/inputs`, etc. would require callers to import multiple paths for basic usage.

The single `fluentcards` package keeps all imports simple:

```go
import "github.com/rido-min/FluentCards/go/fluentcards"
```
