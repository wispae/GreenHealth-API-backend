using GreenHealth_API_backend.Data;
using GreenHealth_API_backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenHealth_API_backend.Services
{
    public class PlantService : IPlantService
    {
        private readonly DataContext _context;

        public PlantService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Plant>> GetPlants(int id)
        {
			var user = await _context.User.FindAsync(id);
			if(user == null || user.OrganisationId == null)
			{
				return null;
			}
			
			var organisation = await _context.Organisation
				.Include(x => x.Plots)
				.ThenInclude(x => x.Plants)
				.SingleOrDefaultAsync(x => x.Id == user.OrganisationId );
			
			if(organisation == null)
			{
				return null;
			}

			List<Plant> plantList = new List<Plant>();
			foreach (Plot p in organisation.Plots)
			{
				plantList.AddRange(p.Plants);
			}

			return plantList;

        }

        public async Task<Plant> GetPlant(int id)
        {
            var plant = await _context.Plant.Include(x => x.Result).FirstOrDefaultAsync(x => x.Id == id);
			if(plant == null)
			{
				return null;
			}

			return plant;
        }

        public async Task<Plant> PutPlant(int id, Plant plant)
        {
            _context.Entry(plant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return plant;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlantExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<Plant> PostPlant(Plant plant)
        {
            _context.Plant.Add(plant);
            await _context.SaveChangesAsync();
            return plant;
        }

        public async Task<Plant> DeletePlant(int id)
        {
            var plant = await _context.Plant.FindAsync(id);

            if (plant == null)
            {
                return null;
            }

            _context.Plant.Remove(plant);
            await _context.SaveChangesAsync();

            return plant;
        }

        private bool PlantExists(int id)
        {
            return _context.Plant.Any(e => e.Id == id);
        }
    }
}
