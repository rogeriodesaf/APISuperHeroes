using ApiSuperHeroes.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiSuperHeroes.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }

        public DbSet<Heroes> Heroes { get; set; }   
    }
}
