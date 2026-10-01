using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;

namespace Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddValidatorsFromAssembly(assembly);
            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(assembly);
            });

            return services;
        }
    }
}
