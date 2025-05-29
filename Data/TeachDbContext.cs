using Microsoft.EntityFrameworkCore;
using MvcE_Learning.Models;

namespace MvcE_Learning.Data
{
    public class TeachDbContext : DbContext
    {
        public TeachDbContext(DbContextOptions<TeachDbContext> options)
        : base(options)
        {
        }

        public DbSet<Teach> Teach { get; set; }
    }
}