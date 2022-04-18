using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Finance.PciDss.Bridge.IceMarket.Server.Services.Integrations;
using Finance.PciDss.Bridge.IceMarket.Server.Services.Integrations.Contracts.Requests;
using NUnit.Framework;

namespace Finance.PciDss.Bridge.IceMarket.Test;

public class IceMarketHttpClientTests
{
    [Test]
    public async Task ShouldReturnHtml()
    {
        var request = new IceMarketInvoiceRequest()
        {
            callback_url = "https://webhook.site/a79faaf6-6459-46bf-8aaa-b618afe26566",
            redirect_url = "https://webhook.site/a79faaf6-6459-46bf-8aaa-b618afe26566",
            amount = "0.01",
            email = "test@mail.com",
            ipn = "",
            billAddress = "SO",
            billCity = "New",
            billCountry = "USA",
            billState = "New",
            billZip = "23231",
            first_name = "Test",
            last_name = "test",
            phoneNum = "4323423423",
            transaction_id = Guid.NewGuid().ToString(),
        };
        var client = new IceMarketHttpClient();
        
        var result = await client.RegisterInvoiceAsync(request, "https://icecreammarketingshop.com/payment/index.php");
        
        Assert.IsNotNull(result);
        Assert.IsFalse(result.IsFailed);
        Assert.IsNotEmpty(result.SuccessResult.PaymentPageHtml);
    }
}