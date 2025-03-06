using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.Lottery.Request.Command
{
    public class DeleteLotteryCommand : IRequest
    {
        public int Id { get; set; }
    }
}
