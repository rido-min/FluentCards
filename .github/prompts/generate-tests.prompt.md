---
description: Generate xunit tests for the current FluentCards source file
---

Generate xunit tests for the code in ${file}. Follow these conventions:
- Use `[Fact]` and `[Theory]` with `[InlineData]`
- Name methods: `MethodOrFeature_Scenario_ExpectedBehavior`
- Follow Arrange/Act/Assert pattern
- Test builder fluent chaining, property setting, and JSON round-trip serialization
- Place in the matching `tests/FluentCards.Tests/` test file
