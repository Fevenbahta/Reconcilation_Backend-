using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.InRtgsAts.Request.Queries;
using LIB.API.Application.DTOs.InRtgsAts;
using LIB.API.Application.DTOs.InRtgsAts.Validators;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.InRtgsAts.Handler.Queries
{
    public class GetInRtgsAtsDetailRequestHandler : IRequestHandler<GetInRtgsAtsDetailRequest, InRtgsAtsDto>
    {
        public IInRtgsAtsRepository _InRtgsAtsRepository;
        public IMapper _mapper;
        public GetInRtgsAtsDetailRequestHandler(IInRtgsAtsRepository InRtgsAtsRepository, IMapper mapper)
        {
            _InRtgsAtsRepository = InRtgsAtsRepository;
            _mapper = mapper;
        }
        public async Task<InRtgsAtsDto> Handle(GetInRtgsAtsDetailRequest request, CancellationToken cancellationToken)
        {
            var InRtgsAts = await _InRtgsAtsRepository.GetByIdString(request.Id);
            if (InRtgsAts == null || InRtgsAts

                .Status == "1")
                throw new NotFoundException(nameof(InRtgsAts), request.Id);
            return _mapper.Map<InRtgsAtsDto>(InRtgsAts);
        }
    }
}
