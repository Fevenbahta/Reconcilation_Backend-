using AutoMapper;
using LIB.API.Application.Contracts.Persistence;

using LIB.API.Application.CQRS.Login.Request.Command;

using LIB.API.Application.DTOs.User.Validator;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.Login.Handler.Command
{
    public class UpdateCommandHandler : IRequestHandler<UpdateCommand, Unit>
    {
        private IUserRepository _UserRepository;
        private IMapper _mapper;

        public UpdateCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _UserRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            var validator = new UserDtoValidator();
            var validationResult = await validator.ValidateAsync(request.userDto);
            if (validationResult.IsValid == false)
                throw new ValidationException(validationResult);

            var use = await _UserRepository.GetById(request.userDto.Id);



            var add = _mapper.Map(request.userDto, use);

            await _UserRepository.Update(add);
            return Unit.Value;
        }
    }
}
