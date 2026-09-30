using Backend.Extensions;
using Service;
using Service.Contracts;
using DotNetEnv;

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
    cfg.AddMaps(typeof(Program).Assembly);
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
