using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.InRtgsCbc.Request.Command;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.InRtgsCbc.Handler.Command
{
    public class DeleteInRtgsCbcCommandHandler : IRequestHandler<DeleteInRtgsCbcCommand>
    {
        public IInRtgsCbcRepository _InRtgsCbcRepository;
        public IMapper _mapper;
        public DeleteInRtgsCbcCommandHandler(IInRtgsCbcRepository InRtgsCbcRepository, IMapper mapper)
        {
            _InRtgsCbcRepository = InRtgsCbcRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteInRtgsCbcCommand request, CancellationToken cancellationToken)
        {
            var InRtgsCbc = await _InRtgsCbcRepository.GetById(request.Id);
            await _InRtgsCbcRepository.Delete(InRtgsCbc);
            return Unit.Value;
        }

        async Task IRequestHandler<DeleteInRtgsCbcCommand>.Handle(DeleteInRtgsCbcCommand request, CancellationToken cancellationToken)
        {
            var InRtgsCbc = await _InRtgsCbcRepository.GetById(request.Id);
            if (InRtgsCbc == null)
                throw new NotFoundException(nameof(InRtgsCbc), request.Id);
            
            await _InRtgsCbcRepository.Delete(InRtgsCbc);

        }
    }
}
