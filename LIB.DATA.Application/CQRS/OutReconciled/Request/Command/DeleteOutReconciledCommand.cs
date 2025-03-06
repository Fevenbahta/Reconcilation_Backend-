using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.OutReconciled.Request.Command
{
    public class DeleteOutReconciledCommand : IRequest
    {
        public string Id { get; set; }
    }
}
