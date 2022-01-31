using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenHealth_API_backend.Models;

namespace GreenHealth_API_backend.Services
{
	public interface IOrganisationService
	{
		Task<Organisation> GetOrganisation(int id);
		Task<Organisation> PutOrganisation(int id, Organisation organisation);
		Task<Organisation> PostOrganisation(Organisation organisation);
		Task<Organisation> DeleteOrganisation(int id);
		Task<IEnumerable<Organisation>> GetOrganisations();
		Task<Organisation> GetOrganisationWithPlots(int id);
	}
}
