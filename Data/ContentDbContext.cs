using Microsoft.EntityFrameworkCore;
using MvcE_Learning.Models;

namespace MvcE_Learning.Data
{
    public class ContentDbContext : DbContext
    {
        public ContentDbContext(DbContextOptions<ContentDbContext> options)
        : base(options)
        {
        }

        public DbSet<Content> Content { get; set; }
    }
}