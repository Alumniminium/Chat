using Microsoft.EntityFrameworkCore;
using Server.Database.Models;

namespace Server.Database
{
    public class SquigglyContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Models.VirtualServer> VirtualServers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder oB) => oB.UseSqlite("Data Source=Squiggly.db");
    }
}
