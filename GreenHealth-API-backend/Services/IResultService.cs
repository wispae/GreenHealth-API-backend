using GreenHealth_API_backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenHealth_API_backend.Services
{
    public interface IResultService
    {
        Task<Result> DeleteResult(int id);
        Task<Result> GetResult(int id);
        Task<IEnumerable<Result>> GetResults();
        Task<Result> PostResult(Result result);
        Task<Result> PutResult(int id, Result result);
    }
}