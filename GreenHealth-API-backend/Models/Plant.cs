using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenHealth_API_backend.Models
{
	public class Plant
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
#nullable enable
		public int? ResultId { get; set; }
		public Result? Result { get; set; }
		public string? Location { get; set; }
#nullable disable
		public string ImagePath { get; set; }
	}
}
