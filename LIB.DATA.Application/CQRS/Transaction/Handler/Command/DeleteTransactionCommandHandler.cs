using AutoMapper;
using LIB.API.Application.Contracts.Persistent;
using LIB.API.Application.CQRS.Transaction.Request.Command;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.Transaction.Handler.Command
{
    public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand>
    {
        private ITransactionRepository _TransactionRepository;
        private IMapper _mapper;
        public DeleteTransactionCommandHandler(ITransactionRepository TransactionRepository, IMapper mapper)
        {
            _TransactionRepository = TransactionRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
        {
            var Transaction = await _TransactionRepository.GetById(request.Id);
            await _TransactionRepository.Delete(Transaction);
            return Unit.Value;
        }

        async Task IRequestHandler<DeleteTransactionCommand>.Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
        {
            var Transaction = await _TransactionRepository.GetById(request.Id);
            if (Transaction == null)
                throw new NotFoundException(nameof(Transaction), request.Id);
            Transaction.Status = "1";
            await _TransactionRepository.Update(Transaction);

        }
    }
}
