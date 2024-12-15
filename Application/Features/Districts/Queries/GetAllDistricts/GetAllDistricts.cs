using Application.Common.DTOs.InfoDtos;
using Application.Common.Interfaces.Infrastructure;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Features.Districts.Queries.GetAllDistricts
{
    public class GetAllDistrictsQuery : IRequest<List<DistrictDto>>
    {
    }

    public class GetAllDistrictsQueryValidator : AbstractValidator<GetAllDistrictsQuery>
    {
        public GetAllDistrictsQueryValidator() { }
    }

    public class GetAllDistrictsQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) : IRequestHandler<GetAllDistrictsQuery, List<DistrictDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<List<DistrictDto>> Handle(GetAllDistrictsQuery request, CancellationToken cancellationToken)
        {
            var districts = await _unitOfWork.Districts.GetAllAsync(cancellationToken);
            var districtsList = _mapper.Map<List<DistrictDto>>(districts);
            return districtsList;
        }
    }
}
