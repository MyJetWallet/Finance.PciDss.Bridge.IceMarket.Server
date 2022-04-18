using Finance.PciDss.Bridge.IceMarket.Server.Interfaces;
using Finance.PciDss.Bridge.IceMarket.Server.Services;
using Finance.PciDss.Bridge.IceMarket.Server.Services.Integrations;
using Microsoft.Extensions.DependencyInjection;
using MyCrm.AuditLog.Grpc;
using Serilog;
using SimpleTrading.ConvertService.Grpc;
using SimpleTrading.GrpcTemplate;
using SimpleTrading.MyLogger;
using SimpleTrading.SettingsReader;

namespace Finance.PciDss.Bridge.IceMarket.Server
{
    public static class ServicesBinder
    {
        public static string AppName { get; private set; } = "Finance.PciDss.Bridge.IceMarket.Server";

        public static void BindServices(this IServiceCollection services)
        {
            services.AddSingleton<IIceMarketHttpClient, IceMarketHttpClient>();
            services.AddSingleton<IPageStorage, AzureBlobPageStorage>();
        }

        public static void BindLogger(this IServiceCollection services, SettingsModel settings)
        {
            var logger = new MyLogger(AppName, settings.SeqServiceUrl);
            Log.Logger = logger;
            services.AddSingleton<ILogger>(logger);
        }

        public static void BindSettings(this IServiceCollection services, SettingsModel settings)
        {
            services.AddSingleton<ISettingsModelProvider, SettingsModelProvider>();
        }

        public static void BindGrpcServices(this IServiceCollection services, SettingsModel settings)
        {
            var client = new GrpcServiceClient<IMyCrmAuditLogGrpcService>(
                () => SettingsReader
                    .ReadSettings<SettingsModel>()
                    .AuditLogGrpcServiceUrl);
            services.AddSingleton(client);

            var clientConvertService = new GrpcServiceClient<IConvertService>(
                () => SettingsReader
                    .ReadSettings<SettingsModel>()
                    .ConvertServiceGrpcUrl);

            services.AddSingleton(clientConvertService);
        }
    }
}
