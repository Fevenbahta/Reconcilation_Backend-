using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.EqubMember.Request.Command;
using LIB.API.Application.DTOs.EqubMember.Validators;
using LIB.API.Domain;
using LIBPROPERTY.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.EqubMember.Handler.Command
{
    internal class CreateEqubMemberCommandHandler : IRequestHandler<CreateEqubMemberCommand, BaseCommandResponse>
    {
        BaseCommandResponse response;
        private IEqubMemberRepository _EqubMemberRepository;
        private IMapper _mapper;
        public CreateEqubMemberCommandHandler(IEqubMemberRepository EqubMemberRepository, IMapper mapper)
        {
            _EqubMemberRepository = EqubMemberRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommandResponse> Handle(CreateEqubMemberCommand request, CancellationToken cancellationToken)
        {
            response = new BaseCommandResponse();
            var validator = new EqubMemberDtoValidators();
            var validationResult = await validator.ValidateAsync(request.EqubMemberDto);

            try
            {
                if (!validationResult.IsValid)
                {
                    response.Success = false;
                    response.Message = "Creation Failed";
                    response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    return response;
                }

                var EqubMember = _mapper.Map<EqubMembers>(request.EqubMemberDto);

                // Generate the unique alphanumeric ID
                EqubMember.Id = GenerateUniqueId();

                var data = await _EqubMemberRepository.Add(EqubMember);
                response.Success = true;
                response.Message = "Creation Successful";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Creation Failed due to an unexpected error";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }

        private string GenerateUniqueId()
        {
            string newId;
            do
            {
                // Generate a 6-digit numeric part and pad it with leading zeros
                var numericPart = new Random().Next(1, 999999);
                newId = $"{numericPart:D6}"; // Format: "000001"
            } while (IdExists(newId));

            return newId;
        }

        private bool IdExists(string id)
        {
            // Check if the generated ID already exists in the repository
            return _EqubMemberRepository.GetByIdString(id) != null;
        }
    }
}