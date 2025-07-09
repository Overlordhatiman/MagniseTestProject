using System.Text.Json.Serialization;

namespace MagniseTask.DTOs;

public class AssetDto
{
	[JsonPropertyName("id")]
	public string InstrumentId { get; set; }
	public string Symbol { get; set; }
	public string Kind { get; set; }
	public string Exchange { get; set; }
	public string Description { get; set; }
	public decimal TickSize { get; set; }
	public string Currency { get; set; }
	[JsonPropertyName("baseCurrency")]
	public string? BaseCurrency { get; set; }
	public Dictionary<string, MappingDto> Mappings { get; set; }
}