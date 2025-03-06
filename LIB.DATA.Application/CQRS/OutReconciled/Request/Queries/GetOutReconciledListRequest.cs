
using LIB.API.Application.DTOs.InRtgsCbc;
using LIB.API.Application.DTOs.OutReconciled;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.OutReconciled.Request.Queries
{
    public class GetOutReconciledListRequest : IRequest<List<OutReconciledDto>>
    {

    }
}
