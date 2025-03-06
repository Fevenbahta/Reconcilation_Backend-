using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.Contracts.Persistent;
using LIB.API.Application.CQRS.EqubType.Request.Command;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.EqubType.Handler.Command
{
    public class DeleteEqubTypeCommandHandler : IRequestHandler<DeleteEqubTypeCommand>
    {
        private IEqubTypeRepository _EqubTypeRepository;
        private IMapper _mapper;
        public DeleteEqubTypeCommandHandler(IEqubTypeRepository EqubTypeRepository, IMapper mapper)
        {
            _EqubTypeRepository = EqubTypeRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteEqubTypeCommand request, CancellationToken cancellationToken)
        {
            var EqubType = await _EqubTypeRepository.GetByIdString(request.Id);
            await _EqubTypeRepository.Delete(EqubType);
            return Unit.Value;
        }

        async Task IRequestHandler<DeleteEqubTypeCommand>.Handle(DeleteEqubTypeCommand request, CancellationToken cancellationToken)
        {
            var EqubType = await _EqubTypeRepository.GetByIdString(request.Id);
            if (EqubType == null)
                throw new NotFoundException(nameof(EqubType), request.Id);
            EqubType.Status = "1";
            await _EqubTypeRepository.Update(EqubType);

        }
    }
}
