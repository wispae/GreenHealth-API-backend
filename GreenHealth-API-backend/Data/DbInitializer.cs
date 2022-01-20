using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenHealth_API_backend.Models;
using static GreenHealth_API_backend.Models.Result;

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

			// Add results

			context.AddRange(
				new Result { Accuracy = 92.6, Stage = GrowthStage.week1 },
				new Result { Accuracy = 91.2, Stage = GrowthStage.week2 },
				new Result { Accuracy = 87.98, Stage = GrowthStage.week3 },
				new Result { Accuracy = 93, Stage = GrowthStage.week4 },
				new Result { Accuracy = 997.009, Stage = GrowthStage.week5 },
				new Result { Accuracy = 95.1, Stage = GrowthStage.week6 }
				);

			//Add users

			context.AddRange(
				new User { FirstName = "Brent", LastName = "Van de Staey", Email = "brent@vandestaey.be", Password = "Brent1234", IsAdmin = true },
				new User { FirstName = "Toon", LastName = "De Bie", Email = "toon@debie.be", Password = "Toon1234", IsAdmin = false },
				new User { FirstName = "Wilfer", LastName = "Spaepen", Email = "Wilfer@spaepen.be", Password = "Wilfer1234", IsAdmin = false }
				);

			context.SaveChanges();

			// Add Plants

			Plant plant1 = new Plant()
			{
				UserId = 1,
				ResultId = 1,
			};

			Plant plant2 = new Plant()
			{
				UserId = 1,
				ResultId = 2,
			};

			Plant plant3 = new Plant()
			{
				UserId = 2,
				ResultId = 3,
			};

			Plant plant4 = new Plant()
			{
				UserId = 2,
				ResultId = 4,
			};

			Plant plant5 = new Plant()
			{
				UserId = 3,
				ResultId = 5,
			};

			Plant plant6 = new Plant()
			{
				UserId = 3,
				ResultId = 6,
			};

			context.Add(plant1);
			context.Add(plant2);
			context.Add(plant3);
			context.Add(plant4);
			context.Add(plant5);
			context.Add(plant6);

			context.SaveChanges();

			//success!
		}
	}
}
