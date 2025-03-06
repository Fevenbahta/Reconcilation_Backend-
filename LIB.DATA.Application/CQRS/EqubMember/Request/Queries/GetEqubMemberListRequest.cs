using LIB.API.Application.DTOs.EqubMember;
using LIB.API.Application.DTOs.Transaction;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.EqubMember.Request.Queries
{
    public class GetEqubMemberListRequest : IRequest<List<EqubMemberDto>>
    {

    }
}
