using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.OutRtgsAts.Request.Command;
using LIB.API.Application.DTOs.InRtgsAts.Validators;
using LIB.API.Application.DTOs.OutRtgsAts.Validators;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.OutRtgsAts.Handler.Command
{
    public class UpdateOutRtgsAtsCommandHandler : IRequestHandler<UpdateOutRtgsAtsCommand, Unit>
    {
        public IOutRtgsAtsRepository _OutRtgsAtsRepository;
        public IMapper _mapper;

        public UpdateOutRtgsAtsCommandHandler(IOutRtgsAtsRepository OutRtgsAtsRepository, IMapper mapper)
        {
            _OutRtgsAtsRepository = OutRtgsAtsRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateOutRtgsAtsCommand request, CancellationToken cancellationToken)
        {
            var validator = new OutRtgsAtsDtoValidators();
            var validationResult = await validator.ValidateAsync(request.OutRtgsAtsDto);
            if (validationResult.IsValid == false)
                throw new ValidationException(validationResult);

            var use = await _OutRtgsAtsRepository.GetByIdString(request.OutRtgsAtsDto.Reference);



            var add = _mapper.Map(request.OutRtgsAtsDto, use);

            await _OutRtgsAtsRepository.Update(add);
            return Unit.Value;
        }
    }
}


