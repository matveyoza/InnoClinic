using Backend.Extensions;
using DotNetEnv;
using Service;
using Service.Constants;
using Service.Contracts;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.ConfigureIdentity();

builder.Services.ConfigureJwt();

builder.Services.ConfigureCors(builder.Configuration);

builder.Services.ConfigureSqlContext();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(Service.Mapping.MappingProfile).Assembly);
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors(AppConstants.CorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
