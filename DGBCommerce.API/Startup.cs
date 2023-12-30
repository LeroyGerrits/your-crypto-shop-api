using DGBCommerce.API.Services;
using DGBCommerce.Data;
using DGBCommerce.Data.Repositories;
using DGBCommerce.Data.Services;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Interfaces.Services;
using Microsoft.Extensions.FileProviders;
using System.Text.Json.Serialization;

namespace DGBCommerce.API
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration _configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = _configuration.GetConnectionString("DGBCommerce") ?? throw new Exception("connectionString 'DBBCommerce' not set.");
            RpcSettings rpcSettings = _configuration.GetSection("RpcSettings").Get<RpcSettings>() ?? throw new Exception("RPC settings not configured.");
            if (rpcSettings.DaemonUrl == null) throw new Exception($"RPC {nameof(rpcSettings.DaemonUrl)} not configured.");
            if (rpcSettings.Username == null) throw new Exception($"RPC {nameof(rpcSettings.Username)} not configured.");
            if (rpcSettings.Password == null) throw new Exception($"RPC {nameof(rpcSettings.Password)} not configured.");

            services.AddCors();
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddSingleton(provider => _configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IDataAccessLayer, DataAccessLayer>(_ => new DataAccessLayer(connectionString));
            services.AddScoped<IJwtUtils, JwtUtils>();
            services.AddScoped<IMailService, MailService>();

            // Services
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IRpcService, RpcService>(_ => new RpcService(rpcSettings.DaemonUrl, rpcSettings.Username, rpcSettings.Password));
            
            // Repositories
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IDeliveryMethodRepository, DeliveryMethodRepository>();
            services.AddScoped<IDigiByteWalletRepository, DigiByteWalletRepository>();
            services.AddScoped<IFaqCategoryRepository, FaqCategoryRepository>();
            services.AddScoped<IFaqRepository, FaqRepository>();
            services.AddScoped<IFinancialStatementTransactionRepository, FinancialStatementTransactionRepository>();
            services.AddScoped<IMerchantRepository, MerchantRepository>();
            services.AddScoped<IMerchantPasswordResetLinkRepository, MerchantPasswordResetLinkRepository>();
            services.AddScoped<INewsMessageRepository, NewsMessageRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProduct2CategoryRepository, Product2CategoryRepository>();
            services.AddScoped<IProductPhotoRepository, ProductPhotoRepository>();
            services.AddScoped<IShopRepository, ShopRepository>();
            services.AddScoped<IShopCategoryRepository, ShopCategoryRepository>();
            services.AddScoped<IStatsRepository, StatsRepository>();
        }

        public void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseCors(
                    options => options
                    .WithOrigins(["http://localhost:4200", "http://localhost:4201"])
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                );
            }
            else
            {
                app.UseCors(
                    options => options
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .WithOrigins("https://*.dgbcommerce.com")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                );
            }

            app.UseMiddleware<JwtMiddleware>();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Resources")),
                RequestPath = new PathString("/Resources")
            });
            app.UseAuthorization();
            app.MapControllers();
        }
    }
}
