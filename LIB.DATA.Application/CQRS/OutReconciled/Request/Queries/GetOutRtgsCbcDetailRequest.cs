
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIB.API.Application.DTOs.OutReconciled;

namespace LIB.API.Application.CQRS.OutReconciled.Request.Queries
{
    public class GetOutReconciledDetailRequest : IRequest<OutReconciledDto>
    {
        public int Id { get; set; }
    }
}
