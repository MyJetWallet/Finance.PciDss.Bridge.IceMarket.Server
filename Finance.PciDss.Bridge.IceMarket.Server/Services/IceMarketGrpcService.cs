using System;
using System.Threading.Tasks;
using Finance.PciDss.Abstractions;
using Finance.PciDss.Bridge.IceMarket.Server.Extensions;
using Finance.PciDss.Bridge.IceMarket.Server.Interfaces;
using Finance.PciDss.Bridge.IceMarket.Server.Services.Integrations;
using Finance.PciDss.PciDssBridgeGrpc;
using Finance.PciDss.PciDssBridgeGrpc.Contracts;
using Finance.PciDss.PciDssBridgeGrpc.Contracts.Enums;
using MyCrm.AuditLog.Grpc;
using MyCrm.AuditLog.Grpc.Models;
using Newtonsoft.Json;
using Serilog;
using SimpleTrading.ConvertService.Grpc;
using SimpleTrading.ConvertService.Grpc.Contracts;
using SimpleTrading.GrpcTemplate;

namespace Finance.PciDss.Bridge.IceMarket.Server.Services
{
    public class IceMarketGrpcService : IFinancePciDssBridgeGrpcService
    {
        private const string PaymentSystemId = "pciDssIceMarketBankCards";
        private const string UsdCurrency = "USD";
        private const string EurCurrency = "EUR";
        private readonly ILogger _logger;
        private readonly GrpcServiceClient<IMyCrmAuditLogGrpcService> _myCrmAuditLogGrpcService;
        private readonly ISettingsModelProvider _optionsMonitorSettingsModelProvider;
        private readonly IPageStorage _pageStorage;
        private readonly IIceMarketHttpClient _iceMarketHttpClient;
        private readonly GrpcServiceClient<IConvertService> _convertServiceClient;

        public IceMarketGrpcService(IIceMarketHttpClient iceMarketHttpClient,
            GrpcServiceClient<IMyCrmAuditLogGrpcService> myCrmAuditLogGrpcService,
            GrpcServiceClient<IConvertService> convertServiceClient,
            ISettingsModelProvider optionsMonitorSettingsModelProvider,
            IPageStorage pageStorage,
            ILogger logger)
        {
            _iceMarketHttpClient = iceMarketHttpClient;
            _myCrmAuditLogGrpcService = myCrmAuditLogGrpcService;
            _convertServiceClient = convertServiceClient;
            _optionsMonitorSettingsModelProvider = optionsMonitorSettingsModelProvider;
            _pageStorage = pageStorage;
            _logger = logger;
        }

        private SettingsModel _settingsModel => _optionsMonitorSettingsModelProvider.Get();

