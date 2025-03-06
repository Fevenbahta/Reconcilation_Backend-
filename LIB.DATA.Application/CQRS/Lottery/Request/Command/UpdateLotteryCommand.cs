using LIB.API.Application.DTOs.EqubType.Validators;
using LIB.API.Application.DTOs.Lottery;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.Lottery.Request.Command
{
    public class UpdateLotteryCommand : IRequest<Unit>
    {
        public LotteryDto LotteryDto { get; set; }
    }
}
