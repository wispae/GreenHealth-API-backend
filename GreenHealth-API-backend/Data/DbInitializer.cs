using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using GreenHealth_API_backend.Models;
using static GreenHealth_API_backend.Models.Result;
using System.Text;

namespace GreenHealth_API_backend.Data
{
	public class DbInitializer
	{
		public static void Initialize(DataContext context)
		{
			context.Database.EnsureCreated();

			// Look for any users.
			if (context.User.Any())
			{
				return;   // DB has been seeded
			}

			//Add users
			HashAlgorithm hashAlgorithm = SHA256.Create();

			hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes("Brent1234"));
			string bHash = Encoding.UTF8.GetString(hashAlgorithm.Hash);

			hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes("Toon1234"));
			string tHash = Encoding.UTF8.GetString(hashAlgorithm.Hash);

			hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes("Wilfer1234"));
			string wHash = Encoding.UTF8.GetString(hashAlgorithm.Hash);

			context.AddRange(
				new User
				{
					FirstName = "Brent",
					LastName = "Van de Staey",
					Email = "brent@vandestaey.be",
					Password = bHash,
					IsAdmin = true
				},
				new User
				{
					FirstName = "Toon",
					LastName = "De Bie",
					Email = "toon@debie.be",
					Password = tHash,
					IsAdmin = false
				},
				new User
				{
					FirstName = "Wilfer",
					LastName = "Spaepen",
					Email = "Wilfer@spaepen.be",
					Password = wHash,
					IsAdmin = false
				}
				);

			context.SaveChanges();

			// Add Plants

			Plant plant1 = new Plant()
			{
				UserId = 1,
				ImagePath = "u1p2.JPG"
			};

			Plant plant2 = new Plant()
			{
				UserId = 1			
			};

			Plant plant3 = new Plant()
			{
				UserId = 2			
			};

			Plant plant4 = new Plant()
			{
				UserId = 2
			};

			Plant plant5 = new Plant()
			{
				UserId = 3
			};

			Plant plant6 = new Plant()
			{
				UserId = 3
			};

			context.Add(plant1);
			context.Add(plant2);
			context.Add(plant3);
			context.Add(plant4);
			context.Add(plant5);
			context.Add(plant6);

			context.SaveChanges();

			// Add results

			Result result1 = new Result()
			{
				Accuracy = 92.6,
				GrowthStage = 1
			};

			Result result2 = new Result()
			{
				Accuracy = 91.2,
				GrowthStage = 2
			};

			Result result3 = new Result()
			{
				Accuracy = 87.98,
				GrowthStage = 3
			};

			Result result4 = new Result()
			{
				Accuracy = 93,
				GrowthStage = 4
			};

			Result result5 = new Result()
			{
				Accuracy = 97.009,
				GrowthStage = 5
			};

			Result result6 = new Result()
			{
				Accuracy = 95.1,
				GrowthStage = 6
			};

			context.Add(result1);
			context.Add(result2);
			context.Add(result3);
			context.Add(result4);
			context.Add(result5);
			context.Add(result6);

			context.SaveChanges();

			//success!
		}
	}
}
