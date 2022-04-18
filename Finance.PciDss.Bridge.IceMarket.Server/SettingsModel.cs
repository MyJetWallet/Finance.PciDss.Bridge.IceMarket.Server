using SimpleTrading.SettingsReader;

namespace Finance.PciDss.Bridge.IceMarket.Server
{
    [YamlAttributesOnly]
    public class SettingsModel
    {
        [YamlProperty("PciDssBridgeIceMarket.SeqServiceUrl")]
        public string SeqServiceUrl { get; set; }

        [YamlProperty("PciDssBridgeIceMarket.IceMarketRegisterPaymentUrl")]
        public string IceMarketRegisterPaymentUrl { get; set; }
        
        [YamlProperty("PciDssBridgeIceMarket.IceMarketPaymentStatusUrl")]
        public string IceMarketPaymentStatusUrl { get; set; }

        [YamlProperty("PciDssBridgeIceMarket.IceMarketCallbackUrl")]
        public string IceMarketCallbackUrl { get; set; }

        [YamlProperty("PciDssBridgeIceMarket.AuditLogGrpcServiceUrl")]
        public string AuditLogGrpcServiceUrl { get; set; }

        [YamlProperty("PciDssBridgeIceMarket.ConvertServiceGrpcUrl")]
        public string ConvertServiceGrpcUrl { get; set; }

        [YamlProperty("PciDssBridgeIceMarket.IceMarketRedirectUrl")]
        public string IceMarketRedirectUrl { get; set; }
        
        [YamlProperty("PciDssBridgeIceMarket.IceMarketIpn")]
        public string IceMarketIpn { get; set; }
        
        [YamlProperty("PciDssBridgeIceMarket.PaymentsAzureBlobConnectionString")]
        public string PaymentsAzureBlobConnectionString { get; set; }
        
        [YamlProperty("PciDssBridgeIceMarket.PaymentPagesAzureBlobContainer")]
        public string PaymentPagesAzureBlobContainer { get; set; }
        
        [YamlProperty("PciDssBridgeIceMarket.PaymentPageStorageBaseUrl")]
        public string PaymentPageStorageBaseUrl { get; set; }
    }
}
