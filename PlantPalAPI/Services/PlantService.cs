using PlantPalAPI.Data;
using PlantPalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlantPalAPI.Services
{
    public class PlantService
    {
        private readonly AppDbContext _context;

        public PlantService(AppDbContext context)
        {
            _context = context;
        }

        
        public List<Plant> GetAllPlants()
        {
            try
            {
                // Tüm bitkileri al
                var plants = _context.Plants.ToList();
                return plants ?? new List<Plant>();
            }
            catch (Exception ex)
            {
                throw new Exception("Bitkiler alınırken bir hata oluştu.", ex);
            }
        }

        // Belirli bir kullanıcıya ait bitkileri al 
        public List<Plant> GetPlantsByUserId(int userId)
        {
            try
            {
               
                var plants = _context.Plants.Where(p => p.UserId == userId).ToList();
                return plants ?? new List<Plant>();
            }
            catch (Exception ex)
            {
                throw new Exception("Kullanıcıya ait bitkiler alınırken bir hata oluştu.", ex);
            }
        }

        // Bitki ekle (Add plant)
        public void AddPlant(Plant plant)
        {
            try
            {
                _context.Plants.Add(plant);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Bitki eklenirken bir hata oluştu.", ex);
            }
        }
    }
}
