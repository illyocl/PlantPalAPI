using System;

namespace PlantPalAPI.Models
{
    public class CalendarEvent
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Size { get; set; }
    }
}
