using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.Login.Request.Command;
using LIB.API.Application.DTOs.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.Login.Handler.Command
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private IMapper _mapper;
        public LoginCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            

            // Perform authentication logic
            var isAuthenticated = await _userRepository.CheckCredentials(request.UserName, request.Password);

            return _mapper.Map<UserDto>(isAuthenticated);

        }
    }
}
