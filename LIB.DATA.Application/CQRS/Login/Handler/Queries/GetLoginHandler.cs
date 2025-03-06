using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.Login.Request.Queries;
using LIB.API.Application.DTOs.User;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.Login.Handler.Queries
{
    public class GetLoginHandler : IRequestHandler<GetLoginRequest, UserDto>
    {
        private IUserRepository _UserRepository;
        private IMapper _mapper;
        public GetLoginHandler(IUserRepository UserRepository, IMapper mapper)
        {
            _UserRepository = UserRepository;
            _mapper = mapper;
        }
        public async Task<UserDto> Handle(GetLoginRequest request, CancellationToken cancellationToken)
        {
            var User = await _UserRepository.GetById(request.UserId);
            if (User == null || User.Status == "1")
                throw new NotFoundException(nameof(User), request.UserId);
            return _mapper.Map<UserDto>(User);
        }
    }
}
