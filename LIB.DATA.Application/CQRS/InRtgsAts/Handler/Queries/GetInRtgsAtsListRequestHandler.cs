using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.InRtgsAts.Request.Queries;
using LIB.API.Application.DTOs.InRtgsAts;
using LIB.API.Application.DTOs.InRtgsAts.Validators;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.InRtgsAts.Handler.Queries
{
    public class GetInRtgsAtsListRequestHandler : IRequestHandler<GetInRtgsAtsListRequest, List<InRtgsAtsDto>>
    {
        public IInRtgsAtsRepository _InRtgsAtsRepository;
        public IMapper _mapper;
        public GetInRtgsAtsListRequestHandler(IInRtgsAtsRepository InRtgsAtsRepository, IMapper mapper)
        {
            _InRtgsAtsRepository = InRtgsAtsRepository;
            _mapper = mapper;
        }
        public async Task<List<InRtgsAtsDto>> Handle(GetInRtgsAtsListRequest request, CancellationToken cancellationToken)
        {
            var InRtgsAts = await _InRtgsAtsRepository.GetAll();
            var fur = InRtgsAts.Where(s => s.Status != "1").ToList();
            return _mapper.Map<List<InRtgsAtsDto>>(fur);
        }
    }
}
