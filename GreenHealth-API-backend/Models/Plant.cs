using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenHealth_API_backend.Models
{
	public class Plant
	{
		public int Id { get; set; }
		public int PlotId { get; set; }
#nullable enable
		public int? ResultId { get; set; }
		public Result? Result { get; set; }
		public int? SeasonId { get; set; }
		public Season? Season { get; set; }
		public string? Location { get; set; }
		public string? Timestamp { get; set; }
#nullable disable
		public string ImagePath { get; set; }

		public Plot Plot { get; set; }
	}
}
