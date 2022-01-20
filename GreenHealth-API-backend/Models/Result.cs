using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenHealth_API_backend.Models
{
	public class Result
	{
		public int Id { get; set; }
		public GrowthStage Stage { get; set; }
#nullable enable
		public double? Accuracy { get; set; }
#nullable disable
		public enum GrowthStage
		{
			week1,
			week2,
			week3,
			week4,
			week5,
			week6
		}
	}
}
