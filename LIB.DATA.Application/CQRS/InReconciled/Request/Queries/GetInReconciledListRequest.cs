
using LIB.API.Application.DTOs.InRtgsCbc;
using LIB.API.Application.DTOs.InReconciled;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.InReconciled.Request.Queries
{
    public class GetInReconciledListRequest : IRequest<List<InReconciledDto>>
    {

    }
}
