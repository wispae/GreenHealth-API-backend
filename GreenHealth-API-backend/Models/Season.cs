using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GreenHealth_API_backend.Models
{
	public class Season
	{
		public int Id { get; set; }
		public string Name { get; set; }
#nullable enable
		public string? StartDate { get; set; }
		public string? EndDate { get; set; }
#nullable disable
		public ICollection<Plant> Plants { get; set; }
	}
}
