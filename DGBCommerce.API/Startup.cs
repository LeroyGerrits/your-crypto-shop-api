using DGBCommerce.API.Services;
using DGBCommerce.Data;
using DGBCommerce.Data.Repositories;
using DGBCommerce.Domain.Interfaces;

namespace DGBCommerce.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DGBCommerce") ?? throw new ArgumentException("connectionString 'DBBCommerce' not set.");

            services.AddCors();
            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.PropertyNamingPolicy = null);
            
            services.AddSingleton(provider => Configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IJwtUtils, JwtUtils>();
            services.AddScoped<IDataAccessLayer, DataAccessLayer>(_ => new DataAccessLayer(connectionString));
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            // Repositories
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IDeliveryMethodRepository, DeliveryMethodRepository>();
            services.AddScoped<IDigiByteWalletRepository, DigiByteWalletRepository>();
            services.AddScoped<IFaqCategoryRepository, FaqCategoryRepository>();
            services.AddScoped<IFaqRepository, FaqRepository>();
            services.AddScoped<IMerchantRepository, MerchantRepository>();
            services.AddScoped<INewsMessageRepository, NewsMessageRepository>();
            services.AddScoped<IShopRepository, ShopRepository>();
        }

        public void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseCors(
                    options => options
                    .WithOrigins("http://localhost:4200")
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
            app.UseAuthorization();
            app.MapControllers();
        }
    }
}
