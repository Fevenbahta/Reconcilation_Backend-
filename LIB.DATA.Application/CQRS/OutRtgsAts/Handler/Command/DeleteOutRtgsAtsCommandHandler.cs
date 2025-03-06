using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.OutRtgsAts.Request.Command;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.OutRtgsAts.Handler.Command
{
    public class DeleteOutRtgsAtsCommandHandler : IRequestHandler<DeleteOutRtgsAtsCommand>
    {
        public IOutRtgsAtsRepository _OutRtgsAtsRepository;
        public IMapper _mapper;
        public DeleteOutRtgsAtsCommandHandler(IOutRtgsAtsRepository OutRtgsAtsRepository, IMapper mapper)
        {
            _OutRtgsAtsRepository = OutRtgsAtsRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteOutRtgsAtsCommand request, CancellationToken cancellationToken)
        {
            var OutRtgsAts = await _OutRtgsAtsRepository.GetById(request.Id);
            await _OutRtgsAtsRepository.Delete(OutRtgsAts);
            return Unit.Value;
        }

        async Task IRequestHandler<DeleteOutRtgsAtsCommand>.Handle(DeleteOutRtgsAtsCommand request, CancellationToken cancellationToken)
        {
            var OutRtgsAts = await _OutRtgsAtsRepository.GetById(request.Id);
            if (OutRtgsAts == null)
                throw new NotFoundException(nameof(OutRtgsAts), request.Id);
            OutRtgsAts.Status = "1";
            await _OutRtgsAtsRepository.Update(OutRtgsAts);

        }
    }
}
