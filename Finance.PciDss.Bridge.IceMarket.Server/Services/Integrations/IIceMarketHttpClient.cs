using System.Threading.Tasks;
using Finance.PciDss.Bridge.IceMarket.Server.Services.Integrations.Contracts.Requests;
using Finance.PciDss.Bridge.IceMarket.Server.Services.Integrations.Contracts.Responses;

namespace Finance.PciDss.Bridge.IceMarket.Server.Services.Integrations
{
    public interface IIceMarketHttpClient
    {
        /// <summary>
        /// A purchase deduct amount immediately. This transaction type is intended when the goods or services
        /// can be immediately provided to the customer. 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        Task<Response<IceMarketInvoiceResponse, IceMarketFailResponseDataPayment>> RegisterInvoiceAsync(
            IceMarketInvoiceRequest request, string baseUrl);

        /// <summary>
        /// It allows to get previous transaction basic information
        /// </summary>
        /// <param name="request"></param>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        Task<Response<IceMarketStatusResponse, IceMarketFailResponseDataPayment>> GetStatusInvoiceAsync(
            IceMarketStatusRequest request, string baseUrl);

    }
}