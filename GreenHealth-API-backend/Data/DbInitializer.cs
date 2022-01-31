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
					FirstName = "Toon",
					LastName = "De Bie",
					Email = "toon@debie.be",
					Password = tHash,
					IsAdmin = false
				}
				);

			context.SaveChanges();

			context.Add(
				new Organisation
				{
					OwnerId = context.User.Single(u => u.FirstName == "Toon").Id,
					Name = "Green Health"
				}
			);

			context.SaveChanges();

			context.User.Find(1).OrganisationId = 1;

			context.AddRange(
				new User
				{
					OrganisationId = 1,
					FirstName = "Brent",
					LastName = "Van de Staey",
					Email = "brent@vandestaey.be",
					Password = bHash,
					IsAdmin = true
				},
				new User
				{
					OrganisationId = 1,
					FirstName = "Wilfer",
					LastName = "Spaepen",
					Email = "Wilfer@spaepen.be",
					Password = wHash,
					IsAdmin = false
				});

			context.SaveChanges();

			context.AddRange(
				new Plot
				{
					OrganisationId = context.Organisation.Single(o => o.Name == "Green Health").Id,
					Location = "De Campus"
				}
				);

			context.SaveChanges();

			// Add Plants

			Plant plant1 = new Plant()
			{
				PlotId = 1,
				ImagePath = "u1p2.JPG"
			};

			Plant plant2 = new Plant()
			{
				PlotId = 1
			};

			Plant plant3 = new Plant()
			{
				PlotId = 1
			};

			context.Add(plant1);
			context.Add(plant2);
			context.Add(plant3);

			context.SaveChanges();

			//success!
		}
	}
}
