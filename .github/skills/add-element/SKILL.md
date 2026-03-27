---
name: add-element
description: 'Scaffold a new Adaptive Card element with model, builder, JSON context registration, and tests. Use when adding a new card element type.'
---

# Add Adaptive Card Element

Scaffolds all files needed for a new Adaptive Card element type.

## When to Use

- Adding a new display element (e.g., CodeBlock, Rating)
- Adding a new input element (e.g., Input.Rating)

## Process

### Step 1: Create the model class
<!-- TODO: Create src/FluentCards/{ElementName}.cs inheriting AdaptiveElement or InputElement -->

### Step 2: Create the builder class
<!-- TODO: Create src/FluentCards/{ElementName}Builder.cs following the WithX()/Build() pattern -->

### Step 3: Register in FluentCardsJsonContext
<!-- TODO: Add [JsonSerializable(typeof({ElementName}))] to FluentCardsJsonContext.cs -->

### Step 4: Add to AdaptiveCardBuilder
<!-- TODO: Add Add{ElementName}(Action<{ElementName}Builder>) method to AdaptiveCardBuilder.cs -->

### Step 5: Write tests
<!-- TODO: Create tests/FluentCards.Tests/{ElementName}Tests.cs with builder and serialization tests -->

## Constraints

- Must be AOT-compatible — no reflection-based serialization.
- All public members need XML doc comments.
- Builder methods return `this` for fluent chaining.

## Validation

- `dotnet build --configuration Release` succeeds with no warnings.
- `dotnet test --configuration Release --no-build` passes.
