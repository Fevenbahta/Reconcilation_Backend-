using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.Lottery.Request.Queries;
using LIB.API.Application.DTOs.Lottery;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.Lottery.Handler.Queries
{
    public class GetLotteryDetailRequestHandler : IRequestHandler<GetLotteryDetailRequest, LotteryDto>
    {
        private ILotteryRepository _LotteryRepository;
        private IMapper _mapper;
        public GetLotteryDetailRequestHandler(ILotteryRepository LotteryRepository, IMapper mapper)
        {
            _LotteryRepository = LotteryRepository;
            _mapper = mapper;
        }
        public async Task<LotteryDto> Handle(GetLotteryDetailRequest request, CancellationToken cancellationToken)
        {
            var Lottery = await _LotteryRepository.GetById(request.Id);
            if (Lottery == null || Lottery

                .Status == "1")
                throw new NotFoundException(nameof(Lottery), request.Id);
            return _mapper.Map<LotteryDto>(Lottery);
        }
    }
}

