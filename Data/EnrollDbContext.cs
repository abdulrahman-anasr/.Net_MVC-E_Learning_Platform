using Microsoft.EntityFrameworkCore;
using MvcE_Learning.Models;

namespace MvcE_Learning.Data
{
    public class EnrollDbContext : DbContext
    {
        public EnrollDbContext(DbContextOptions<EnrollDbContext> options)
        : base(options)
        {
        }

        public DbSet<Enroll> Enroll { get; set; }
    }
}