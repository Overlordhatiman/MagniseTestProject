using MagniseTask.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace MagniseTask.Data;

public class MarketDbContext : DbContext
{
	public MarketDbContext(DbContextOptions<MarketDbContext> options) : base(options) { }
	
	public DbSet<Asset> Assets { get; set; } 
	public DbSet<Mapping> Mappings { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfiguration(new AssetConfig());
		modelBuilder.ApplyConfiguration(new MappingConfig());
	}
}