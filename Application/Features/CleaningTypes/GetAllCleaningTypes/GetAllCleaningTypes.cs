using Application.Common.DTOs.InfoDtos;
using Application.Common.Interfaces.Infrastructure;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Features.CleaningTypes.GetAllCleaningTypes
{
    public class GetAllCleaningTypesQuery : IRequest<List<CleaningTypeDto>>
    {
    }

    public class GetAllCleaningTypesQueryValidator : AbstractValidator<GetAllCleaningTypesQuery>
    {
        public GetAllCleaningTypesQueryValidator() { }
    }

    public class GetAllCleaningTypesQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) : IRequestHandler<GetAllCleaningTypesQuery, List<CleaningTypeDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<List<CleaningTypeDto>> Handle(GetAllCleaningTypesQuery request, CancellationToken cancellationToken)
        {
            var cleaningTypes = await _unitOfWork.CleaningTypes.GetAllAsync(cancellationToken);
            var cleaningTypesList = _mapper.Map<List<CleaningTypeDto>>(cleaningTypes);
            return cleaningTypesList;
        }
    }
}
