using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.OutRtgsAts.Request.Command
{
    public class DeleteOutRtgsAtsCommand : IRequest
    {
        public int Id { get; set; }
    }
}
