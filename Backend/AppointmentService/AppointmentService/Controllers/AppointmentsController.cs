using AppointmentPresentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Service.Shared.Constants;
using Service.Shared.DataTransferObjects;

namespace AppointmentPresentation.Controllers;

[ApiController]
[Route("api/appointments")]
[Authorize]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet]
    [Authorize(Roles = $"{AppRoles.Patient},{AppRoles.Doctor},{AppRoles.Admin}")]
    public async Task<IActionResult> GetAppointments(CancellationToken cancellationToken)
    {
        var appointments = await _appointmentService.GetAppointmentsAsync(
            User.GetUserId(),
            User.GetRole(),
            cancellationToken);

        return Ok(appointments);
    }

    [HttpPost]
    [Authorize(Roles = $"{AppRoles.Patient},{AppRoles.Admin}")]
    public async Task<IActionResult> CreateAppointment(
        [FromBody] AppointmentForCreationDto request,
        CancellationToken cancellationToken)
    {
        var created = await _appointmentService.CreateAppointmentAsync(
            User.GetUserId(),
            User.GetRole(),
            User.GetUserName(),
            request,
            cancellationToken);

        return Created();
    }
}