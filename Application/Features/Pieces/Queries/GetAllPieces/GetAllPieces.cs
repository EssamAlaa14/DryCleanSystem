using Application.Common.DTOs.InfoDtos;
using Application.Common.Interfaces.Infrastructure;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Features.Pieces.Queries.GetAllPieces
{
    public record GetAllPiecesQuery : IRequest<List<PieceDto>>
    {
    }

    public class GetAllPiecesQueryValidator : AbstractValidator<GetAllPiecesQuery>
    {
        public GetAllPiecesQueryValidator() { }
    }

    public class GetAllPiecesQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) : IRequestHandler<GetAllPiecesQuery, List<PieceDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<List<PieceDto>> Handle(GetAllPiecesQuery request, CancellationToken cancellationToken)
        {
            var pieces = await _unitOfWork.Pieces.GetAllAsync(cancellationToken);
            var piecesList = _mapper.Map<List<PieceDto>>(pieces);
            return piecesList;
        }
    }
}
