using AutoMapper;
using Domain.Entities.Orders;

namespace Application.Common.DTOs.InfoDtos
{
    public class OrderItemDto
    {
        public string PieceName { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public decimal SingleQuantityPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Comment { get; set; }
        public string CleaningTypes { get; set; }

        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<OrderItem, OrderItemDto>()
                    .ForMember(dest => dest.CleaningTypes, opt => opt.MapFrom(src => string.Join(", ", src.CleaningTypes.Select(x => x.Name))))
                    .ForMember(dest => dest.PieceName, opt => opt.MapFrom(src => src.Piece != null ? src.Piece.Name : ""));
            }
        }
    }
}
