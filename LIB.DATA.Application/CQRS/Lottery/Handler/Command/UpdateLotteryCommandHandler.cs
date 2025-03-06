using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.Lottery.Request.Command;
using LIB.API.Application.DTOs.Lottery.Validators;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.Lottery.Handler.Command
{
    public class UpdateLotteryCommandHandler : IRequestHandler<UpdateLotteryCommand, Unit>
    {
        private ILotteryRepository _LotteryRepository;
        private IMapper _mapper;

        public UpdateLotteryCommandHandler(ILotteryRepository LotteryRepository, IMapper mapper)
        {
            _LotteryRepository = LotteryRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateLotteryCommand request, CancellationToken cancellationToken)
        {
            var validator = new LotteryDtoValidators();
            var validationResult = await validator.ValidateAsync(request.LotteryDto);
            if (validationResult.IsValid == false)
                throw new ValidationException(validationResult);

            var use = await _LotteryRepository.GetById(request.LotteryDto.Id);



            var add = _mapper.Map(request.LotteryDto, use);

            await _LotteryRepository.Update(add);
            return Unit.Value;
        }
    }
}


