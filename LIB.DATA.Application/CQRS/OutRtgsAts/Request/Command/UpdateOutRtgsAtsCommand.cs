using LIB.API.Application.DTOs.InRtgsAts.Validators;
using LIB.API.Application.DTOs.OutRtgsAts.Validators;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.OutRtgsAts.Request.Command
{
    public class UpdateOutRtgsAtsCommand : IRequest<Unit>
    {
        public OutRtgsAtsDto OutRtgsAtsDto { get; set; }
    }
}
