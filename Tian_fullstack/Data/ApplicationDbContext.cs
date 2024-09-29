using Microsoft.EntityFrameworkCore;
using Tian_fullstack.Models;

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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lesson>().HasData(
                new Lesson
                {
                    Id = 1,
                    Order = 1,
                    Title = "Hello world",
                },
                new Lesson
                {
                    Id = 2,
                    Order =2,
                    Title = "AFSSAFFSAFAFS"
                });
            modelBuilder.Entity<Slide>().HasData(
                new Slide
                {
                    Id = 1,
                    LessonId = 1,  // Reference to the "Hello World" lesson
                    Content = "What's the color of the sky",
                    Options = new List<string> { "Blue", "Red", "Brown" },
                    CorrectOptionIndex = 0
                }
            );

        }

    }
}
