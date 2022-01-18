using GreenHealth_API_backend.Data;
using GreenHealth_API_backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenHealth_API_backend.Services
{
    public class ResultService : IResultService
    {
        private readonly DataContext _context;

        public ResultService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Result>> GetResults()
        {
            return await _context.Result.ToListAsync();
        }

        public async Task<Result> GetResult(int id)
        {
            return await _context.Result.FindAsync(id);
        }

        public async Task<Result> PutResult(int id, Result result)
        {
            _context.Entry(result).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return result;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResultExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<Result> PostResult(Result result)
        {
            _context.Result.Add(result);
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<Result> DeleteResult(int id)
        {
            var result = await _context.Result.FindAsync(id);

            if (result == null)
            {
                return null;
            }

            _context.Result.Remove(result);
            await _context.SaveChangesAsync();

            return result;
        }

        private bool ResultExists(int id)
        {
            return _context.Result.Any(e => e.Id == id);
        }
    }
}
