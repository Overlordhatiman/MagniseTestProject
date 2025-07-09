using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MagniseTask.Data.Configurations;

public class AssetConfig : IEntityTypeConfiguration<Asset>
{
	public void Configure(EntityTypeBuilder<Asset> builder)
	{
		builder.ToTable("Assets");

		builder.HasKey(a => a.Id);

		builder.Property(a => a.Id)
			.IsRequired()
			.ValueGeneratedOnAdd();

		builder.Property(a => a.InstrumentId)
			.IsRequired()
			.HasMaxLength(50);

		builder.Property(a => a.Symbol)
			.IsRequired()
			.HasMaxLength(20);

		builder.Property(a => a.Kind)
			.IsRequired()
			.HasMaxLength(20);

		builder.Property(a => a.Exchange)
			.HasMaxLength(50);

		builder.Property(a => a.Description)
			.HasMaxLength(250);

		builder.Property(a => a.Currency)
			.IsRequired()
			.HasMaxLength(10);

		builder.Property(a => a.BaseCurrency)
			.HasMaxLength(10);
		
		builder.Property(a => a.TickSize)
			.HasPrecision(18, 5);

		builder.HasMany(a => a.Mappings)
			.WithOne(m => m.Asset)
			.HasForeignKey(m => m.AssetId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}