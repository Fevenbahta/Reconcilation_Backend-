using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.InReconciled.Request.Command
{
    public class DeleteInReconciledCommand : IRequest
    {
        public string Id { get; set; }
    }
}
