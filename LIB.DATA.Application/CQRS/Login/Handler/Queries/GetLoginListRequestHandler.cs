using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.Contracts.Persistent;
using LIB.API.Application.CQRS.Login.Request.Queries;
using LIB.API.Application.DTOs.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.Login.Handler.Queries
{
    public class GetLoginListRequestHandler : IRequestHandler<GetLoginList, List<UserDto>>
    {
        private IUserRepository _userRepository;
        private IMapper _mapper;
        public GetLoginListRequestHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<List<UserDto>> Handle(GetLoginList request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAll();
          var  newUser=user.Where(s => s.Status != "1").ToList();
            return _mapper.Map<List<UserDto>>(newUser);
        }
    }
}
