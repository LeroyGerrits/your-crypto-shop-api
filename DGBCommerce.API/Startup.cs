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

            services.AddSingleton(provider => Configuration);
            services.AddScoped<IDataAccessLayer, DataAccessLayer>(_ => new DataAccessLayer(connectionString));

            services.AddScoped<IDeliveryMethodRepository, DeliveryMethodRepository>();
            services.AddScoped<IFaqCategoryRepository, FaqCategoryRepository>();
            services.AddScoped<IFaqRepository, FaqRepository>();
            services.AddScoped<INewsMessageRepository, NewsMessageRepository>();
            services.AddScoped<IShopRepository, ShopRepository>();

            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.PropertyNamingPolicy = null);
        }

        public void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseCors(options => options.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader());
            }
            else
            {

            }

            
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
        }
    }
}
