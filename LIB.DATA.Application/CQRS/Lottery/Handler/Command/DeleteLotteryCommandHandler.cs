using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.Lottery.Request.Command;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.Lottery.Handler.Command
{
    public class DeleteLotteryCommandHandler : IRequestHandler<DeleteLotteryCommand>
    {
        private ILotteryRepository _LotteryRepository;
        private IMapper _mapper;
        public DeleteLotteryCommandHandler(ILotteryRepository LotteryRepository, IMapper mapper)
        {
            _LotteryRepository = LotteryRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteLotteryCommand request, CancellationToken cancellationToken)
        {
            var Lottery = await _LotteryRepository.GetById(request.Id);
            await _LotteryRepository.Delete(Lottery);
            return Unit.Value;
        }

        async Task IRequestHandler<DeleteLotteryCommand>.Handle(DeleteLotteryCommand request, CancellationToken cancellationToken)
        {
            var Lottery = await _LotteryRepository.GetById(request.Id);
            if (Lottery == null)
                throw new NotFoundException(nameof(Lottery), request.Id);
            Lottery.Status = "1";
            await _LotteryRepository.Update(Lottery);

        }
    }
}
