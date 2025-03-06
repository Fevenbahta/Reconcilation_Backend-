using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.Login.Request.Command;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.Login.Handler.Command
{
    public class DeleteCommandHandler: IRequestHandler<DeleteCommand>
    {
        private IUserRepository _userRepository;
        private IMapper _mapper;
        public DeleteCommandHandler(IUserRepository UserRepository, IMapper mapper)
        {
            _userRepository = UserRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            var use = await _userRepository.GetById(request.Id);
            await _userRepository.Update(use);
            return Unit.Value;
        }

        async Task IRequestHandler<DeleteCommand>.Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(request.Id);
            if (user == null)
                throw new NotFoundException(nameof(user
                     ), request.Id);
            user.Status = "1";

            await _userRepository.Update(user);

        }
    }
}
