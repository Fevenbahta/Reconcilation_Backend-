using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.Contracts.Persistent;
using LIB.API.Application.CQRS.EqubType.Request.Queries;
using LIB.API.Application.DTOs.EqubType;
using LIB.API.Application.DTOs.EqubType.Validators;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.EqubType.Handler.Queries
{
    public class GetEqubTypeListRequestHandler : IRequestHandler<GetEqubTypeListRequest, List<EqubTypeDto>>
    {
        private IEqubTypeRepository _EqubTypeRepository;
        private IMapper _mapper;
        public GetEqubTypeListRequestHandler(IEqubTypeRepository EqubTypeRepository, IMapper mapper)
        {
            _EqubTypeRepository = EqubTypeRepository;
            _mapper = mapper;
        }
        public async Task<List<EqubTypeDto>> Handle(GetEqubTypeListRequest request, CancellationToken cancellationToken)
        {
            var EqubType = await _EqubTypeRepository.GetAll();
            var fur = EqubType.Where(s => s.Status != "1").ToList();
            return _mapper.Map<List<EqubTypeDto>>(fur);
        }
    }
}
