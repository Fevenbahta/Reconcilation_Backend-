using AutoMapper;
using LIB.API.Application.Contracts.Persistence;

using LIB.API.Application.CQRS.InRtgsAts.Request.Command;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.InRtgsAts.Handler.Command
{
    public class DeleteInRtgsAtsCommandHandler : IRequestHandler<DeleteInRtgsAtsCommand>
    {
        public IInRtgsAtsRepository _InRtgsAtsRepository;
        public IMapper _mapper;
        public DeleteInRtgsAtsCommandHandler(IInRtgsAtsRepository InRtgsAtsRepository, IMapper mapper)
        {
            _InRtgsAtsRepository = InRtgsAtsRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteInRtgsAtsCommand request, CancellationToken cancellationToken)
        {
            var InRtgsAts = await _InRtgsAtsRepository.GetByIdString(request.Id);
            await _InRtgsAtsRepository.Delete(InRtgsAts);
            return Unit.Value;
        }

        async Task IRequestHandler<DeleteInRtgsAtsCommand>.Handle(DeleteInRtgsAtsCommand request, CancellationToken cancellationToken)
        {
            var InRtgsAts = await _InRtgsAtsRepository.GetByIdString(request.Id);
            if (InRtgsAts == null)
                throw new NotFoundException(nameof(InRtgsAts), request.Id);
            InRtgsAts.Status = "1";
            await _InRtgsAtsRepository.Update(InRtgsAts);

        }
    }
}
