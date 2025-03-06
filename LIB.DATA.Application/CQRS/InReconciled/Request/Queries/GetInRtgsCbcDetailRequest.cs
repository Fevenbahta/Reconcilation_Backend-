
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIB.API.Application.DTOs.InReconciled;

namespace LIB.API.Application.CQRS.InReconciled.Request.Queries
{
    public class GetInReconciledDetailRequest : IRequest<InReconciledDto>
    {
        public int Id { get; set; }
    }
}
