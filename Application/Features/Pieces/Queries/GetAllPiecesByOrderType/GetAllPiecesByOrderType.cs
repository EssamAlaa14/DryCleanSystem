using Application.Common.DTOs.InfoDtos;
using Application.Common.Extensions;
using Application.Common.Interfaces.Infrastructure;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;
using System.Linq.Expressions;

namespace Application.Features.Pieces.Queries.GetAllPiecesByOrderType
{
    public record GetAllPiecesByOrderTypeQuery : IRequest<List<PieceDto>>
    {
        public OrderType OrderType { get; set; }
    }

    public class GetAllPiecesByOrderTypeQueryValidator : AbstractValidator<GetAllPiecesByOrderTypeQuery>
    {
        public GetAllPiecesByOrderTypeQueryValidator()
        {
            RuleFor(x => x.OrderType)
                .IsInEnum()
                .WithMessage("{PropertyName} with {PropertyValue} is invalid");
        }
    }

    public class GetAllPiecesByOrderTypeQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) : IRequestHandler<GetAllPiecesByOrderTypeQuery, List<PieceDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<List<PieceDto>> Handle(GetAllPiecesByOrderTypeQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Piece, bool>> predicate = PredicateBuilder.True<Piece>();
            if (request.OrderType == OrderType.Clothes)
            {
                predicate = predicate.And(p => p.ClothingType == ClothingType.InternalClothing
                    || p.ClothingType == ClothingType.ExternalClothing);
            }
            else if (request.OrderType == OrderType.Rugs)
            {
                predicate = predicate.And(p => p.ClothingType == ClothingType.Rug
                    || p.ClothingType == ClothingType.Carpet);
            }
            else
            {
                predicate = predicate.And(p => p.ClothingType == ClothingType.Linen);
            }

            var pieces = await _unitOfWork.Pieces.GetByPredicateAsync(predicate, cancellationToken);
            var piecesList = _mapper.Map<List<PieceDto>>(pieces);
            return piecesList;
        }
    }
}
