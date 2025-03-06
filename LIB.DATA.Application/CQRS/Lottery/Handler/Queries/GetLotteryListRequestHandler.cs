using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.Lottery.Request.Queries;
using LIB.API.Application.DTOs.Lottery;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.Lottery.Handler.Queries
{
    public class GetLotteryListRequestHandler   : IRequestHandler<GetLotteryListRequest, List<LotteryDto>>
    {
        private ILotteryRepository _LotteryRepository;
        private IMapper _mapper;
        public GetLotteryListRequestHandler(ILotteryRepository LotteryRepository, IMapper mapper)
        {
            _LotteryRepository = LotteryRepository;
            _mapper = mapper;
        }
        public async Task<List<LotteryDto>> Handle(GetLotteryListRequest request, CancellationToken cancellationToken)
        {
            var Lottery = await _LotteryRepository.GetAll();
            var fur = Lottery.Where(s => s.Status != "1").ToList();
            return _mapper.Map<List<LotteryDto>>(fur);
        }
    }
}


