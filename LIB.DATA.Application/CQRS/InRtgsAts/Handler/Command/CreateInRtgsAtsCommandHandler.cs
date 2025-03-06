using AutoMapper;
using LIB.API.Application.Contracts.Persistence;

using LIB.API.Application.CQRS.InRtgsAts.Request.Command;
using LIB.API.Application.DTOs.InRtgsAts.Validators;
using LIB.API.Domain;
using LIBPROPERTY.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.InRtgsAts.Handler.Command
{
    public class CreateInRtgsAtsCommandHandler : IRequestHandler<CreateInRtgsAtsCommand, BaseCommandResponse>
    {
        BaseCommandResponse response;
        public IInRtgsAtsRepository _InRtgsAtsRepository;
        public IMapper _mapper;
        public CreateInRtgsAtsCommandHandler(IInRtgsAtsRepository InRtgsAtsRepository, IMapper mapper)
        {
            _InRtgsAtsRepository = InRtgsAtsRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommandResponse> Handle(CreateInRtgsAtsCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            // Validate the incoming DTO
            var validator = new InRtgsAtsDtoValidators();
            var validationResult = await validator.ValidateAsync(request.InRtgsAtsDto);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Creation Failed";
                response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return response;
            }

            try
            {
                // Map the DTO to the domain model
                var InRtgsAts = _mapper.Map<InRtgsAtss>(request.InRtgsAtsDto);

                // Generate a unique ID in the format A0000001
          

                // Add the new InRtgsAts to the repository
                await _InRtgsAtsRepository.Add(InRtgsAts);

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

  
    }
}