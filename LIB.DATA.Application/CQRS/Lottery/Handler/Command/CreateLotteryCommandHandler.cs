using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.Lottery.Request.Command;
using LIB.API.Application.DTOs.Lottery.Validators;
using LIB.API.Domain;
using LIBPROPERTY.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.Lottery.Handler.Command
{
    public  class CreateLotteryCommandHandler : IRequestHandler<CreateLotteryCommand, BaseCommandResponse>
    {
        BaseCommandResponse response;
        private ILotteryRepository _LotteryRepository;
        private IMapper _mapper;
        public CreateLotteryCommandHandler(ILotteryRepository LotteryRepository, IMapper mapper)
        {
            _LotteryRepository = LotteryRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommandResponse> Handle(CreateLotteryCommand request, CancellationToken cancellationToken)
        {
            response = new BaseCommandResponse();
            var validator = new LotteryDtoValidators();
            var validationResult = await validator.ValidateAsync(request.LotteryDto);
            try
            {
                if (validationResult.IsValid == false)
                {
                    response.Success = false;
                    response.Message = "Creation Faild";
                    response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                }

                var Lottery = _mapper.Map<Lotteries>(request.LotteryDto);


                var data = await _LotteryRepository.Add(Lottery);
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
