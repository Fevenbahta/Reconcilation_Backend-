using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.InRtgsCbc.Request.Command
{
    public class DeleteInRtgsCbcCommand : IRequest
    {
        public int Id { get; set; }
    }
}
