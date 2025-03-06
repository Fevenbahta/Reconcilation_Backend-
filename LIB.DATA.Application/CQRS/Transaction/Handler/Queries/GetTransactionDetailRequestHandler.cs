using AutoMapper;
using LIB.API.Application.Contracts.Persistent;
using LIB.API.Application.CQRS.Transaction.Request.Queries;
using LIB.API.Application.DTOs.Transaction;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.Transaction.Handler.Queries
{
    public class GetEmployeDetaileRequestHandler : IRequestHandler<GetTransactionDetaileRequest, TransactionDto>
    {
        private ITransactionSqlRepository _TransactionSqlRepository;
        private IMapper _mapper;
        public GetEmployeDetaileRequestHandler(ITransactionSqlRepository TransactionSqlRepository, IMapper mapper)
        {
            _TransactionSqlRepository = TransactionSqlRepository;
            _mapper = mapper;
        }
        public async Task<TransactionDto> Handle(GetTransactionDetaileRequest request, CancellationToken cancellationToken)
        {
            var Transaction = await _TransactionSqlRepository.GetById(request.Id);
            if (Transaction == null || Transaction

                .Status == "1")
                throw new NotFoundException(nameof(Transaction), request.Id);
            return _mapper.Map<TransactionDto>(Transaction);
        }
    }
}
