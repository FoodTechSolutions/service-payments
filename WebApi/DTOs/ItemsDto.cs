using Newtonsoft.Json;

namespace WebApplication1.DTOs;

public class ItemsDto
{
    [JsonProperty("sku_number")] public string? SkuNumber { get; set; }

    [JsonProperty("category")] public string? Category { get; set; }

    [JsonProperty("title")] public string? Title { get; set; }

    [JsonProperty("description")] public string? Description { get; set; }

    [JsonProperty("unit_price")] public decimal UnitPrice { get; set; }

    [JsonProperty("quantity")] public int Quantity { get; set; }

    [JsonProperty("unit_measure")] public string? UnitMeasure { get; set; }

    [JsonProperty("total_amount")] public decimal TotalAmount { get; set; }
}
