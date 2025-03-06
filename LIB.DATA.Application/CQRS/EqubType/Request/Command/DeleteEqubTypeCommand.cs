using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.EqubType.Request.Command
{
    public class DeleteEqubTypeCommand : IRequest
    {
        public string Id { get; set; }
    }
}
