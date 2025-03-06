
using LIB.API.Application.DTOs.Transaction;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.Transaction.Request.Queries
{
    public class GetTransactionDetaileRequest : IRequest<TransactionDto>
    {
        public int Id { get; set; }
    }
}
