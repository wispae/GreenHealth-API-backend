using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GreenHealth_API_backend.Models
{
	public class Organisation
	{
		public int Id { get; set; }
		public int OwnerId { get; set; }
		public string Name { get; set; }

		[InverseProperty("Organisation")]
		public ICollection<User> Users { get; set; }
		[ForeignKey("OwnerId")]
		public User Owner { get; set; }
		[InverseProperty("Organisation")]
		public ICollection<Plot> Plots { get; set; }
	}
}
