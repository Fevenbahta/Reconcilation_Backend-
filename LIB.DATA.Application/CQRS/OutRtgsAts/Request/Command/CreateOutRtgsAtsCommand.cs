using LIB.API.Application.DTOs.InRtgsAts.Validators;
using LIB.API.Application.DTOs.OutRtgsAts.Validators;
using LIBPROPERTY.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.OutRtgsAts.Request.Command
{
    public class CreateOutRtgsAtsCommand : IRequest<BaseCommandResponse>
    {
        public OutRtgsAtsDto OutRtgsAtsDto { get; set; }
    }
}
