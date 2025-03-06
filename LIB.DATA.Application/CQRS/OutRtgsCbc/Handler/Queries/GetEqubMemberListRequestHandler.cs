using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.OutRtgsCbc.Request.Queries;

using LIB.API.Application.DTOs.OutRtgsCbc;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.OutRtgsCbc.Handler.Queries
{
    public class GetOutRtgsCbcListRequestHandler : IRequestHandler<GetOutRtgsCbcListRequest, List<OutRtgsCbcDto>>
    {
        public IOutRtgsCbcRepository _OutRtgsCbcRepository;
        public IMapper _mapper;
        public GetOutRtgsCbcListRequestHandler(IOutRtgsCbcRepository OutRtgsCbcRepository, IMapper mapper)
        {
            _OutRtgsCbcRepository = OutRtgsCbcRepository;
            _mapper = mapper;
        }
        public async Task<List<OutRtgsCbcDto>> Handle(GetOutRtgsCbcListRequest request, CancellationToken cancellationToken)
        {
            var OutRtgsCbc = await _OutRtgsCbcRepository.GetAll();
            var fur = OutRtgsCbc.ToList();
            return _mapper.Map<List<OutRtgsCbcDto>>(fur);
        }
    }
}
