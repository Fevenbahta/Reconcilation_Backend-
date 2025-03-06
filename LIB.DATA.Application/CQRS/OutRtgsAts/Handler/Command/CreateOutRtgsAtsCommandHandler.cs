using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.OutRtgsAts.Request.Command;
using LIB.API.Application.DTOs.InRtgsAts.Validators;
using LIB.API.Application.DTOs.OutRtgsAts.Validators;
using LIB.API.Domain;
using LIBPROPERTY.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.OutRtgsAts.Handler.Command
{
    public  class CreateOutRtgsAtsCommandHandler : IRequestHandler<CreateOutRtgsAtsCommand, BaseCommandResponse>
    {
        BaseCommandResponse response;
        public IOutRtgsAtsRepository _OutRtgsAtsRepository;
        public IMapper _mapper;
        public CreateOutRtgsAtsCommandHandler(IOutRtgsAtsRepository OutRtgsAtsRepository, IMapper mapper)
        {
            _OutRtgsAtsRepository = OutRtgsAtsRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommandResponse> Handle(CreateOutRtgsAtsCommand request, CancellationToken cancellationToken)
        {
            response = new BaseCommandResponse();
            var validator = new OutRtgsAtsDtoValidators();
            var validationResult = await validator.ValidateAsync(request.OutRtgsAtsDto);
            try
            {
                if (validationResult.IsValid == false)
                {
                    response.Success = false;
                    response.Message = "Creation Faild";
                    response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                }

                var OutRtgsAts = _mapper.Map<OutRtgsAtss>(request.OutRtgsAtsDto);


                var data = await _OutRtgsAtsRepository.Add(OutRtgsAts);
                response.Success = true;
                response.Message = "Creation Successfull";


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
