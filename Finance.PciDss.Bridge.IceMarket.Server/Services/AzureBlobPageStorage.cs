using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Finance.PciDss.Bridge.IceMarket.Server.Interfaces;
using Finance.PciDss.Bridge.IceMarket.Server.Models;
using Serilog;

namespace Finance.PciDss.Bridge.IceMarket.Server.Services
{
    public class AzureBlobPageStorage : IPageStorage
    {
        private readonly ILogger _logger;
        private readonly BlobContainerClient _blobContainerClient;
        private readonly SettingsModel _settings;

        public AzureBlobPageStorage(
            ISettingsModelProvider settingsModelProvider,
            ILogger logger
            )
        {
            _logger = logger;
            _settings = settingsModelProvider.Get();
            _blobContainerClient = new BlobContainerClient(_settings.PaymentsAzureBlobConnectionString, _settings.PaymentPagesAzureBlobContainer);
        }

        public async Task<ResultModel<string>> SavePageAsync(string html, string id)
        {
            try
            {
                var blobName = $"icemarket-{id}.html";
                var blobClient = _blobContainerClient.GetBlobClient(blobName);
                await blobClient.UploadAsync(BinaryData.FromString(html), new BlobUploadOptions
                {
                    
                    HttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = "text/html"
                    }
                });

                return new ResultModel<string>
                {
                    Value = $"{_settings.PaymentPageStorageBaseUrl}{blobName}"
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Failed to save page {id} to storage");
                return new ResultModel<string>
                {
                    IsFailed = true,
                    Message = $"Failed to save page {id} to storage. {ex.Message}"
                };
            }
            
        }
    }
}