using GreenHealth_API_backend.Data;
using GreenHealth_API_backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenHealth_API_backend.Services
{
	public class SeasonService : ISeasonService
	{
		private readonly DataContext _context;

		public SeasonService(DataContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Season>> GetSeasons()
		{
			return await _context.Season.ToListAsync();
		}

		public async Task<Season> GetSeason(int id)
		{
			return await _context.Season.FindAsync(id);
		}

		public async Task<Season> PutSeason(int id, Season season)
		{
			_context.Entry(season).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
				return season;
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!SeasonExists(id))
				{
					return null;
				}
				else
				{
					throw;
				}
			}
		}

		public async Task<Season> PostSeason(Season season)
		{
			_context.Season.Add(season);
			await _context.SaveChangesAsync();
			return season;
		}

		public async Task<Season> DeleteSeason(int id)
		{
			var season = await _context.Season.FindAsync(id);

			if (season == null)
			{
				return null;
			}

			_context.Season.Remove(season);
			await _context.SaveChangesAsync();

			return season;
		}

		private bool SeasonExists(int id)
		{
			return _context.Season.Any(e => e.Id == id);
		}
	}
}
