using Microsoft.EntityFrameworkCore;
using Client.Entities;

namespace Client.Database
{
    public class SquigglyContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<VirtualServer> VirtualServers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder oB) 
        {
            oB.UseSqlite("Data Source=Squiggly.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
