using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.Login.Request.Command;
using LIB.API.Application.DTOs.User.Validator;
using LIB.API.Domain;
using LIBPROPERTY.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.Login.Handler.Command
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, BaseCommandResponse>

    {
        BaseCommandResponse response;
        private IMapper _mapper;
        private readonly IUserRepository _userRepository;
        //private readonly IPasswordHasher _passwordHasher;

        public RegisterCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            // _passwordHasher = passwordHasher;
        }


        public async Task<BaseCommandResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            response = new BaseCommandResponse();
            var validator = new UserDtoValidator();
            var validationResult = await validator.ValidateAsync(request.userDto);

            if (validationResult.IsValid == false)
            {
                response.Success = false;
                response.Message = "Creation Faild";
                response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            }
           

            // Increment the last ID by 1 to get the next ID


            var User = _mapper.Map<Users>(request.userDto);

            User.Id = 0;
            var data = await _userRepository.Add(User);
            response.Success = true;
            response.Message = "Creation Successfull";
            return response;
        }
    }
}
