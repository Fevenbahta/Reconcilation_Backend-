
using LIB.API.Application.DTOs.InRtgsCbc;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIB.API.Application.DTOs.OutReconciled;

namespace LIB.API.Application.CQRS.OutReconciled.Request.Command
{
    public class UpdateOutReconciledCommand : IRequest<Unit>
    {
        public OutReconciledDto OutReconciledDto { get; set; }
    }
}
