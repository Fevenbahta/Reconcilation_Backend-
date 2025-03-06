using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.InRtgsAts.Request.Command;

using LIB.API.Application.DTOs.InRtgsAts.Validators;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.InRtgsAts.Handler.Command
{
    public class UpdateInRtgsAtsCommandHandler : IRequestHandler<UpdateInRtgsAtsCommand, Unit>
    {
        public IInRtgsAtsRepository _InRtgsAtsRepository;
        public IMapper _mapper;

        public UpdateInRtgsAtsCommandHandler(IInRtgsAtsRepository InRtgsAtsRepository, IMapper mapper)
        {
            _InRtgsAtsRepository = InRtgsAtsRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateInRtgsAtsCommand request, CancellationToken cancellationToken)
        {
            var validator = new InRtgsAtsDtoValidators();
            var validationResult = await validator.ValidateAsync(request.InRtgsAtsDto);
            if (validationResult.IsValid == false)
                throw new ValidationException(validationResult);

            var use = await _InRtgsAtsRepository.GetByIdString(request.InRtgsAtsDto.Reference);



            var add = _mapper.Map(request.InRtgsAtsDto, use);

            await _InRtgsAtsRepository.Update(add);
            return Unit.Value;
        }
    }
}
