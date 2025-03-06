using AutoMapper;
using LIB.API.Application.Contracts.Persistence;

using LIB.API.Application.CQRS.InRtgsCbc.Request.Queries;
using LIB.API.Application.DTOs.InRtgsCbc;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.InRtgsCbc.Handler.Queries
{
    public class GetInRtgsCbcListRequestHandler : IRequestHandler<GetInRtgsCbcListRequest, List<InRtgsCbcDto>>
    {
        public IInRtgsCbcRepository _InRtgsCbcsqlRepository;
        public IMapper _mapper;
        public GetInRtgsCbcListRequestHandler(IInRtgsCbcRepository InRtgsCbcsqlRepository, IMapper mapper)
        {
            _InRtgsCbcsqlRepository = InRtgsCbcsqlRepository;
            _mapper = mapper;
        }
        public async Task<List<InRtgsCbcDto>> Handle(GetInRtgsCbcListRequest request, CancellationToken cancellationToken)
        {
            var InRtgsCbc = await _InRtgsCbcsqlRepository.GetAll();
            var fur = InRtgsCbc.ToList();
            return _mapper.Map<List<InRtgsCbcDto>>(fur);
        }
    }
}
