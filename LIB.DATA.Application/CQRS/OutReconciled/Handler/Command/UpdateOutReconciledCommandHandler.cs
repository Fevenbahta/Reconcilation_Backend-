using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.OutReconciled.Request.Command;

using LIB.API.Application.DTOs.OutRtgsCbc.Validators;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.OutReconciled.Handler.Command
{
    public class UpdateOutReconciledCommandHandler : IRequestHandler<UpdateOutReconciledCommand, Unit>
    {
        public IOutReconciledRepository _OutReconciledRepository;
        public IMapper _mapper;

        public UpdateOutReconciledCommandHandler(IOutReconciledRepository OutReconciledRepository, IMapper mapper)
        {
            _OutReconciledRepository = OutReconciledRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateOutReconciledCommand request, CancellationToken cancellationToken)
        {
            var validator = new OutReconciledDtoValidators();
            var validationResult = await validator.ValidateAsync(request.OutReconciledDto);
            if (validationResult.IsValid == false)
                throw new ValidationException(validationResult);

            var use = await _OutReconciledRepository.GetById(request.OutReconciledDto.No);



            var add = _mapper.Map(request.OutReconciledDto, use);

            await _OutReconciledRepository.Update(add);
            return Unit.Value;
        }
    }
}
