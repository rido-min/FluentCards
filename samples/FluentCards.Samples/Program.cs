using FluentCards;

var card = new AdaptiveCard
{
    Body = new List<AdaptiveElement>
    {
        new FactSet
        {
            Facts = new List<Fact>
            {
                new Fact { Title = "Phone", Value = "+1-555-1234" }
            }
        }
    }
};

var json = card.ToJson();
Console.WriteLine(json);
Console.WriteLine("---");
Console.WriteLine("Contains check: " + json.Contains("\"value\": \"+1-555-1234\""));
Console.WriteLine("Contains check2: " + json.Contains("+1-555-1234"));
