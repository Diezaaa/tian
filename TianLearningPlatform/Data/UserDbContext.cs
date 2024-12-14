using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tian_fullstack.Areas.Account.Models;

namespace Tian_fullstack.Data
{
    public class UserDbContext : IdentityDbContext
    {
        public UserDbContext(DbContextOptions options)
            : base(options)
            {
            }
        public DbSet<CompletedLesson> CompletedLessons { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
