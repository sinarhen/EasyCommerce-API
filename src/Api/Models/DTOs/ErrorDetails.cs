using System.Text.Json;

namespace ECommerce.Models.DTOs;

public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string Message { get; set; }

    public override string ToString()
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        return JsonSerializer.Serialize(this, jsonSerializerOptions);
    }
}