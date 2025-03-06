using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.EqubMember.Request.Queries;
using LIB.API.Application.DTOs.EqubMember;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.EqubMember.Handler.Queries
{
    internal class GetEqubMemberListRequestHandler : IRequestHandler<GetEqubMemberListRequest, List<EqubMemberDto>>
    {
        private IEqubMemberRepository _EqubMemberRepository;
        private IMapper _mapper;
        public GetEqubMemberListRequestHandler(IEqubMemberRepository EqubMemberRepository, IMapper mapper)
        {
            _EqubMemberRepository = EqubMemberRepository;
            _mapper = mapper;
        }
        public async Task<List<EqubMemberDto>> Handle(GetEqubMemberListRequest request, CancellationToken cancellationToken)
        {
            var EqubMember = await _EqubMemberRepository.GetAll();
            var fur = EqubMember.Where(s => s.Status != "1").ToList();
            return _mapper.Map<List<EqubMemberDto>>(fur);
        }
    }
}
