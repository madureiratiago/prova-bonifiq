using ProvaPub.Interfaces;
using ProvaPub.Services;

namespace ProvaPub
{
    public static class ApiModule
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomerService, CustomerService>();

            return services;
        }
    }
}
