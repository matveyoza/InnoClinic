using Microsoft.EntityFrameworkCore;
using Repository;
using Service.Constants;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ServicesPresentation.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration) =>
            services.AddCors(options =>
            {
                options.AddPolicy(AppConstants.CorsPolicy, builder =>
                    builder.WithOrigins("https://localhost:5173")
                    .WithMethods("GET", "POST", "PUT", "DELETE")
                    .WithHeaders("Content-Type", "Authorization", "Accept", "X-Requested-With")
                    .AllowCredentials());
            });
        public static void ConfigureSqlContext(this IServiceCollection services)
        {
            var connectionString = Environment.GetEnvironmentVariable("SERVICES_DB_CONNECTION");
            services.AddDbContext<ServicesDbContext>(opts =>
                opts.UseSqlServer(connectionString));
        }

        public static void ConfigureJwt(this IServiceCollection services)
        {
            var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
            var validIssuer = Environment.GetEnvironmentVariable("JWT_VALID_ISSUER");
            var validAudience = Environment.GetEnvironmentVariable("JWT_VALID_AUDIENCE");

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {

                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = validIssuer,
                    ValidAudience = validAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),

                    RoleClaimType = "role",
                    NameClaimType = "sub"
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.ContainsKey("AuthToken"))
                        {
                            context.Token = context.Request.Cookies["AuthToken"];
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
