using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// JSON converter for enums that uses camelCase naming.
/// </summary>
/// <typeparam name="T">The enum type to convert.</typeparam>
internal class CamelCaseEnumConverter<T> : JsonStringEnumConverter<T> where T : struct, Enum
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CamelCaseEnumConverter{T}"/> class.
    /// </summary>
    public CamelCaseEnumConverter() : base(JsonNamingPolicy.CamelCase)
    {
    }
}
