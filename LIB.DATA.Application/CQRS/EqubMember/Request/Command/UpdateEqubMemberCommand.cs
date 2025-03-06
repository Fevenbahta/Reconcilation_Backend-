using LIB.API.Application.DTOs.EqubMember;
using LIB.API.Application.DTOs.Transaction;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.EqubMember.Request.Command
{
    public class UpdateEqubMemberCommand : IRequest<Unit>
    {
        public EqubMemberDto EqubMemberDto { get; set; }
    }
}
