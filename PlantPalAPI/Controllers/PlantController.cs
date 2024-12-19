using Microsoft.AspNetCore.Mvc;
using PlantPalAPI.Data;
using PlantPalAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace PlantPalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlantController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddPlant([FromQuery] int userId, [FromBody] Plant plant)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "Kullanıcı bulunamadı." });
            }

            plant.UserId = userId;
            plant.DateAdded = DateTime.Now;

            _context.Plants.Add(plant);
            await _context.SaveChangesAsync();

            
            AddDefaultEvents(plant);

            return Ok(new { message = "Bitki başarıyla eklendi." });
        }

        private void AddDefaultEvents(Plant plant)
        {
            var events = new List<CalendarEvent>();
            DateTime today = DateTime.Now;

            
            int wateringFrequency = plant.Size switch
            {
                "küçük" => 7,
                "orta" => 3,
                "büyük" => 2,
                _ => 7
            };

            for (int i = 1; i <= 12; i++)
            {
                events.Add(new CalendarEvent
                {
                    PlantId = plant.Id,
                    UserId = plant.UserId,
                    Title = "Sulama",
                    Date = today.AddDays(i * wateringFrequency),
                    Size = plant.Size
                });
            }

            // Gübreleme (ayda iki kez)
            for (int i = 1; i <= 6; i++)
            {
                events.Add(new CalendarEvent
                {
                    PlantId = plant.Id,
                    UserId = plant.UserId,
                    Title = "Gübreleme",
                    Date = today.AddMonths(i).AddDays(15),
                    Size = plant.Size
                });
            }

            // Toprak Değişimi (ayda bir kez)
            for (int i = 1; i <= 12; i++)
            {
                events.Add(new CalendarEvent
                {
                    PlantId = plant.Id,
                    UserId = plant.UserId,
                    Title = "Toprak Değişimi",
                    Date = today.AddMonths(i),
                    Size = plant.Size
                });
            }
            foreach (var ev in events)
            {
                Console.WriteLine($"Added Event: {ev.Title}, Date: {ev.Date}");
            }

            _context.CalendarEvents.AddRange(events);
            _context.SaveChanges();
        }

        // Kullanıcıya Ait Bitkileri Listele
        [HttpGet]
        public async Task<IActionResult> GetPlants([FromQuery] int userId)
        {
            var plants = await _context.Plants
                .Where(p => p.UserId == userId)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Type,
                    p.Size,
                    p.DateAdded
                })
                .ToListAsync();

            return Ok(plants);
        }
    }
}
