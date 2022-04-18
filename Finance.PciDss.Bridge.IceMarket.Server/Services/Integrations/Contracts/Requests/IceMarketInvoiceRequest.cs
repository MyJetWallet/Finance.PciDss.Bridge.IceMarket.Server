using Newtonsoft.Json;

namespace Finance.PciDss.Bridge.IceMarket.Server.Services.Integrations.Contracts.Requests
{
    public class IceMarketInvoiceRequest
    {
        [JsonProperty("transaction_id")] public string transaction_id { get; set; }
        [JsonProperty("amount")] public string amount { get; set; }
        [JsonProperty("email")] public string email { get; set; }
        [JsonProperty("first_name")] public string first_name { get; set; }
        [JsonProperty("last_name")] public string last_name { get; set; }
        [JsonProperty("phoneNum")] public string phoneNum { get; set; }
        [JsonProperty("billCountry")] public string billCountry { get; set; }
        [JsonProperty("billState")] public string billState { get; set; }
        [JsonProperty("billCity")] public string billCity { get; set; }
        [JsonProperty("billAddress")] public string billAddress { get; set; }
        [JsonProperty("billZip")] public string billZip { get; set; }
        [JsonProperty("ipn")] public string ipn { get; set; }
        [JsonProperty("callback_url")] public string callback_url { get; set; }
        [JsonProperty("redirect_url")] public string redirect_url { get; set; }
    }
}