using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Service.Shared.DataTransferObjects;
using InnoClinic.Shared.Constants;

namespace ServicesPresentation.Controllers
{
    [ApiController]
    [Route("api/services")]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesService _servicesService;

        public ServicesController(IServicesService servicesService)
        {
            _servicesService = servicesService;
        }

        [HttpGet("doctor/{doctorId:guid}")]
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Patient}")]
        public async Task<IActionResult> GetByDoctor(Guid doctorId, [FromQuery] DateOnly? date, CancellationToken cancellationToken)
        {
            var segments = await _servicesService.GetDoctorBookedSegmentsAsync(doctorId, date, cancellationToken);
            return Ok(segments);
        }

        [HttpPost]
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Patient}")]
        public async Task<IActionResult> BookSegment([FromBody] BookTimeSegmentDto dto, CancellationToken cancellationToken)
        {
            var created = await _servicesService.BookTimeSegmentAsync(dto, cancellationToken);

            if(!created)
                return BadRequest(new { message = "Time segment booking failed." });

            return Created();
        }

        [HttpGet("doctor-profiles")]

        public async Task<ActionResult<IEnumerable<DoctorProfileDto>>> GetDoctorProfiles(CancellationToken cancellationToken)
        {
            var profiles = await _servicesService.GetAllDoctorProfilesAsync(cancellationToken);
            return Ok(profiles);
        }
    }
}
