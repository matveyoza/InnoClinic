using DotNetEnv;
using Appointments.Service.Mapping;
using Appointments.Service.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Appointments.Presentation.Extensions;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureSqlContext();

builder.Services.ConfigureJwt();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(MappingProfile).Assembly);
});

builder.Services.ConfigureRepository();
builder.Services.ConfigureService();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(AppointmentForCreationDtoValidator).Assembly);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
