namespace FluentCards;

/// <summary>
/// Fluent builder for creating FactSet elements.
/// </summary>
public class FactSetBuilder
{
    private readonly FactSet _factSet = new() { Facts = new List<Fact>() };

    /// <summary>
    /// Sets the unique identifier for the fact set.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public FactSetBuilder WithId(string id)
    {
        _factSet.Id = id;
        return this;
    }

    /// <summary>
    /// Adds a fact to the fact set.
    /// </summary>
    /// <param name="title">The title of the fact.</param>
    /// <param name="value">The value of the fact.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public FactSetBuilder AddFact(string title, string value)
    {
        _factSet.Facts!.Add(new Fact { Title = title, Value = value });
        return this;
    }

    /// <summary>
    /// Adds a fact to the fact set.
    /// </summary>
    /// <param name="fact">The fact to add.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public FactSetBuilder AddFact(Fact fact)
    {
        _factSet.Facts!.Add(fact);
        return this;
    }

    /// <summary>
    /// Builds and returns the configured FactSet.
    /// </summary>
    /// <returns>The configured FactSet instance.</returns>
    public FactSet Build()
    {
        return _factSet;
    }
}
