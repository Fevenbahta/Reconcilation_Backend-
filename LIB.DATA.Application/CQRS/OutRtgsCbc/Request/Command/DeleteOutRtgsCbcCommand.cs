using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.OutRtgsCbc.Request.Command
{
    public class DeleteOutRtgsCbcCommand : IRequest
    {
        public string Id { get; set; }
    }
}
