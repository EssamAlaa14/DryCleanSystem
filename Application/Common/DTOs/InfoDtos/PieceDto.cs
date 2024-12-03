using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Common.DTOs.InfoDtos
{
    public class PieceDto(
        Guid id,
        string name,
        ClothingType clothingType
        )
    {
        public Guid Id { get; set; } = id;
        public string Name { get; set; } = name;
        public ClothingType ClothingType { get; set; } = clothingType;

        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Piece, PieceDto>();
            }
        }
    }
}