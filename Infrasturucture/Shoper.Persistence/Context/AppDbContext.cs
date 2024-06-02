using Microsoft.EntityFrameworkCore;
using Shoper.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Persistence.Context
{
    public class AppDbContext :DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=MSA; database=Shoper;Integrated Security=True;TrustServerCertificate=True;");
        }
        public DbSet<Category> Categoeries{ get; set; }
    }
}
