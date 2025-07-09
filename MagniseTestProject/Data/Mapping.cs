using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagniseTask.Data;

public class Mapping
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
	public int Id { get; set; }
	
	[Required]
	[MaxLength(50)]
	public string Platform { get; set; } = string.Empty;
	
	[Required]
	[MaxLength(50)]
	public string Symbol { get; set; } = string.Empty;

	[Required]
	[MaxLength(50)]
	public string Exchange { get; set; } = string.Empty;

	public int? DefaultOrderSize { get; set; } = 0;
	
	[ForeignKey("AssetId")]
	public int AssetId { get; set; }
	
	// Nav prop
	public Asset Asset { get; set; }
}