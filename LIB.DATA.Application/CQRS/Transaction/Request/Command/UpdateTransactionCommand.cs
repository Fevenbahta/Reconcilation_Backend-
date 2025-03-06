using LIB.API.Application.DTOs.Transaction;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.Transaction.Request.Command
{
    public class UpdateTransactionCommand : IRequest<Unit>
    {
        public TransactionDto TransactionDto { get; set; }
    }

}

