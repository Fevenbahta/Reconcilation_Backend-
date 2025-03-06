
using LIB.API.Application.DTOs.InRtgsCbc;
using LIBPROPERTY.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.InRtgsCbc.Request.Command
{
    public class CreateInRtgsCbcCommand : IRequest<BaseCommandResponse>
    {
        public InRtgsCbcDto InRtgsCbcDto { get; set; }
    }
}
