using ProvaPub.Interfaces;
using ProvaPub.Services;

namespace ProvaPub
{
    public static class ApiModule
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //Services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomerService, CustomerService>();

            //Payment Strategy
            services.AddScoped<IPaymentStrategy, PaypalPayment>(); 
            services.AddScoped<IPaymentStrategy, CreditCardPayment>(); 
            services.AddScoped<IPaymentStrategy, PixPayment>();
            services.AddScoped<OrderService>();

            //Providers
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();

            return services;
        }
    }
}
