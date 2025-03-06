
using LIB.API.Application.DTOs.InRtgsAts.Validators;
using LIB.API.Application.DTOs.InRtgsCbc;
using LIBPROPERTY.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIB.API.Application.DTOs.OutRtgsCbc;

namespace LIB.API.Application.CQRS.OutRtgsCbc.Request.Command
{
    public class CreateOutRtgsCbcCommand : IRequest<BaseCommandResponse>
    {
        public OutRtgsCbcDto OutRtgsCbcDto { get; set; }
    }
}
