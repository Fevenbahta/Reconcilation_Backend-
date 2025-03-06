using AutoMapper;
using LIB.API.Application.Contracts.Persistence;

using LIB.API.Application.CQRS.InRtgsCbc.Request.Queries;
using LIB.API.Application.DTOs.InRtgsCbc;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.InRtgsCbc.Handler.Queries
{
    public class GetEmployeDetaileRequestHandler : IRequestHandler<GetInRtgsCbcDetaileRequest, InRtgsCbcDto>
    {
        public IInRtgsCbcRepository _InRtgsCbcsqlRepository;
        public IMapper _mapper;
        public GetEmployeDetaileRequestHandler(IInRtgsCbcRepository InRtgsCbcsqlRepository, IMapper mapper)
        {
            _InRtgsCbcsqlRepository = InRtgsCbcsqlRepository;
            _mapper = mapper;
        }
        public async Task<InRtgsCbcDto> Handle(GetInRtgsCbcDetaileRequest request, CancellationToken cancellationToken)
        {
            var InRtgsCbc = await _InRtgsCbcsqlRepository.GetById(request.Id);
            if (InRtgsCbc == null )
                throw new NotFoundException(nameof(InRtgsCbc), request.Id);
            return _mapper.Map<InRtgsCbcDto>(InRtgsCbc);
        }
    }
}
