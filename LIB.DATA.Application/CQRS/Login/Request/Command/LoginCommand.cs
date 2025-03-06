using LIB.API.Application.DTOs.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.Login.Request.Command
{
    public class LoginCommand: IRequest<UserDto>
    {
        public string UserName { get; set; }
    public string Password { get; set; }
}
}
