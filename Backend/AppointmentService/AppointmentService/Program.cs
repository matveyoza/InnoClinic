using AppointmentPresentation.Extensions;
using DotNetEnv;
using Service.Mapping;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureSqlContext();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(Service.Mapping.MappingProfile).Assembly);
});

builder.Services.ConfigureRepository();
builder.Services.ConfigureService();

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
