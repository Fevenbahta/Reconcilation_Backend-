
using LIB.API.Application.DTOs.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.Login.Request.Queries
{
    public class GetLoginList : IRequest<List<UserDto>>
    {

    }
}
