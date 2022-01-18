using GreenHealth_API_backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenHealth_API_backend.Services
{
    public interface IPlantService
    {
        Task<Plant> DeletePlant(int id);
        Task<Plant> GetPlant(int id);
        Task<IEnumerable<Plant>> GetPlants();
        Task<Plant> PostPlant(Plant plant);
        Task<Plant> PutPlant(int id, Plant plant);
    }
}