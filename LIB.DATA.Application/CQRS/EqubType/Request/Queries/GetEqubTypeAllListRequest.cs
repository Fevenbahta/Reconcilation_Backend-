using LIB.API.Application.DTOs.EqubType.Validators;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.EqubType.Request.Queries
{
    public class GetEqubTypeAllListRequest : IRequest<List<EqubTypeDto>>
    {

    }
}
