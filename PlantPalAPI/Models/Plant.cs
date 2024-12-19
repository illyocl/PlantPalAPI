using System;

namespace PlantPalAPI.Models
{
    public class Plant
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public string Name { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public DateTime DateAdded { get; set; } 
    }
}
