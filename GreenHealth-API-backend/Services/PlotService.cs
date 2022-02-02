using GreenHealth_API_backend.Data;
using GreenHealth_API_backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenHealth_API_backend.Services
{
	public class PlotService : IPlotService
	{
		private readonly DataContext _context;

		public PlotService(DataContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Plot>> GetPlots(int id)
		{
			var user = await _context.User
				.Include(x => x.Organisation)
				.ThenInclude(x => x.Plots)
				.FirstOrDefaultAsync(x => x.Id == id);
			if(user == null)
			{
				return null;
			}
			return user.Organisation.Plots;
		}

		public async Task<Plot> GetPlot(int id)
		{
			return await _context.Plot.FindAsync(id);
		}

		public async Task<IEnumerable<Plant>> GetPlotPlants(int uid, int pid)
		{
			var user = await _context.User.FindAsync(uid);
			if(user == null)
			{
				return null;
			}

			var plot = await _context.Plot
				.Include(x => x.Plants)
				.FirstOrDefaultAsync(x => x.Id == pid);
			if(plot == null || plot.OrganisationId != user.OrganisationId)
			{
				return null;
			}

			return plot.Plants;
		}

		public async Task<Plot> PutPlot(int id, Plot plot)
		{
			_context.Entry(plot).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
				return plot;
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!PlotExists(id))
				{
					return null;
				}
				else
				{
					throw;
				}
			}
		}

		public async Task<Plot> PostPlot(Plot plot, int uid)
		{
			var user = _context.User.Find(uid);
			if(user == null)
			{
				return null;
			} else if(user.OrganisationId == null) {
				return null;
			}
			plot.OrganisationId = (int)user.OrganisationId;
			_context.Plot.Add(plot);
			await _context.SaveChangesAsync();
			return plot;
		}

		public async Task<Plot> DeletePlot(int id)
		{
			var plot = await _context.Plot.FindAsync(id);

			if (plot == null)
			{
				return null;
			}

			_context.Plot.Remove(plot);
			await _context.SaveChangesAsync();

			return plot;
		}

		private bool PlotExists(int id)
		{
			return _context.Plot.Any(e => e.Id == id);
		}
	}
}
