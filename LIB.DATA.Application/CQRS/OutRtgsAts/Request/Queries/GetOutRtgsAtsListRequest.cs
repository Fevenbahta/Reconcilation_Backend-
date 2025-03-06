using LIB.API.Application.DTOs.InRtgsAts.Validators;
using LIB.API.Application.DTOs.OutRtgsAts.Validators;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.OutRtgsAts.Request.Queries
{
    public class GetOutRtgsAtsListRequest : IRequest<List<OutRtgsAtsDto>>
    {

    }
}
