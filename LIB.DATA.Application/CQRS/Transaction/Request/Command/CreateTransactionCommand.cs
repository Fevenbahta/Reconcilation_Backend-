
using LIB.API.Application.DTOs.Transaction;
using LIBPROPERTY.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.Transaction.Request.Command
{
    public class CreateTransactionCommand : IRequest<BaseCommandResponse>
    {
        public TransactionDto TransactionDto { get; set; }
    }
}
