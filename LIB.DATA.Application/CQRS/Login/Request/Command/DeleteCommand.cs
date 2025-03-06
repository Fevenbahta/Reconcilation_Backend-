using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.Login.Request.Command
{
    public class DeleteCommand : IRequest
    {
        public int Id { get; set; }
    }
}
