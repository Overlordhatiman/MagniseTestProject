using AutoMapper;
using MagniseTask.Data;
using MagniseTask.DTOs;

namespace MagniseTask.MapperProfiles;

public class AssetProfile : Profile
{
	public AssetProfile()
	{
		CreateMap<AssetDto, Asset>()
			.ForMember(dest => dest.Mappings, opt => opt.MapFrom(src => src.Mappings.Select(m => new Mapping
			{
				Platform = m.Key,
				Symbol = m.Value.Symbol,
				Exchange = m.Value.Exchange,
				DefaultOrderSize = m.Value.DefaultOrderSize,
			}).ToList()));
		
		CreateMap<Asset, AssetDto>()
			.ForMember(dest => dest.Mappings, opt => opt.MapFrom(src => src.Mappings.ToDictionary(m => m.Platform, m => new MappingDto
			{
				Symbol = m.Symbol,
				Exchange = m.Exchange,
				DefaultOrderSize = m.DefaultOrderSize
			})));
	}
}