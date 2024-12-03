using AutoMapper;
using Domain.Entities;

namespace Application.Common.DTOs.InfoDtos
{
    public class CustomerDto(
        Guid id,
        string name,
        string primaryPhone,
        string? secondaryPhone,
        string? email,
        string address,
        DateTime createdAt,
        DateTime updatedAt
        )
    {
        public Guid Id { get; set; } = id;
        public string Name { get; set; } = name;
        public string PrimaryPhone { get; set; } = primaryPhone;
        public string? SecondaryPhone { get; set; } = secondaryPhone;
        public string? Email { get; set; } = email;
        public string Address { get; set; } = address;
        public DateTime CreatedAt { get; set; } = createdAt;
        public DateTime UpdatedAt { get; set; } = updatedAt;

        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Customer, CustomerDto>();
            }
        }
    }
}
