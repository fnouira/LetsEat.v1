using LetsEat.Services.Core;
using Microsoft.Extensions.DependencyInjection;

namespace LetsEat.Services.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<ITableBookingFactory, TableBookingFactory>();

            services.AddTransient<ITableBookingProcessorService, TableBookingProcessorService>();

            return services;
        }
    }
}
