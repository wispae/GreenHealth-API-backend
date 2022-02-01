using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenHealth_API_backend.Models;

namespace GreenHealth_API_backend.Services
{
	public interface IPlotService
	{
		Task<Plot> GetPlot(int id);
		Task<Plot> PutPlot(int id, Plot plot);
		Task<Plot> PostPlot(Plot plot, int userId);
		Task<Plot> DeletePlot(int id);
		Task<IEnumerable<Plot>> GetPlots(int id);
	}
}
