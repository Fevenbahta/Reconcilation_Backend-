using LIB.API.Application.DTOs.EqubType.Validators;
using LIB.API.Application.DTOs.Transaction;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.EqubType.Request.Queries
{
    public class GetEqubTypeDetailRequest : IRequest<EqubTypeDto>
    {
        public string Id { get; set; }
    }
}
