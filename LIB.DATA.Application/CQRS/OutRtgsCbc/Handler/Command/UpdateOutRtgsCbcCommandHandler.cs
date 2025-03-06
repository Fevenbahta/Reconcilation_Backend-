using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.OutRtgsCbc.Request.Command;
using LIB.API.Application.DTOs.OutRtgsCbc.Validators;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.OutRtgsCbc.Handler.Command
{
    public class UpdateOutRtgsCbcCommandHandler : IRequestHandler<UpdateOutRtgsCbcCommand, Unit>
    {
        public IOutRtgsCbcRepository _OutRtgsCbcRepository;
        public IMapper _mapper;

        public UpdateOutRtgsCbcCommandHandler(IOutRtgsCbcRepository OutRtgsCbcRepository, IMapper mapper)
        {
            _OutRtgsCbcRepository = OutRtgsCbcRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateOutRtgsCbcCommand request, CancellationToken cancellationToken)
        {
            var validator = new OutRtgsCbcDtoValidators();
            var validationResult = await validator.ValidateAsync(request.OutRtgsCbcDto);
            if (validationResult.IsValid == false)
                throw new ValidationException(validationResult);

            var use = await _OutRtgsCbcRepository.GetByIdString(request.OutRtgsCbcDto.REFNO);



            var add = _mapper.Map(request.OutRtgsCbcDto, use);

            await _OutRtgsCbcRepository.Update(add);
            return Unit.Value;
        }
    }
}
