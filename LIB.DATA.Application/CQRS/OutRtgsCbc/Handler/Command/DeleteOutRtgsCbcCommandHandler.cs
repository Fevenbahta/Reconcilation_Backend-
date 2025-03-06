using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.OutRtgsCbc.Request.Command;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.OutRtgsCbc.Handler.Command
{
    public class DeleteOutRtgsCbcCommandHandler : IRequestHandler<DeleteOutRtgsCbcCommand>
    {
        public IOutRtgsCbcRepository _OutRtgsCbcRepository;
        public IMapper _mapper;
        public DeleteOutRtgsCbcCommandHandler(IOutRtgsCbcRepository OutRtgsCbcRepository, IMapper mapper)
        {
            _OutRtgsCbcRepository = OutRtgsCbcRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteOutRtgsCbcCommand request, CancellationToken cancellationToken)
        {
            var OutRtgsCbc = await _OutRtgsCbcRepository.GetByIdString(request.Id);
            await _OutRtgsCbcRepository.Delete(OutRtgsCbc);
            return Unit.Value;
        }

        async Task IRequestHandler<DeleteOutRtgsCbcCommand>.Handle(DeleteOutRtgsCbcCommand request, CancellationToken cancellationToken)
        {
            var OutRtgsCbc = await _OutRtgsCbcRepository.GetByIdString(request.Id);
            if (OutRtgsCbc == null)
                throw new NotFoundException(nameof(OutRtgsCbc), request.Id);
          
            await _OutRtgsCbcRepository.Update(OutRtgsCbc);

        }
    }
}
