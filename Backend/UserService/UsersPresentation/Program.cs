using DotNetEnv;
using Users.Service;
using Users.Service.Constants;
using Users.Service.Contracts;
using Users.Presentation.Extensions;

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
    cfg.AddMaps(typeof(Users.Service.Mapping.MappingProfile).Assembly);
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors(AppConstants.CorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
