using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.EqubMember.Request.Command;
using LIB.API.Application.DTOs.EqubMember.Validators;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.EqubMember.Handler.Command
{
    public class UpdateEqubMemberCommandHandler : IRequestHandler<UpdateEqubMemberCommand, Unit>
    {
        private IEqubMemberRepository _EqubMemberRepository;
        private IMapper _mapper;

        public UpdateEqubMemberCommandHandler(IEqubMemberRepository EqubMemberRepository, IMapper mapper)
        {
            _EqubMemberRepository = EqubMemberRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateEqubMemberCommand request, CancellationToken cancellationToken)
        {
            var validator = new EqubMemberDtoValidators();
            var validationResult = await validator.ValidateAsync(request.EqubMemberDto);
            if (validationResult.IsValid == false)
                throw new ValidationException(validationResult);

            var use = await _EqubMemberRepository.GetByIdString(request.EqubMemberDto.Id);



            var add = _mapper.Map(request.EqubMemberDto, use);

            await _EqubMemberRepository.Update(add);
            return Unit.Value;
        }
    }
}
