using Microsoft.EntityFrameworkCore;
using Tian_fullstack.Areas.Learning.Models;

namespace Tian_fullstack.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets for your entities (tables)
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Slide> Slides { get; set; }
        }

    }

