using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.Contracts.Persistent;
using LIB.API.Application.CQRS.EqubType.Request.Command;
using LIB.API.Application.DTOs.EqubType.Validators;
using LIB.API.Domain;
using LIBPROPERTY.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.EqubType.Handler.Command
{
    public class CreateEqubTypeCommandHandler : IRequestHandler<CreateEqubTypeCommand, BaseCommandResponse>
    {
        BaseCommandResponse response;
        private IEqubTypeRepository _EqubTypeRepository;
        private IMapper _mapper;
        public CreateEqubTypeCommandHandler(IEqubTypeRepository EqubTypeRepository, IMapper mapper)
        {
            _EqubTypeRepository = EqubTypeRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommandResponse> Handle(CreateEqubTypeCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            // Validate the incoming DTO
            var validator = new EqubTypeDtoValidators();
            var validationResult = await validator.ValidateAsync(request.EqubTypeDto);

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
                var equbType = _mapper.Map<EqubTypes>(request.EqubTypeDto);

                // Generate a unique ID in the format A0000001
                equbType.Id = await GenerateUniqueIdAsync();

                // Add the new EqubType to the repository
                await _EqubTypeRepository.Add(equbType);

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

        private async Task<string> GenerateUniqueIdAsync()
        {
            // Retrieve the last used ID from the repository
            var lastId = await _EqubTypeRepository.GetLastIdAsync();

            // Define the maximum ID value before transitioning to the next prefix
            int maxId = 99999; // 5 digits, maximum 99999
            string prefixSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            // Extract the prefix and number from the last ID
            string currentPrefix = lastId.Substring(0, 1);
            int lastUsedId = int.Parse(lastId.Substring(1));

            // Check if the current ID needs to change prefix
            if (lastUsedId >= maxId)
            {
                // Find the next prefix
                int currentIndex = prefixSet.IndexOf(currentPrefix);
                if (currentIndex < prefixSet.Length - 1)
                {
                    // Increment prefix
                    currentPrefix = prefixSet[currentIndex + 1].ToString();
                    lastUsedId = 0; // Reset ID for the new prefix
                }
                else
                {
                    // No more prefixes available (optional handling)
                    throw new InvalidOperationException("No more prefixes available.");
                }
            }
            else
            {
                // Increment the ID
                lastUsedId++;
            }

            // Format the new ID as A00001
            string newId = $"{currentPrefix}{lastUsedId:D5}";

            // Update the repository with the new ID


            return newId;
        }

    }
}