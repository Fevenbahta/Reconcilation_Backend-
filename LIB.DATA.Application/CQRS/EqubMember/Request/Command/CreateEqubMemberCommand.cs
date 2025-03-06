using LIB.API.Application.DTOs.EqubMember;
using LIB.API.Application.DTOs.EqubType.Validators;
using LIB.API.Application.DTOs.Transaction;
using LIBPROPERTY.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.EqubMember.Request.Command
{
    public class CreateEqubMemberCommand : IRequest<BaseCommandResponse>
    {
        public EqubMemberDto EqubMemberDto { get; set; }
    }
}
