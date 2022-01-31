using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GreenHealth_API_backend.Models
{
	public class Plot
	{
		public int Id { get; set; }
		public int OrganisationId { get; set; }
		public string Location { get; set; }

		public Organisation Organisation { get; set; }
		[InverseProperty("Plot")]
		public ICollection<Plant> Plants { get; set; }
	}
}
