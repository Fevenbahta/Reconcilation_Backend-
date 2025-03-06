using LIB.API.Application.DTOs.InRtgsAts.Validators;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.InRtgsAts.Request.Queries
{
    public class GetInRtgsAtsAllListRequest : IRequest<List<InRtgsAtsDto>>
    {

    }
}
