using AutoMapper;
using Domain.Entities;

namespace Application.Common.DTOs.InfoDtos
{
    public class DistrictDto(
        Guid id,
        string name
        )
    {
        public Guid Id { get; set; } = id;
        public string Name { get; set; } = name;

        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<District, DistrictDto>();
            }
        }
    }
}
