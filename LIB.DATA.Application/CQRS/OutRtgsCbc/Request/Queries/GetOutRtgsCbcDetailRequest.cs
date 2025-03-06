
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIB.API.Application.DTOs.OutRtgsCbc;

namespace LIB.API.Application.CQRS.OutRtgsCbc.Request.Queries
{
    public class GetOutRtgsCbcDetailRequest : IRequest<OutRtgsCbcDto>
    {
        public int Id { get; set; }
    }
}
