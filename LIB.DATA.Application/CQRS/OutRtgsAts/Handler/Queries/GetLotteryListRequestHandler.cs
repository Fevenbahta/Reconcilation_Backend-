using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.OutRtgsAts.Request.Queries;
using LIB.API.Application.DTOs.InRtgsAts.Validators;
using LIB.API.Application.DTOs.OutRtgsAts.Validators;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.OutRtgsAts.Handler.Queries
{
    public class GetOutRtgsAtsListRequestHandler   : IRequestHandler<GetOutRtgsAtsListRequest, List<OutRtgsAtsDto>>
    {
        public IOutRtgsAtsRepository _OutRtgsAtsRepository;
        public IMapper _mapper;
        public GetOutRtgsAtsListRequestHandler(IOutRtgsAtsRepository OutRtgsAtsRepository, IMapper mapper)
        {
            _OutRtgsAtsRepository = OutRtgsAtsRepository;
            _mapper = mapper;
        }
        public async Task<List<OutRtgsAtsDto>> Handle(GetOutRtgsAtsListRequest request, CancellationToken cancellationToken)
        {
            var OutRtgsAts = await _OutRtgsAtsRepository.GetAll();
            var fur = OutRtgsAts.Where(s => s.Status != "1").ToList();
            return _mapper.Map<List<OutRtgsAtsDto>>(fur);
        }
    }
}


