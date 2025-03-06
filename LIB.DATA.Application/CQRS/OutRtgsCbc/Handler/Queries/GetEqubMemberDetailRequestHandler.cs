using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.OutRtgsCbc.Request.Queries;
using LIB.API.Application.DTOs.OutRtgsCbc;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.OutRtgsCbc.Handler.Queries
{
    public class GetOutRtgsCbcDetailRequestHandler : IRequestHandler<GetOutRtgsCbcDetailRequest, OutRtgsCbcDto>
    {
        public IOutRtgsCbcRepository _OutRtgsCbcRepository;
        public IMapper _mapper;
        public GetOutRtgsCbcDetailRequestHandler(IOutRtgsCbcRepository OutRtgsCbcRepository, IMapper mapper)
        {
            _OutRtgsCbcRepository = OutRtgsCbcRepository;
            _mapper = mapper;
        }
        public async Task<OutRtgsCbcDto> Handle(GetOutRtgsCbcDetailRequest request, CancellationToken cancellationToken)
        {
            var OutRtgsCbc = await _OutRtgsCbcRepository.GetById(request.Id);
            if (OutRtgsCbc == null )
                throw new NotFoundException(nameof(OutRtgsCbc), request.Id);
            return _mapper.Map<OutRtgsCbcDto>(OutRtgsCbc);
        }
    }
}
