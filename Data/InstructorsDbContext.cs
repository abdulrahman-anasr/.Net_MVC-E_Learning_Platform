using Microsoft.EntityFrameworkCore;
using MvcE_Learning.Models;

namespace MvcE_Learning.Data
{
    public class InstructorsDbContext : DbContext
    {
        public InstructorsDbContext(DbContextOptions<InstructorsDbContext> options)
        : base(options)
        {
        }

        public DbSet<Instructor> Instructors { get; set; }
    }
}