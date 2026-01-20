using FluentCards.WebRenderer.Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddScoped<ICardService, CardService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseStaticFiles();

// Minimal API endpoints
app.MapGet("/api/cards/{cardType}", (string cardType, ICardService cardService) =>
{
    var json = cardType.ToLower() switch
    {
        "welcome" => cardService.GetWelcomeCard(),
        "form" => cardService.GetFormCard(),
        "layout" => cardService.GetLayoutCard(),
        "product" => cardService.GetProductCard(),
        _ => null
    };

    if (json == null)
    {
        return Results.BadRequest(new { error = "Unknown card type" });
    }

    return Results.Content(json, "application/json");
});

app.MapGet("/api/cards/types", () =>
{
    var types = new[]
    {
        new { id = "welcome", name = "Welcome Card", description = "Introduction card with actions" },
        new { id = "form", name = "Contact Form", description = "Interactive form with validation" },
        new { id = "layout", name = "Layout Demo", description = "Advanced layouts with containers" },
        new { id = "product", name = "Product Card", description = "E-commerce product display" }
    };

    var json = System.Text.Json.JsonSerializer.Serialize(types);
    return Results.Content(json, "application/json");
});

app.MapPost("/api/cards/submit", async (CardSubmissionRequest request, ICardService cardService) =>
{
    if (request == null || string.IsNullOrEmpty(request.CardType))
    {
        return Results.BadRequest(new { error = "Card type is required" });
    }

    try
    {
        var result = cardService.HandleCardSubmission(request.CardType, request.Data ?? new Dictionary<string, object>());
        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message, statusCode: 500);
    }
});

// Fallback to index.html for SPA routing (but not for API routes)
app.MapFallbackToFile("index.html");

app.Run();

public class CardSubmissionRequest
{
    public string CardType { get; set; } = string.Empty;
    public Dictionary<string, object>? Data { get; set; }
}
