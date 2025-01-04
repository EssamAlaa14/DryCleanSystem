using AutoMapper;
using Domain.Entities;

namespace Application.Common.DTOs.InfoDtos
{
    public class CustomerDto(
        Guid id,
        string name,
        string primaryPhone,
        string? secondaryPhone,
        DateTime createdAt
        )
    {
        public Guid Id { get; set; } = id;
        public string Name { get; set; } = name;
        public string PrimaryPhone { get; set; } = primaryPhone;
        public string? SecondaryPhone { get; set; } = secondaryPhone;
        public DateTime CreatedAt { get; set; } = createdAt;

        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Customer, CustomerDto>();
            }
        }
    }
}
