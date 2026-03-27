using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// JSON serialization context for FluentCards with source generation support.
/// </summary>
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    WriteIndented = true)]
[JsonSerializable(typeof(AdaptiveCard))]
[JsonSerializable(typeof(List<AdaptiveElement>))]
[JsonSerializable(typeof(List<AdaptiveAction>))]
[JsonSerializable(typeof(TextBlock))]
[JsonSerializable(typeof(Image))]
[JsonSerializable(typeof(Media))]
[JsonSerializable(typeof(MediaSource))]
[JsonSerializable(typeof(ImageSet))]
[JsonSerializable(typeof(RichTextBlock))]
[JsonSerializable(typeof(TextRun))]
[JsonSerializable(typeof(FactSet))]
[JsonSerializable(typeof(Fact))]
[JsonSerializable(typeof(ActionSet))]
[JsonSerializable(typeof(Table))]
[JsonSerializable(typeof(TableColumnDefinition))]
[JsonSerializable(typeof(TableRow))]
[JsonSerializable(typeof(TableCell))]
[JsonSerializable(typeof(BackgroundImage))]
[JsonSerializable(typeof(OpenUrlAction))]
[JsonSerializable(typeof(SubmitAction))]
[JsonSerializable(typeof(ShowCardAction))]
[JsonSerializable(typeof(ToggleVisibilityAction))]
[JsonSerializable(typeof(ExecuteAction))]
[JsonSerializable(typeof(TargetElement))]
[JsonSerializable(typeof(ActionStyle))]
[JsonSerializable(typeof(AssociatedInputs))]
[JsonSerializable(typeof(RefreshConfiguration))]
[JsonSerializable(typeof(AuthenticationConfiguration))]
[JsonSerializable(typeof(TokenExchangeResource))]
[JsonSerializable(typeof(AuthCardButton))]
[JsonSerializable(typeof(CardMetadata))]
[JsonSerializable(typeof(InputText))]
[JsonSerializable(typeof(InputNumber))]
[JsonSerializable(typeof(InputDate))]
[JsonSerializable(typeof(InputTime))]
[JsonSerializable(typeof(InputToggle))]
[JsonSerializable(typeof(InputChoiceSet))]
[JsonSerializable(typeof(Choice))]
[JsonSerializable(typeof(TextInputStyle))]
[JsonSerializable(typeof(ChoiceInputStyle))]
[JsonSerializable(typeof(Container))]
[JsonSerializable(typeof(ColumnSet))]
[JsonSerializable(typeof(Column))]
[JsonSerializable(typeof(CaptionSource))]
[JsonSerializable(typeof(DataQuery))]
[JsonSerializable(typeof(FontType))]
[JsonSerializable(typeof(TextBlockStyle))]
[JsonSerializable(typeof(InputLabelPosition))]
[JsonSerializable(typeof(InputStyle))]
[JsonSerializable(typeof(ActionMode))]
[JsonSerializable(typeof(Spacing))]
[JsonSerializable(typeof(List<object>))]
[JsonSerializable(typeof(JsonElement))]
public partial class FluentCardsJsonContext : JsonSerializerContext
{
}

