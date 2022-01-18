using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenHealth_API_backend.Models
{
	public class Plant
	{
		public int ID { get; set; }
		public int UserID { get; set; }
		public int? ResultID { get; set; }
		public string ImagePath { get; set; }
		public string? Location { get; set; }
	}
}
