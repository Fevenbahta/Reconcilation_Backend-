using LIB.API.Application.DTOs.EqubType.Validators;
using LIB.API.Application.DTOs.Transaction;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.EqubType.Request.Command
{
    public class UpdateEqubTypeCommand : IRequest<Unit>
    {
        public EqubTypeDto EqubTypeDto { get; set; }
    }
}
