using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.InRtgsAts.Request.Queries;
using LIB.API.Application.DTOs.InRtgsAts.Validators;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.InRtgsAts.Handler.Queries
{
    public class GetInRtgsAtsAllListRequestHandler : IRequestHandler<GetInRtgsAtsAllListRequest, List<InRtgsAtsDto>>
    {
        public IInRtgsAtsRepository _InRtgsAtsRepository;
        public IMapper _mapper;
        public GetInRtgsAtsAllListRequestHandler(IInRtgsAtsRepository InRtgsAtsRepository, IMapper mapper)
        {
            _InRtgsAtsRepository = InRtgsAtsRepository;
            _mapper = mapper;
        }
        public async Task<List<InRtgsAtsDto>> Handle(GetInRtgsAtsAllListRequest request, CancellationToken cancellationToken)
        {
            var InRtgsAts = await _InRtgsAtsRepository.GetAll();
      
            return _mapper.Map<List<InRtgsAtsDto>>(InRtgsAts);
        }
    }
}


