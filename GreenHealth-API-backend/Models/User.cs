using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenHealth_API_backend.Models
{
	public class User
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
#nullable enable
		public string? Address { get; set; }
		public ICollection<Plant>? Plants { get; set; }
#nullable disable
		public string Password { get; set; }
		public bool IsAdmin { get; set; }
		
	}
}
