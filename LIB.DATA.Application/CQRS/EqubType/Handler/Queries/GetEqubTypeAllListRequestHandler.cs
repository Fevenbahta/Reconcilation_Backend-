using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.EqubType.Request.Queries;
using LIB.API.Application.DTOs.EqubType.Validators;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.EqubType.Handler.Queries
{
    public class GetEqubTypeAllListRequestHandler : IRequestHandler<GetEqubTypeAllListRequest, List<EqubTypeDto>>
    {
        private IEqubTypeRepository _EqubTypeRepository;
        private IMapper _mapper;
        public GetEqubTypeAllListRequestHandler(IEqubTypeRepository EqubTypeRepository, IMapper mapper)
        {
            _EqubTypeRepository = EqubTypeRepository;
            _mapper = mapper;
        }
        public async Task<List<EqubTypeDto>> Handle(GetEqubTypeAllListRequest request, CancellationToken cancellationToken)
        {
            var EqubType = await _EqubTypeRepository.GetAll();
      
            return _mapper.Map<List<EqubTypeDto>>(EqubType);
        }
    }
}


