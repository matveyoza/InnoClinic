using Service.Shared.DataTransferObjects;
using Service.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentPresentation.AppointmentsController
{
    [ApiController]
    [Route("api/appointments")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }


        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateAppointmentDto dto,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdAppointment = await _appointmentService.CreateAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdAppointment.Id },
                createdAppointment);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var appointments = await _appointmentService.GetAllAsync(cancellationToken);
            return Ok(appointments);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var appointment = await _appointmentService.GetByIdAsync(id, cancellationToken);

            if (appointment is null)
            {
                return NotFound(new { message = $"Appointment with ID '{id}' was not found." });
            }

            return Ok(appointment);
        }
    }
}