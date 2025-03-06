
using LIB.API.Application.DTOs.InRtgsAts.Validators;
using LIB.API.Application.DTOs.InRtgsCbc;
using LIBPROPERTY.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIB.API.Application.DTOs.OutReconciled;

namespace LIB.API.Application.CQRS.OutReconciled.Request.Command
{
    public class CreateOutReconciledCommand : IRequest<BaseCommandResponse>
    {
        public OutReconciledDto OutReconciledDto { get; set; }
    }
}
