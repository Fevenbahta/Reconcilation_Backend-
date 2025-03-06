using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.EqubType.Request.Command;

using LIB.API.Application.DTOs.EqubType.Validators;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.EqubType.Handler.Command
{
    public class UpdateEqubTypeCommandHandler : IRequestHandler<UpdateEqubTypeCommand, Unit>
    {
        private IEqubTypeRepository _EqubTypeRepository;
        private IMapper _mapper;

        public UpdateEqubTypeCommandHandler(IEqubTypeRepository EqubTypeRepository, IMapper mapper)
        {
            _EqubTypeRepository = EqubTypeRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateEqubTypeCommand request, CancellationToken cancellationToken)
        {
            var validator = new EqubTypeDtoValidators();
            var validationResult = await validator.ValidateAsync(request.EqubTypeDto);
            if (validationResult.IsValid == false)
                throw new ValidationException(validationResult);

            var use = await _EqubTypeRepository.GetByIdString(request.EqubTypeDto.Id);



            var add = _mapper.Map(request.EqubTypeDto, use);

            await _EqubTypeRepository.Update(add);
            return Unit.Value;
        }
    }
}
