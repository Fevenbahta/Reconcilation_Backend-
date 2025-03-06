
using LIB.API.Application.DTOs.InRtgsAts.Validators;
using LIB.API.Application.DTOs.InRtgsCbc;
using LIBPROPERTY.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIB.API.Application.DTOs.InReconciled;

namespace LIB.API.Application.CQRS.InReconciled.Request.Command
{
    public class CreateInReconciledCommand : IRequest<BaseCommandResponse>
    {
        public InReconciledDto InReconciledDto { get; set; }
    }
}
