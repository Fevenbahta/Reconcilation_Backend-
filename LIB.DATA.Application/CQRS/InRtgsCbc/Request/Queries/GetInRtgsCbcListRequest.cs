
using LIB.API.Application.DTOs.InRtgsCbc;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.InRtgsCbc.Request.Queries
{
    public class GetInRtgsCbcListRequest : IRequest<List<InRtgsCbcDto>>
    {

    }
}
