using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.EqubMember.Request.Queries;
using LIB.API.Application.DTOs.EqubMember;
using LIB.API.Application.DTOs.EqubMember.Validators;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.EqubMember.Handler.Queries
{
    internal class GetEqubMemberDetailRequestHandler : IRequestHandler<GetEqubMemberDetailRequest, EqubMemberDto>
    {
        private IEqubMemberRepository _EqubMemberRepository;
        private IMapper _mapper;
        public GetEqubMemberDetailRequestHandler(IEqubMemberRepository EqubMemberRepository, IMapper mapper)
        {
            _EqubMemberRepository = EqubMemberRepository;
            _mapper = mapper;
        }
        public async Task<EqubMemberDto> Handle(GetEqubMemberDetailRequest request, CancellationToken cancellationToken)
        {
            var EqubMember = await _EqubMemberRepository.GetById(request.Id);
            if (EqubMember == null || EqubMember

                .Status == "1")
                throw new NotFoundException(nameof(EqubMember), request.Id);
            return _mapper.Map<EqubMemberDto>(EqubMember);
        }
    }
}
