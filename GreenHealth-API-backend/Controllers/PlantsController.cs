﻿using System;
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
using System.Net.Http;
using Newtonsoft.Json;
using static GreenHealth_API_backend.Models.Result;

namespace GreenHealth_API_backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]

	public class PlantsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly PlantService _plantService;
        private readonly ResultService _resultService;
		private string _blobConnectionString;
        private string _apiConntectionString;

        public PlantsController(DataContext context, PlantService service, ResultService resultService, IConfiguration configuration)
        {
            _context = context;
            _plantService = service;
            _resultService = resultService;
            _blobConnectionString = configuration.GetConnectionString("BlobConnection");
            _apiConntectionString = configuration.GetConnectionString("ApiConnection");
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

        // GET: api/Plants/5/Result
        [HttpGet("{id}/result")]
        public async Task<ActionResult<Result>> GetPlantResult(int id)
        {
            try
            {
                var plant = await _plantService.GetPlant(id);

                if (plant.ImagePath == null && plant.Result == null)
                {
                    return NotFound();
                }
                else if (plant.ImagePath != null && plant.Result == null)
                {
                    string imageUrl = "https://storagemainfotosplanten.blob.core.windows.net/greenhealth/" + plant.ImagePath;
                    string url = _apiConntectionString + "/?link=" + imageUrl;
                    var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync(url);
                    var result = await response.Content.ReadAsStringAsync();

                    AiResult jsonResult = JsonConvert.DeserializeObject<AiResult>(result);

                    Result putResult = new Result();
                    putResult.Accuracy = Convert.ToDouble(jsonResult.accuracy);
                    putResult.Stage = (GrowthStage)Convert.ToInt32(jsonResult.output);

                    try
                    {
                        var resresult = await _resultService.PostResult(putResult);

                        plant.Result = resresult;
                        plant.ResultId = resresult.Id;

                        return CreatedAtAction("GetResult", new { id = resresult.Id }, resresult);
                    }
                    catch (Exception)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, "Error posting data to the database");
                    }
                }
                else if (plant.Result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(plant.Result);
                }    
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
