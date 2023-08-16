using Microsoft.EntityFrameworkCore;
using RecruitmentTask.Entities;

namespace RecruitmentTask.Data
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Contact> Contacts { get; set; }
    }
}
