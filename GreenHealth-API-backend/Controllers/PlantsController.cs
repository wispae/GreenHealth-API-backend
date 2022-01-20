using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using GreenHealth_API_backend.Data;
using GreenHealth_API_backend.Models;
using GreenHealth_API_backend.Services;

namespace GreenHealth_API_backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]

	public class PlantsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly PlantService _plantService;
		private string _blobConnectionString;

		public PlantsController(DataContext context, PlantService service, IConfiguration configuration)
        {
            _context = context;
            _plantService = service;
			_blobConnectionString = configuration.GetConnectionString("BlobConnection");
        }

        // GET: api/Plants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plant>>> GetPlant()
        {
            return Ok(await _plantService.GetPlants());
        }

        // GET: api/Plants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Plant>> GetPlant(int id)
        {
            try
            {
                var plant = await _plantService.GetPlant(id);

                if (plant == null)
                {
                    return NotFound();
                }

                return Ok(plant);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }  
        }

		[HttpGet("{id}/image")]
		public async Task<ActionResult> GetPlantImage(int id)
		{
			try
			{
				var plant = await _plantService.GetPlant(id);

				if (plant == null)
				{
					return NotFound();
				}

				BlobClient blobClient = new BlobClient(_blobConnectionString, "greenhealth", plant.ImagePath);
				BlobDownloadResult result = await blobClient.DownloadContentAsync();
				return new FileContentResult(result.Content.ToArray(), "image/jpg");

			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
			}
		}

        // PUT: api/Plants/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlant(int id, Plant plant)
        {
            if (id != plant.Id)
            {
                return BadRequest();
            }

            try
            {
                var plantresult = await _plantService.PutPlant(id, plant);
                if (plantresult == null)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error writing changes to the database");
            }
        }

		// PATCH: api/Plants/5/image
		[HttpPatch("{id}/image")]
		public async Task<ActionResult<Plant>> PatchPlant(int id, IFormFile image)
		{
			try
			{
				var plantResult = await _plantService.GetPlant(id);

				if(plantResult == null)
				{
					return NotFound();
				}

				string imageName = "u" + plantResult.UserId.ToString() + "p" + plantResult.Id + ".JPG";
				BlobClient blobClient = new BlobClient(_blobConnectionString, "greenhealth", imageName);
				await blobClient.UploadAsync(image.OpenReadStream());

				plantResult.ImagePath = imageName;

				await _plantService.PutPlant(id, plantResult);
				return Ok(plantResult);
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
			}
		}

        // POST: api/Plants
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Plant>> PostPlant(Plant plant)
        {
            try
            {
                var plantresult = await _plantService.PostPlant(plant);
                return CreatedAtAction("GetPlant", new { id = plantresult.Id }, plantresult);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error posting data to the database");
            }
        }

        // DELETE: api/Plants/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlant(int id)
        {
            try
            {
                var plant = await _plantService.DeletePlant(id);
                if (plant == null)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data from the database");
            }
        }
    }
}
