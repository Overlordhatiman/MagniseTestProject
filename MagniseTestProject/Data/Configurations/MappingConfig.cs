using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MagniseTask.Data.Configurations;

public class MappingConfig : IEntityTypeConfiguration<Mapping>
{
	public void Configure(EntityTypeBuilder<Mapping> builder)
	{
		builder.ToTable("Mappings");

		builder.HasKey(m => m.Id);

		builder.Property(m => m.Id)
			.IsRequired()
			.ValueGeneratedOnAdd();

		builder.Property(m => m.Symbol)
			.IsRequired()
			.HasMaxLength(20);

		builder.Property(m => m.Exchange)
			.HasMaxLength(50);
		
		builder.Property(m => m.DefaultOrderSize)
			.IsRequired()
			.HasDefaultValue(0);
		
		builder.HasOne(m => m.Asset)
			.WithMany(a => a.Mappings)
			.HasForeignKey(m => m.AssetId);
	}
}