        public async ValueTask<MakeBridgeDepositGrpcResponse> MakeDepositAsync(MakeBridgeDepositGrpcRequest request)
        {
            _logger.Information("IceMarketGrpcService start process MakeBridgeDepositGrpcRequest {@request}", request);

            try
            {
                var validateResult = request.Validate();
                if (validateResult.IsFailed)
                {
                    _logger.Warning("IceMarket request is not valid. Errors {@validateResult}", validateResult);
                    await SendMessageToAuditLogAsync(request.PciDssInvoiceGrpcModel,
                        $"Fail IceMarket create invoice. Error {validateResult}");
                    return MakeBridgeDepositGrpcResponse.Failed(DepositBridgeRequestGrpcStatus.PaymentDeclined,
                        validateResult.ToString());
                }

                var createInvoiceRequest = request.PciDssInvoiceGrpcModel.ToCreatePaymentInvoiceRequest(_settingsModel);

                _logger.Information("IceMarket send request {@Request}", createInvoiceRequest);
                await SendMessageToAuditLogAsync(request.PciDssInvoiceGrpcModel,
                    @"IceMarket send sale request Amount: {createInvoiceRequest.Amount} currency: {createInvoiceRequest.Currency}");

                var createInvoiceResult =
                    await _iceMarketHttpClient.RegisterInvoiceAsync(createInvoiceRequest, _settingsModel.IceMarketRegisterPaymentUrl);

                if (createInvoiceResult.IsFailed)
                {
                    await SendMessageToAuditLogAsync(request.PciDssInvoiceGrpcModel,
                        $"{PaymentSystemId}. Fail IceMarket create invoice with kyc: {request.PciDssInvoiceGrpcModel.KycVerification}. Message: {createInvoiceResult.FailedResult.Message}. " +
                        $"Error: {JsonConvert.SerializeObject(createInvoiceResult.FailedResult.FieldError)}");
                    return MakeBridgeDepositGrpcResponse.Failed(DepositBridgeRequestGrpcStatus.ServerError,
                        createInvoiceResult.FailedResult.Message);
                }

                _logger.Information("Created deposit invoice {@TransactionId} {@Kyc} {@Resp}",
                    request.PciDssInvoiceGrpcModel.OrderId,
                    request.PciDssInvoiceGrpcModel.KycVerification,
                    createInvoiceResult.SuccessResult);

                await SendMessageToAuditLogAsync(request.PciDssInvoiceGrpcModel,
                    $"Created deposit invoice with id: {request.PciDssInvoiceGrpcModel.OrderId} " +
                    $"kyc: {request.PciDssInvoiceGrpcModel.KycVerification} " +
                    $"redirectUrl: {createInvoiceResult.SuccessResult}");

                var pageSaveResult = await _pageStorage.SavePageAsync(
                    createInvoiceResult.SuccessResult.PaymentPageHtml,
                    createInvoiceResult.SuccessResult.TransactionId);
                
                if (pageSaveResult.IsFailed)
                {
                    await SendMessageToAuditLogAsync(request.PciDssInvoiceGrpcModel,
                        $"{PaymentSystemId}. Failed to save IceMarket payment page. Message: {createInvoiceResult.FailedResult.Message}.");
                    return MakeBridgeDepositGrpcResponse.Failed(DepositBridgeRequestGrpcStatus.ServerError,
                        createInvoiceResult.FailedResult.Message);
                }

                return MakeBridgeDepositGrpcResponse.Create(pageSaveResult.Value,
                    createInvoiceResult.SuccessResult.TransactionId,
                    DepositBridgeRequestGrpcStatus.Success);
            }
            catch (Exception e)
            {
                _logger.Error(e, "MakeDepositAsync failed for traderId {traderId}",
                    request.PciDssInvoiceGrpcModel.TraderId);
                await SendMessageToAuditLogAsync(request.PciDssInvoiceGrpcModel,
                    $"{PaymentSystemId}. MakeDeposit failed");
                return MakeBridgeDepositGrpcResponse.Failed(DepositBridgeRequestGrpcStatus.ServerError, e.Message);
            }
        }

        public ValueTask<GetPaymentSystemGrpcResponse> GetPaymentSystemNameAsync()
        {
            return new ValueTask<GetPaymentSystemGrpcResponse>(GetPaymentSystemGrpcResponse.Create(PaymentSystemId));
        }

        public ValueTask<GetPaymentSystemCurrencyGrpcResponse> GetPsCurrencyAsync()
        {
            return new ValueTask<GetPaymentSystemCurrencyGrpcResponse>(
                GetPaymentSystemCurrencyGrpcResponse.Create(EurCurrency));
        }

        public async ValueTask<GetPaymentSystemAmountGrpcResponse> GetPsAmountAsync(
            GetPaymentSystemAmountGrpcRequest request)
        {
            if (request.Currency.Equals(UsdCurrency, StringComparison.OrdinalIgnoreCase))
            {
                var convertResponse = await _convertServiceClient.Value.Convert(new CovertRequest
                {
                    InstrumentId = EurCurrency + UsdCurrency,
                    ConvertType = ConvertTypes.QuoteToBase,
                    Amount = request.Amount
                });

                return GetPaymentSystemAmountGrpcResponse.Create(convertResponse.ConvertedAmount, EurCurrency);
            }

            return default;
        }

        public ValueTask<GetDepositStatusGrpcResponse> GetDepositStatusAsync(GetDepositStatusGrpcRequest request)
        {
            throw new NotImplementedException();
        }

        public ValueTask<DecodeBridgeInfoGrpcResponse> DecodeInfoAsync(DecodeBridgeInfoGrpcRequest request)
        {
            throw new NotImplementedException();
        }

        public ValueTask<MakeConfirmGrpcResponse> MakeDepositConfirmAsync(MakeConfirmGrpcRequest request)
        {
            throw new NotImplementedException();
        }

        private ValueTask SendMessageToAuditLogAsync(IPciDssInvoiceModel invoice, string message)
        {
            return _myCrmAuditLogGrpcService.Value.SaveAsync(new AuditLogEventGrpcModel
            {
                TraderId = invoice.TraderId,
                Action = "deposit",
                ActionId = invoice.OrderId,
                DateTime = DateTime.UtcNow,
                Message = message
            });
        }
    }
}