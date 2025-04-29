using BlazorApp.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ScreenDefinition> ScreenDefinitions { get; set; }
    }
}
