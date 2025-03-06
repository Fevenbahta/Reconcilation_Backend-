
using LIB.API.Application.DTOs.InRtgsCbc;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIB.API.Application.DTOs.OutRtgsCbc;

namespace LIB.API.Application.CQRS.OutRtgsCbc.Request.Command
{
    public class UpdateOutRtgsCbcCommand : IRequest<Unit>
    {
        public OutRtgsCbcDto OutRtgsCbcDto { get; set; }
    }
}
