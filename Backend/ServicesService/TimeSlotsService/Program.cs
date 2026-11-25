using Entities.Contracts;
using Repository;
using Service;
using Service.Contracts;
using DotNetEnv;
using ServicesPresentation.Extensions;
using Service.Constants;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.ConfigureJwt();

builder.Services.ConfigureCors(builder.Configuration);

builder.Services.ConfigureSqlContext();

builder.Services.AddScoped<IServicesRepository, ServicesRepository>();
builder.Services.AddScoped<IServicesService, ServicesService>();

builder.Services.AddControllers();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(Service.Mapping.MappingProfile).Assembly);
});

var app = builder.Build();


app.UseHttpsRedirection();
app.UseCors(AppConstants.CorsPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();
