﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenHealth_API_backend.Models
{
	public class Plant
	{
		public int Id { get; set; }
		public User User { get; set; }
		public int UserId { get; set; }
		public Result Result { get; set; }
		public int? ResultId { get; set; }
		public string ImagePath { get; set; }
		public string? Location { get; set; }
	}
}