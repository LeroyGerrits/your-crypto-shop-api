﻿using YourCryptoShop.API.Services;
using YourCryptoShop.Data;
using YourCryptoShop.Data.Repositories;
using YourCryptoShop.Data.Services;
using YourCryptoShop.Data.Services.RpcServices;
using YourCryptoShop.Domain.Interfaces;
using YourCryptoShop.Domain.Interfaces.Repositories;
using YourCryptoShop.Domain.Interfaces.Services;
using YourCryptoShop.Domain.Interfaces.Services.RpcServices;
using Microsoft.Extensions.FileProviders;
using System.Text.Json.Serialization;

namespace YourCryptoShop.API
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration _configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = _configuration.GetConnectionString("YourCryptoShop") ?? throw new Exception("Connection string 'YourCryptoShop' not set.");
            RpcSettings rpcSettings = _configuration.GetSection("RpcSettings").Get<RpcSettings>() ?? throw new Exception("RPC settings not configured.");
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
            services.AddScoped<IUtils, Utils>();

            // Services
            services.AddScoped<IAddressService, AddressService>();
            
            // RPC Services
            services.AddScoped<IBitcoinCashRpcService, BitcoinCashRpcService>(_ => new BitcoinCashRpcService(rpcSettings.Username, rpcSettings.Password));
            services.AddScoped<IDecredRpcService, DecredRpcService>(_ => new DecredRpcService(rpcSettings.Username, rpcSettings.Password));
            services.AddScoped<IDigiByteRpcService, DigiByteRpcService>(_ => new DigiByteRpcService(rpcSettings.Username, rpcSettings.Password));
            services.AddScoped<IDogecoinRpcService, DogecoinRpcService>(_ => new DogecoinRpcService(rpcSettings.Username, rpcSettings.Password));
            services.AddScoped<ILitecoinRpcService, LitecoinRpcService>(_ => new LitecoinRpcService(rpcSettings.Username, rpcSettings.Password));
            services.AddScoped<IMoneroRpcService, MoneroRpcService>(_ => new MoneroRpcService(rpcSettings.Username, rpcSettings.Password));

            // Repositories
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<ICurrencyRateRepository, CurrencyRateRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IDeliveryMethodRepository, DeliveryMethodRepository>();
            services.AddScoped<IDeliveryMethodCostsPerCountryRepository, DeliveryMethodCostsPerCountryRepository>();
            services.AddScoped<ICryptoWalletRepository, CryptoWalletRepository>();
            services.AddScoped<IFaqCategoryRepository, FaqCategoryRepository>();
            services.AddScoped<IFaqRepository, FaqRepository>();
            services.AddScoped<IFieldRepository, FieldRepository>();
            services.AddScoped<IFinancialStatementTransactionRepository, FinancialStatementTransactionRepository>();
            services.AddScoped<IGeneralRepository, GeneralRepository>();
            services.AddScoped<IMerchantRepository, MerchantRepository>();
            services.AddScoped<IMerchantPasswordResetLinkRepository, MerchantPasswordResetLinkRepository>();
            services.AddScoped<INewsMessageRepository, NewsMessageRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IPageRepository, PageRepository>();
            services.AddScoped<IPage2CategoryRepository, Page2CategoryRepository>();
            services.AddScoped<IPageCategoryRepository, PageCategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProduct2CategoryRepository, Product2CategoryRepository>();
            services.AddScoped<IProductFieldDataRepository, ProductFieldDataRepository>();
            services.AddScoped<IProductPhotoRepository, ProductPhotoRepository>();
            services.AddScoped<IShopRepository, ShopRepository>();
            services.AddScoped<IShop2CryptoWalletRepository, Shop2CryptoWalletRepository>();
            services.AddScoped<IShopCategoryRepository, ShopCategoryRepository>();
            services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            services.AddScoped<IShoppingCartItemRepository, ShoppingCartItemRepository>();
            services.AddScoped<IShoppingCartItemFieldDataRepository, ShoppingCartItemFieldDataRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
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
                app.UseHttpsRedirection();
                app.UseCors(
                    options => options
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .WithOrigins("https://*.yourcrypto.shop")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                );
            }

            app.UseMiddleware<JwtMiddleware>();
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