using Backend.Extensions;
using Service;
using Service.Contracts;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.ConfigureIdentity();

builder.Services.ConfigureJwt(builder.Configuration);

builder.Services.ConfigureCors(builder.Configuration);

builder.Services.ConfigureSqlContext();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
