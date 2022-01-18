using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenHealth_API_backend.Models;

namespace GreenHealth_API_backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public DbSet<User> User { get; set; }
        public DbSet<Plant> Plant { get; set; }
        public DbSet<Result> Result { get; set; }
    }
}
