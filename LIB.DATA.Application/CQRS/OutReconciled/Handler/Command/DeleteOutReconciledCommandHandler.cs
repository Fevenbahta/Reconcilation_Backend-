using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.OutReconciled.Request.Command;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.OutReconciled.Handler.Command
{
    public class DeleteOutReconciledCommandHandler : IRequestHandler<DeleteOutReconciledCommand>
    {
        public IOutReconciledRepository _OutReconciledRepository;
        public IMapper _mapper;
        public DeleteOutReconciledCommandHandler(IOutReconciledRepository OutReconciledRepository, IMapper mapper)
        {
            _OutReconciledRepository = OutReconciledRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteOutReconciledCommand request, CancellationToken cancellationToken)
        {
            var OutReconciled = await _OutReconciledRepository.GetByIdString(request.Id);
            await _OutReconciledRepository.Delete(OutReconciled);
            return Unit.Value;
        }

        async Task IRequestHandler<DeleteOutReconciledCommand>.Handle(DeleteOutReconciledCommand request, CancellationToken cancellationToken)
        {
            var OutReconciled = await _OutReconciledRepository.GetByIdString(request.Id);
            if (OutReconciled == null)
                throw new NotFoundException(nameof(OutReconciled), request.Id);
          
            await _OutReconciledRepository.Update(OutReconciled);

        }
    }
}
