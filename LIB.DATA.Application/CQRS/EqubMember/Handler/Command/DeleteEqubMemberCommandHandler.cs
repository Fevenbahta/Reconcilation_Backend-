using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.EqubMember.Request.Command;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.EqubMember.Handler.Command
{
    public class DeleteEqubMemberCommandHandler : IRequestHandler<DeleteEqubMemberCommand>
    {
        private IEqubMemberRepository _EqubMemberRepository;
        private IMapper _mapper;
        public DeleteEqubMemberCommandHandler(IEqubMemberRepository EqubMemberRepository, IMapper mapper)
        {
            _EqubMemberRepository = EqubMemberRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteEqubMemberCommand request, CancellationToken cancellationToken)
        {
            var EqubMember = await _EqubMemberRepository.GetByIdString(request.Id);
            await _EqubMemberRepository.Delete(EqubMember);
            return Unit.Value;
        }

        async Task IRequestHandler<DeleteEqubMemberCommand>.Handle(DeleteEqubMemberCommand request, CancellationToken cancellationToken)
        {
            var EqubMember = await _EqubMemberRepository.GetByIdString(request.Id);
            if (EqubMember == null)
                throw new NotFoundException(nameof(EqubMember), request.Id);
            EqubMember.Status = "1";
            await _EqubMemberRepository.Update(EqubMember);

        }
    }
}
