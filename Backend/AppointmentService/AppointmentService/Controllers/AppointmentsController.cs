using Service.Shared.DataTransferObjects;
using Service.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AppointmentPresentation.AppointmentsController
{
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


        [HttpPost]
        [Authorize(Roles = "Admin,Patient")]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentForCreationDto appointmentDto, CancellationToken cancellationToken)
        {
            var isCreated = await _appointmentService.CreateAppointmentAsync(appointmentDto, cancellationToken);

            if (!isCreated)
                return BadRequest(new { message = "User profile creation failed." });

            return Created();
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetAllAppointments(CancellationToken cancellationToken)
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync(cancellationToken);
            return Ok(appointments);
        }

        [HttpGet("{appointmentId:guid}")]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public async Task<IActionResult> GetAppointmentById([FromRoute] Guid appointmentId, CancellationToken cancellationToken)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(appointmentId, cancellationToken);

            if (appointment is null)
            {
                return NotFound(new { message = $"Appointment with ID '{appointmentId}' was not found." });
            }

            return Ok(appointment);
        }
    }
}