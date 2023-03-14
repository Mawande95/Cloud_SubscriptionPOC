using Entities;
using Microsoft.EntityFrameworkCore;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace CloudSubscriptionAPI.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<CloudServices> CloudServices { get; set; }
        public DbSet<Users> Users { get; set; }
    }
}
