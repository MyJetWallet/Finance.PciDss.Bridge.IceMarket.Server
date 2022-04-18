using Newtonsoft.Json;

namespace Finance.PciDss.Bridge.IceMarket.Server.Services.Integrations.Contracts.Responses
{
    public class IceMarketInvoiceResponse
    {
        public string PaymentPageHtml { get; set; }
        public string TransactionId { get; set; }
    }
}