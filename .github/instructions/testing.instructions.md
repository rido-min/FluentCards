---
applyTo: 'tests/**/*.cs'
---
# Testing Conventions
**When to read:** Writing or modifying test files.

- Use `[Fact]` for single-case tests, `[Theory]` with `[InlineData]` for parameterized tests.
- Name test methods: `MethodOrFeature_Scenario_ExpectedBehavior` (e.g., `Builder_WithTextBlock_AddsTextBlockToBody`).
- Follow Arrange/Act/Assert structure. Use `// Arrange & Act` when combined.
- Assert with xunit built-ins (`Assert.Equal`, `Assert.NotNull`, `Assert.Single`) — do not add FluentAssertions or other assertion libraries.
- One test class per feature file, named `{Feature}Tests.cs`.
