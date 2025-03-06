
using LIB.API.Application.DTOs.InRtgsCbc;
using LIB.API.Application.DTOs.OutRtgsCbc;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.OutRtgsCbc.Request.Queries
{
    public class GetOutRtgsCbcListRequest : IRequest<List<OutRtgsCbcDto>>
    {

    }
}
