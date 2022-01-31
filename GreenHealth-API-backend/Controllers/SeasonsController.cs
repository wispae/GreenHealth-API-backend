using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GreenHealth_API_backend.Data;
using GreenHealth_API_backend.Models;
using GreenHealth_API_backend.Services;
using Microsoft.AspNetCore.Authorization;

namespace GreenHealth_API_backend.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class SeasonsController : ControllerBase
	{
		private readonly DataContext _context;
		private readonly SeasonService _seasonService;

		public SeasonsController(DataContext context, SeasonService service)
		{
			_context = context;
			_seasonService = service;
		}

		// GET: api/Seasons
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Season>>> GetSeasons()
		{
			return Ok(await _seasonService.GetSeasons());
		}

		// GET: api/Seasons/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Season>> GetSeason(int id)
		{
			try
			{
				var season = await _seasonService.GetSeason(id);

				if (season == null)
				{
					return NotFound();
				}

				return Ok(season);
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
			}
		}

		// PUT: api/Seasons/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutSeason(int id, Season season)
		{
			if (id != season.Id)
			{
				return BadRequest();
			}

			try
			{
				var seasonresult = await _seasonService.PutSeason(id, season);
				if (seasonresult == null)
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

		// POST: api/Seasons
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Season>> PostSeason(Season season)
		{
			try
			{
				var seasonresult = await _seasonService.PostSeason(season);
				return CreatedAtAction("GetSeason", new { id = seasonresult.Id }, seasonresult);
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error posting data to the database");
			}
		}

		// DELETE: api/Seasons/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteSeason(int id)
		{
			try
			{
				var season = await _seasonService.DeleteSeason(id);
				if (season == null)
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
