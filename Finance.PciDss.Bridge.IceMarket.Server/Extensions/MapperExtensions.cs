using System.Diagnostics;
using System.Globalization;
using Finance.PciDss.Abstractions;
using Finance.PciDss.Bridge.IceMarket.Server.Services.Integrations.Contracts.Requests;
using Finance.PciDss.Bridge.IceMarket.Server.Services.Integrations.Contracts.Responses;
using Flurl;

namespace Finance.PciDss.Bridge.IceMarket.Server.Extensions
{
    public static class MapperExtensions
    {
        public static IceMarketInvoiceRequest ToCreatePaymentInvoiceRequest(this IPciDssInvoiceModel model, SettingsModel settingsModel)
        {
            var names = model.FullName.Split(' ');
            
            return new IceMarketInvoiceRequest
            {
                callback_url = settingsModel.IceMarketCallbackUrl,
                redirect_url = settingsModel.IceMarketRedirectUrl,
                amount = model.PsAmount.ToString(CultureInfo.InvariantCulture),
                email = model.Email,
                ipn = settingsModel.IceMarketIpn,
                billAddress = model.Address,
                billCity = model.City,
                billCountry = model.Country,
                billState = model.City,
                billZip = model.Zip,
                first_name = names[0],
                last_name = names.Length == 2 ? names[1] : null,
                phoneNum = model.PhoneNumber,
                transaction_id = model.OrderId,
            };
        }

        public static IceMarketStatusRequest ToStatusRequest(this IceMarketInvoiceResponse src, SettingsModel settingsModel)
        {
            return new IceMarketStatusRequest
            {
               
            };
        }
    }
}
