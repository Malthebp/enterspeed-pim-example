using CakeShop.Integration.PIM.Enterspeed;
using CakeShop.Integration.PIM.Enterspeed.Services;
using CakeShop.Integration.PIM.Enterspeed.Services.MyPim;
using Enterspeed.Source.Sdk.Api.Connection;
using Enterspeed.Source.Sdk.Api.Providers;
using Enterspeed.Source.Sdk.Api.Services;
using Enterspeed.Source.Sdk.Configuration;
using Enterspeed.Source.Sdk.Domain.Connection;
using Enterspeed.Source.Sdk.Domain.Providers;
using Enterspeed.Source.Sdk.Domain.Services;
using Enterspeed.Source.Sdk.Domain.SystemTextJson;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace CakeShop.Integration.PIM.Enterspeed
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<IPimService, MyPimService>();

            //Enterspeed
            builder.Services.AddScoped<IEnterspeedConnection, EnterspeedConnection>();
            builder.Services.AddTransient<IEnterspeedIngestService, EnterspeedIngestService>();
            builder.Services.AddScoped<IJsonSerializer, SystemTextJsonSerializer>();
            builder.Services.AddSingleton<IEnterspeedConfigurationProvider>(new EnterspeedConfigurationProvider(
                new EnterspeedConfiguration()
                {
                    // Create a Data Source in your Enterspeed tenant and use the Api Key from it here.
                    // You data source API Key starts with source-, eg. source-5fa4ed01-9ce9-41c9-80e2-681888337f56
                    // See: https://docs.enterspeed.com/ingest
                    ApiKey = "", 

                    // BaseUrl is always https://api.enterspeed.com, unless you have been told something else from Enterspeed.
                    BaseUrl = "https://api.enterspeed.com"
                }));

        }
    }
}