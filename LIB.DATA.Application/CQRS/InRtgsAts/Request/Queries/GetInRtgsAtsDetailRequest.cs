using LIB.API.Application.DTOs.InRtgsAts.Validators;
using LIB.API.Application.DTOs.InRtgsCbc;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.InRtgsAts.Request.Queries
{
    public class GetInRtgsAtsDetailRequest : IRequest<InRtgsAtsDto>
    {
        public string Id { get; set; }
    }
}
