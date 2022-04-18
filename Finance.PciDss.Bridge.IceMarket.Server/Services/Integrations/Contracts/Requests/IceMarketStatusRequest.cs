namespace Finance.PciDss.Bridge.IceMarket.Server.Services.Integrations.Contracts.Requests
{
    public class IceMarketStatusRequest
    {
        public string Ipn { get; set; }
        public string ClientTransactionId { get; set; }
    }
}