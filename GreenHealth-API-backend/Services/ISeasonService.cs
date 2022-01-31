using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenHealth_API_backend.Models;

namespace GreenHealth_API_backend.Services
{
	public interface ISeasonService
	{
		Task<Season> GetSeason(int id);
		Task<Season> PutSeason(int id, Season season);
		Task<Season> PostSeason(Season season);
		Task<Season> DeleteSeason(int id);
		Task<IEnumerable<Season>> GetSeasons();
	}
}
