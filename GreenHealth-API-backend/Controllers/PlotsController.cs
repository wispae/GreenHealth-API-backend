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
			return Ok(await _plotService.GetPlots());
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
			try
			{
				var plotresult = await _plotService.PostPlot(plot);
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
