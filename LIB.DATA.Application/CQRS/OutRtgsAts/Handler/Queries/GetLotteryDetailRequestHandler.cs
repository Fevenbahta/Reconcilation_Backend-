using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.OutRtgsAts.Request.Queries;
using LIB.API.Application.DTOs.InRtgsAts.Validators;
using LIB.API.Application.DTOs.OutRtgsAts.Validators;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.OutRtgsAts.Handler.Queries
{
    public class GetOutRtgsAtsDetailRequestHandler : IRequestHandler<GetOutRtgsAtsDetailRequest, OutRtgsAtsDto>
    {
        public IOutRtgsAtsRepository _OutRtgsAtsRepository;
        public IMapper _mapper;
        public GetOutRtgsAtsDetailRequestHandler(IOutRtgsAtsRepository OutRtgsAtsRepository, IMapper mapper)
        {
            _OutRtgsAtsRepository = OutRtgsAtsRepository;
            _mapper = mapper;
        }
        public async Task<OutRtgsAtsDto> Handle(GetOutRtgsAtsDetailRequest request, CancellationToken cancellationToken)
        {
            var OutRtgsAts = await _OutRtgsAtsRepository.GetById(request.Id);
            if (OutRtgsAts == null || OutRtgsAts

                .Status == "1")
                throw new NotFoundException(nameof(OutRtgsAts), request.Id);
            return _mapper.Map<OutRtgsAtsDto>(OutRtgsAts);
        }
    }
}

