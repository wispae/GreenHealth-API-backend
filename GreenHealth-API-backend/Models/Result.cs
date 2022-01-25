using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GreenHealth_API_backend.Models
{
	public class Result
	{
		[ForeignKey("Plant")]
		public int Id { get; set; }
        public int GrowthStage { get; set; }
#nullable enable
		public double? Accuracy { get; set; }
#nullable disable
	}
}
