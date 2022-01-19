using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenHealth_API_backend.Models
{
	public class Result
	{
		public int Id { get; set; }
#nullable enable
		public double? Accuracy { get; set; }
#nullable disable
		public int GrowthStage { get; set; }
	}
}
