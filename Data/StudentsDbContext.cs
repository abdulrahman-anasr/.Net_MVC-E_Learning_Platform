using Microsoft.EntityFrameworkCore;
using MvcE_Learning.Models;

namespace MvcE_Learning.Data
{
    public class StudentsDbContext : DbContext
    {
        public StudentsDbContext(DbContextOptions<StudentsDbContext> options)
        : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
    }
}