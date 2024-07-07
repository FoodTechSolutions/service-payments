using Newtonsoft.Json;
using WebApplication1.models;

namespace WebApplication1.DTOs;

public class CreateQRCodeDTO
{
    [JsonProperty("cash_out")] public CashOutViewModel? CashOut { get; set; }
    [JsonProperty("description")] public string? Description { get; set; }
    [JsonProperty("external_reference")] public string? ExternalReference { get; set; }
    [JsonProperty("items")] public IEnumerable<ItemsDto>? Items { get; set; }
    [JsonProperty("notification_url")] public string? NotificationUrl { get; set; }
    [JsonProperty("title")] public string? Title { get; set; }
    [JsonProperty("total_amount")] public decimal TotalAmount { get; set; }
    [System.Text.Json.Serialization.JsonIgnore] public Guid OrderId { get; set; }
}
