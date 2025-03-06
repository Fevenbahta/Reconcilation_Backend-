using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.InReconciled.Request.Command;

using LIB.API.Application.DTOs.InRtgsCbc.Validators;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.InReconciled.Handler.Command
{
    public class UpdateInReconciledCommandHandler : IRequestHandler<UpdateInReconciledCommand, Unit>
    {
        public IInReconciledRepository _InReconciledRepository;
        public IMapper _mapper;

        public UpdateInReconciledCommandHandler(IInReconciledRepository InReconciledRepository, IMapper mapper)
        {
            _InReconciledRepository = InReconciledRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateInReconciledCommand request, CancellationToken cancellationToken)
        {
            var validator = new InReconciledDtoValidators();
            var validationResult = await validator.ValidateAsync(request.InReconciledDto);
            if (validationResult.IsValid == false)
                throw new ValidationException(validationResult);

            var use = await _InReconciledRepository.GetById(request.InReconciledDto.No);



            var add = _mapper.Map(request.InReconciledDto, use);

            await _InReconciledRepository.Update(add);
            return Unit.Value;
        }
    }
}
