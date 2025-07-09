using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagniseTask.Data;

public class Asset
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	[Required]
	[MaxLength(50)]
	public string InstrumentId { get; set; } = string.Empty;

	[Required]
	[MaxLength(50)]
	public string Symbol { get; set; } = string.Empty;

	[Required]
	[MaxLength(50)]
	public string Kind { get; set; } = string.Empty;

	[MaxLength(50)]
	public string? Exchange { get; set; } = string.Empty;

	[MaxLength(100)]
	public string Description { get; set; } = string.Empty;

	[Column(TypeName = "decimal(18, 5)")]
	public decimal TickSize { get; set; } = decimal.Zero;

	[Required]
	[MaxLength(50)]
	public string Currency { get; set; } = string.Empty;

	[MaxLength(50)]
	public string? BaseCurrency { get; set; } = string.Empty;

	// Initialize navigation property
	public List<Mapping> Mappings { get; set; } = new List<Mapping>();
}

	