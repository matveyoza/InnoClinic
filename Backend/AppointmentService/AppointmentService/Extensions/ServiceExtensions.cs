using Entities.Contracts;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service.Contracts;
using Service;

namespace AppointmentPresentation.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services)
        {
            var connectionString = Environment.GetEnvironmentVariable("USER_DB_CONNECTION");
            services.AddDbContext<AppointmentDbContext>(opts =>
                opts.UseSqlServer(connectionString));
        }

        public static IServiceCollection ConfigureRepository(this IServiceCollection services) =>
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();

        public static IServiceCollection ConfigureService(this IServiceCollection services) =>
        services.AddScoped<IAppointmentService, AppointmentService>();

    }
}
