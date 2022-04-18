using System;
using System.Threading.Tasks;
using Finance.PciDss.Bridge.IceMarket.Server.Services.Integrations.Contracts.Requests;
using Finance.PciDss.Bridge.IceMarket.Server.Services.Integrations.Contracts.Responses;
using Flurl;
using Flurl.Http;

namespace Finance.PciDss.Bridge.IceMarket.Server.Services.Integrations
{
    public class IceMarketHttpClient : IIceMarketHttpClient
    {
        public async Task<Response<IceMarketInvoiceResponse, IceMarketFailResponseDataPayment>> 
            RegisterInvoiceAsync(IceMarketInvoiceRequest request, string baseUrl)
        {
            var result = await baseUrl
                .PostUrlEncodedAsync(request);
            var htmlPage = await result.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(htmlPage) || !result.IsSuccessStatusCode)
            {
                return Response<IceMarketInvoiceResponse, IceMarketFailResponseDataPayment>.CreateFailed(
                    new IceMarketFailResponseDataPayment());
            }
            
            return Response<IceMarketInvoiceResponse, IceMarketFailResponseDataPayment>.CreateSuccess(
                new IceMarketInvoiceResponse
                {
                    PaymentPageHtml = htmlPage,
                    TransactionId = request.transaction_id
                });
        }

        public async Task<Response<IceMarketStatusResponse, IceMarketFailResponseDataPayment>> 
            GetStatusInvoiceAsync(IceMarketStatusRequest request, string baseUrl)
        {
            var result = await baseUrl
                .SetQueryParam("client_transaction_id", request.ClientTransactionId)
                .SetQueryParam("ipn", request.Ipn)
                .GetAsync();
            
            return await result.DeserializeTo<IceMarketStatusResponse, IceMarketFailResponseDataPayment>();
        }
    }
}