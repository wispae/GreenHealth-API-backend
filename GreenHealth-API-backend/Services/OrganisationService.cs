using GreenHealth_API_backend.Data;
using GreenHealth_API_backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenHealth_API_backend.Services
{
	public class OrganisationService : IOrganisationService
	{
		private readonly DataContext _context;

		public OrganisationService(DataContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Organisation>> GetOrganisations()
		{
			return await _context.Organisation.ToListAsync();
		}

		public async Task<Organisation> GetOrganisation(int id)
		{
			return await _context.Organisation.FindAsync(id);
		}

		public async Task<Organisation> GetOrganisationWithPlots(int id)
		{
			return await _context.Organisation.Include(o => o.Plots).SingleOrDefaultAsync(o => o.Id == id);
		}

		public async Task<Organisation> PutOrganisation(int id, Organisation organisation)
		{
			_context.Entry(organisation).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
				return organisation;
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!OrganisationExists(id))
				{
					return null;
				}
				else
				{
					throw;
				}
			}
		}

		public async Task<Organisation> PostOrganisation(Organisation organisation)
		{
			_context.Organisation.Add(organisation);
			await _context.SaveChangesAsync();
			return organisation;
		}

		public async Task<Organisation> DeleteOrganisation(int id)
		{
			var organisation = await _context.Organisation.FindAsync(id);

			if (organisation == null)
			{
				return null;
			}

			_context.Organisation.Remove(organisation);
			await _context.SaveChangesAsync();

			return organisation;
		}

		private bool OrganisationExists(int id)
		{
			return _context.Organisation.Any(e => e.Id == id);
		}
	}
}
