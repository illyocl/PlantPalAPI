using Microsoft.AspNetCore.Mvc;
using PlantPalAPI.Data;
using PlantPalAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace PlantPalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarEventController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CalendarEventController(AppDbContext context)
        {
            _context = context;
        }

        // Kullanıcıya ait takvim olayları
      
        [HttpGet("byUser")]
        public async Task<IActionResult> GetEventsByUser(int userId)
        {
            var events = await _context.CalendarEvents
                .Where(e => e.UserId == userId)
                .ToListAsync();

            foreach (var ev in events)
            {
                Console.WriteLine($"Event Fetched: {ev.Title}, Date: {ev.Date}");
            }

            return Ok(events);
        }

    }
}
