
using LIB.API.Application.DTOs.InRtgsCbc;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIB.API.Application.DTOs.InReconciled;

namespace LIB.API.Application.CQRS.InReconciled.Request.Command
{
    public class UpdateInReconciledCommand : IRequest<Unit>
    {
        public InReconciledDto InReconciledDto { get; set; }
    }
}
