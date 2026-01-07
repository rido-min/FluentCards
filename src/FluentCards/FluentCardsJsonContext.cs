using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// JSON serialization context for FluentCards with source generation support.
/// </summary>
[JsonSerializable(typeof(AdaptiveCard))]
[JsonSerializable(typeof(TextBlock))]
[JsonSerializable(typeof(OpenUrlAction))]
[JsonSerializable(typeof(SubmitAction))]
[JsonSerializable(typeof(ShowCardAction))]
[JsonSerializable(typeof(ToggleVisibilityAction))]
[JsonSerializable(typeof(ExecuteAction))]
[JsonSerializable(typeof(TargetElement))]
[JsonSerializable(typeof(ActionStyle))]
[JsonSerializable(typeof(AssociatedInputs))]
[JsonSerializable(typeof(InputText))]
[JsonSerializable(typeof(InputNumber))]
[JsonSerializable(typeof(InputDate))]
[JsonSerializable(typeof(InputTime))]
[JsonSerializable(typeof(InputToggle))]
[JsonSerializable(typeof(InputChoiceSet))]
[JsonSerializable(typeof(Choice))]
[JsonSerializable(typeof(TextInputStyle))]
[JsonSerializable(typeof(ChoiceInputStyle))]
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    WriteIndented = true)]
public partial class FluentCardsJsonContext : JsonSerializerContext
{
}
