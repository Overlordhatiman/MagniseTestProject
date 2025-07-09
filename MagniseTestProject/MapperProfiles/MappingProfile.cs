using AutoMapper;
using MagniseTask.Data;
using MagniseTask.DTOs;

namespace MagniseTask.Mappers;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<Mapping, MappingDto>()
			.ReverseMap();
	}
}