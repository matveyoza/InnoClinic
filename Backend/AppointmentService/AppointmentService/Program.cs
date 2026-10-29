using AppointmentPresentation.Extensions;
using DotNetEnv;
using Service.Mapping;
using Service.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;

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

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(AppointmentForCreationDtoValidator).Assembly);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
