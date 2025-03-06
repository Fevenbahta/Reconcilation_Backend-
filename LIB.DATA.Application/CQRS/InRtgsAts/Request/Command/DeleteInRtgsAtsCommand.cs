using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.InRtgsAts.Request.Command
{
    public class DeleteInRtgsAtsCommand : IRequest
    {
        public string Id { get; set; }
    }
}
