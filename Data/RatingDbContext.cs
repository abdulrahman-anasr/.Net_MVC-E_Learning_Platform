using Microsoft.EntityFrameworkCore;
using MvcE_Learning.Models;

namespace MvcE_Learning.Data
{
    public class RatingDbContext : DbContext
    {
        public RatingDbContext(DbContextOptions<RatingDbContext> options)
        : base(options)
        {
        }

        public DbSet<Rating> Rating { get; set; }
    }
}