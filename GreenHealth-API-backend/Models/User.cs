using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
		public string Address { get; set; }
		public string Password { get; set; }
		public bool IsAdmin { get; set; }
		public ICollection<Plant> Plants { get; set; }
		[NotMapped]
		public string Token { get; set; }

		public User()
		{
			Id = 0;
			FirstName = "";
			LastName = "";
			Email = "";
			Address = "";
			Password = "";
			IsAdmin = false;
		}
	}
}
