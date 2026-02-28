using Microsoft.AspNetCore.Mvc;
using MovieBooking.Services;

namespace MovieBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CinemaController : ControllerBase
    {
        private readonly ICinemaService _service;

        public CinemaController(ICinemaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetCinemas()
        {
            var cinemas = await _service.GetAllCinemasAsync();
            return Ok(cinemas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCinemaById(int id)
        {
            if (id <= 0)
                return BadRequest("Cinema ID không hợp lệ.");

            var cinema = await _service.GetCinemaByIdAsync(id);

            if (cinema == null)
                return NotFound();

            return Ok(cinema);
        }
    }
}
