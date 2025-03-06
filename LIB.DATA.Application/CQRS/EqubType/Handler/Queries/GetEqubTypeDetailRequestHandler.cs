using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.Contracts.Persistent;
using LIB.API.Application.CQRS.EqubType.Request.Queries;
using LIB.API.Application.DTOs.EqubType;
using LIB.API.Application.DTOs.EqubType.Validators;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.EqubType.Handler.Queries
{
    public class GetEqubTypeDetailRequestHandler : IRequestHandler<GetEqubTypeDetailRequest, EqubTypeDto>
    {
        private IEqubTypeRepository _EqubTypeRepository;
        private IMapper _mapper;
        public GetEqubTypeDetailRequestHandler(IEqubTypeRepository EqubTypeRepository, IMapper mapper)
        {
            _EqubTypeRepository = EqubTypeRepository;
            _mapper = mapper;
        }
        public async Task<EqubTypeDto> Handle(GetEqubTypeDetailRequest request, CancellationToken cancellationToken)
        {
            var EqubType = await _EqubTypeRepository.GetByIdString(request.Id);
            if (EqubType == null || EqubType

                .Status == "1")
                throw new NotFoundException(nameof(EqubType), request.Id);
            return _mapper.Map<EqubTypeDto>(EqubType);
        }
    }
}
