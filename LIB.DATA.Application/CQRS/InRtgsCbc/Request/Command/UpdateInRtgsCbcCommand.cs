using LIB.API.Application.DTOs.InRtgsCbc;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.InRtgsCbc.Request.Command
{
    public class UpdateInRtgsCbcCommand : IRequest<Unit>
    {
        public InRtgsCbcDto InRtgsCbcDto { get; set; }
    }

}

