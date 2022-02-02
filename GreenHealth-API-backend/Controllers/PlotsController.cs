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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GreenHealth_API_backend.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class PlotsController : ControllerBase
	{
		private readonly DataContext _context;
		private readonly PlotService _plotService;

		public PlotsController(DataContext context, PlotService service)
		{
			_context = context;
			_plotService = service;
		}

		// GET: api/Plots
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Plot>>> GetPlots()
		{
			var result = new JsonResult(from c in User.Claims
										select new
										{
											c.Type,
											c.Value
										});
			var userClaimId = User.Claims.Single(c => c.Type == "Id").Value;
			if (userClaimId == null || userClaimId == "")
			{
				return StatusCode(StatusCodes.Status401Unauthorized, "You are not logged in");
			}
			try
			{
				var plots = await _plotService.GetPlots(int.Parse(userClaimId));
				if (plots == null)
				{
					return NotFound();
				}
				plots.AsParallel().ForAll(p => { p.Organisation.Plots = null; p.Organisation.Users = null; });
				return Ok(plots);
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
			}
		}

		// GET: api/Plots/5/Plants
		[HttpGet("{id}/Plants")]
		public async Task<ActionResult<IEnumerable<Plant>>> GetPlotPlants(int id)
		{
			var result = new JsonResult(from c in User.Claims
										select new
										{
											c.Type,
											c.Value
										});
			var userClaimId = User.Claims.Single(c => c.Type == "Id").Value;
			if (userClaimId == null || userClaimId == "")
			{
				return StatusCode(StatusCodes.Status401Unauthorized, "You are not logged in");
			}

			var plantList = await _plotService.GetPlotPlants(int.Parse(userClaimId), id);

			plantList.AsParallel().ForAll(x => x.Plot = null);

			return Ok(plantList);
		}

		// GET: api/Plots/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Plot>> GetPlot(int id)
		{
			try
			{
				var plot = await _plotService.GetPlot(id);

				if (plot == null)
				{
					return NotFound();
				}

				return Ok(plot);
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
			}
		}

		// PUT: api/Plots/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutPlot(int id, Plot plot)
		{
			if (id != plot.Id)
			{
				return BadRequest();
			}

			try
			{
				var plotresult = await _plotService.PutPlot(id, plot);
				if (plotresult == null)
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

		// POST: api/Plots
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Plot>> PostPlot(Plot plot)
		{
			var result = new JsonResult(from c in User.Claims
										select new
										{
											c.Type,
											c.Value
										});
			var userClaimId = User.Claims.Single(c => c.Type == "Id").Value;
			if (userClaimId == null || userClaimId == "")
			{
				return StatusCode(StatusCodes.Status401Unauthorized, "You are not logged in");
			}
			try
			{
				var plotresult = await _plotService.PostPlot(plot, int.Parse(userClaimId));
				if(plotresult == null)
				{
					return StatusCode(StatusCodes.Status500InternalServerError, "Error posting data to the database");
				}
				return CreatedAtAction("GetPlot", new { id = plotresult.Id }, plotresult);
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error posting data to the database");
			}
		}

		// DELETE: api/Plots/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeletePlot(int id)
		{
			try
			{
				var plot = await _plotService.DeletePlot(id);
				if (plot == null)
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
