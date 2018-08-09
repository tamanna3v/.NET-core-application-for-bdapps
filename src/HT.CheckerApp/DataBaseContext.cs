using HT.CheckerApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HT.CheckerApp.API
{
    public class DataBaseContext : DbContext
    {
        public DbSet<PBDrawResult> PBDrawResults { get; set; }
        public DbSet<PBOnDemand> PBOnDemand { get; set; }
        public DbSet<PBSubscriptions> PBSubscriptions { get; set; }
        public DbSet<PBSubscriptionRevenue> PBSubscriptionRevenues { get; set; }

        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        { }
    }
}
