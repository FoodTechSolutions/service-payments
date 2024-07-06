using Newtonsoft.Json;

namespace WebApplication1.models
{
    public class CashOutViewModel
    {
        [JsonProperty("amount")] public decimal Amount { get; set; }
    }
}
