using Microsoft.EntityFrameworkCore;
using MiniProject____API.Models;

namespace MiniProject____API.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<Person> People { get; set; }
        public DbSet<Interest> Interests { get; set; }
        public DbSet<InterestLink> InterestsLinks { get; set; }    
       

    }
}
