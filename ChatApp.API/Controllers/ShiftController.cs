using ChatApp.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly IShiftService _shiftService;

        public ShiftController(IShiftService shiftService)
        {
            _shiftService = shiftService;
        }

        [HttpGet("allshift")]
        public IActionResult GetAllShifts()
        {
            var shifts = _shiftService.GetAllShifts();
            return Ok(shifts);
        }

        [HttpGet("{id}")]
        public IActionResult GetShiftById(int id)
        {
            var shift = _shiftService.GetShiftById(id);
            if (shift == null)
            {
                return NotFound();
            }

            return Ok(shift);
        }
    }
}

