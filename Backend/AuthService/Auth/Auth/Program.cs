using AuthPresentation.Extensions;
using DotNetEnv;
using FluentValidation;
using FluentValidation.AspNetCore;
using Service;
using Service.Constants;
using Service.Contracts;
using Service.Mapping;

var builder = WebApplication.CreateBuilder(args);

Env.Load(Path.Combine(Directory.GetCurrentDirectory(), ".env"));

builder.Services.ConfigureSqlContext();

builder.Services.ConfigureIdentity();

builder.Services.ConfigureJwt(builder.Configuration);

builder.Services.ConfigureCors(builder.Configuration);

builder.Services.AddHttpClient("UserService", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:UserServiceUrl"] ?? "https://localhost:7183/");
});

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddControllers()
    .AddApplicationPart(typeof(Auth.Controllers.AuthController).Assembly);

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssembly(typeof(MappingProfile).Assembly);

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(MappingProfile).Assembly);
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors(AppConstants.CorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
