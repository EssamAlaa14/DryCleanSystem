using AutoMapper;
using Domain.Entities.Orders;

namespace Application.Common.DTOs.InfoDtos
{
    public class CleaningTypeDto(
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
                CreateMap<CleaningType, CleaningTypeDto>();
            }
        }
    }
}
