using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.InReconciled.Request.Command;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.InReconciled.Handler.Command
{
    public class DeleteInReconciledCommandHandler : IRequestHandler<DeleteInReconciledCommand>
    {
        public IInReconciledRepository _InReconciledRepository;
        public IMapper _mapper;
        public DeleteInReconciledCommandHandler(IInReconciledRepository InReconciledRepository, IMapper mapper)
        {
            _InReconciledRepository = InReconciledRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteInReconciledCommand request, CancellationToken cancellationToken)
        {
            var InReconciled = await _InReconciledRepository.GetByIdString(request.Id);
            await _InReconciledRepository.Delete(InReconciled);
            return Unit.Value;
        }

        async Task IRequestHandler<DeleteInReconciledCommand>.Handle(DeleteInReconciledCommand request, CancellationToken cancellationToken)
        {
            var InReconciled = await _InReconciledRepository.GetByIdString(request.Id);
            if (InReconciled == null)
                throw new NotFoundException(nameof(InReconciled), request.Id);
          
            await _InReconciledRepository.Update(InReconciled);

        }
    }
}
