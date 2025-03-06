using LIB.API.Application.DTOs.EqubType.Validators;
using LIB.API.Application.DTOs.Lottery;
using LIBPROPERTY.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.Lottery.Request.Command
{
    public class CreateLotteryCommand : IRequest<BaseCommandResponse>
    {
        public LotteryDto LotteryDto { get; set; }
    }
}
