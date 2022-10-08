
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OpahQueue.Models;
using OpahQueue.Data;

namespace OpahQueue.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        public DbSet<OpahQueue.Models.Image> Images { get; set; } = default!;

        public DbSet<OpahQueue.Models.User> Users { get; set; } = default!;

        public DbSet<OpahQueue.Models.TIatende> TIatende { get; set; } = default!;

        public DbSet<OpahQueue.Models.Exceller> Exceller { get; set; } = default!;

        public DbSet<OpahQueue.Models.Report> Report { get; set; } = default!;
    }
}
