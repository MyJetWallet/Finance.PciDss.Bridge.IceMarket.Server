using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Finance.PciDss.Bridge.IceMarket.Server;
using NUnit.Framework;

namespace Finance.PciDss.Bridge.IceMarket.Test;

public class Tests
{
    [Test]
    public async Task Test()
    {
        var connStr =
            "DefaultEndpointsProtocol=https;AccountName=simpletradingpayments;AccountKey=Gip2yPvE9gwNeCkWA8vOiB9/n9+lGluKZMrTeUzA0jr1Ilga9D/VzFZNA/KfBtIl8PjoSh/rzRgGZQplFHUPFg==;EndpointSuffix=core.windows.net";
        var blobContainerClient = new BlobContainerClient(connStr, "payment-pages");

        var name = $"{Guid.NewGuid().ToString()}.html";
        var html = $"<h2>Hello, it is a payment page {name}</h2>";
        var blob = BinaryData.FromString(html);
        var blobClient = blobContainerClient.GetBlobClient(name);
        var blobHttpHeaders = new BlobHttpHeaders
        {
            ContentType = "text/html"
        };
        var uploadedBlob = await blobClient.UploadAsync(blob, new BlobUploadOptions()
        {
            HttpHeaders = blobHttpHeaders
        });
        
        var uri = $"{blobContainerClient.Uri}/{name}";
    }
}