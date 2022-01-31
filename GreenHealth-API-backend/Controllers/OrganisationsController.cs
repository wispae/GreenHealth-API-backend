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
using System.Security.Claims;

namespace GreenHealth_API_backend.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class OrganisationsController : ControllerBase
	{
		private readonly DataContext _context;
		private readonly OrganisationService _organisationService;

		public OrganisationsController(DataContext context, OrganisationService service)
		{
			_context = context;
			_organisationService = service;
		}

		// GET: api/Organisations
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Organisation>>> GetOrganisations()
		{
			return Ok(await _organisationService.GetOrganisations());
		}

		// GET: api/Organisations/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Organisation>> GetOrganisation(int id)
		{
			try
			{
				var organisation = await _organisationService.GetOrganisation(id);

				if (organisation == null)
				{
					return NotFound();
				}

				return Ok(organisation);
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
			}
		}

		[HttpGet("{id}/plots")]
		public async Task<ActionResult<IEnumerable<Plot>>> GetOrganisationPlots(int id)
		{
			try
			{
				var organisation = await _organisationService.GetOrganisationWithPlots(id);

				if (organisation == null)
				{
					return NotFound();
				}

				organisation.Plots.AsParallel().ForAll(p => p.Organisation = null);

				return Ok(organisation.Plots);
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
			}
		}

		// PUT: api/Organisations/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutOrganisation(int id, Organisation organisation)
		{
			if (id != organisation.Id)
			{
				return BadRequest();
			}

			try
			{
				var organisationresult = await _organisationService.PutOrganisation(id, organisation);
				if (organisationresult == null)
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

		// POST: api/Organisations
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Organisation>> PostOrganisation(Organisation organisation)
		{
			try
			{
				var organisationresult = await _organisationService.PostOrganisation(organisation);
				return CreatedAtAction("GetOrganisation", new { id = organisationresult.Id }, organisationresult);
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error posting data to the database");
			}
		}

		// DELETE: api/Organisations/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteOrganisation(int id)
		{
			try
			{
				var organisation = await _organisationService.DeleteOrganisation(id);
				if (organisation == null)
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
