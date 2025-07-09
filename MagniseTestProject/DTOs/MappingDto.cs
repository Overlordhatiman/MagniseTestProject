namespace MagniseTask.DTOs;

public class MappingDto
{
	public string Platform { get; set; } = string.Empty;
	public string Symbol { get; set; } = string.Empty;
	public string Exchange { get; set; } = string.Empty;
	public int? DefaultOrderSize { get; set; } = 0;
}