using LIB.API.Application.DTOs.User;
using LIBPROPERTY.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.Login.Request.Command
{
    public class RegisterCommand :IRequest<BaseCommandResponse>
    {

        public UserDto userDto { get; set; }
}
}
