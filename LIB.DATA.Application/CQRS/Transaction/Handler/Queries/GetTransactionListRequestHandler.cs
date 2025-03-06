using AutoMapper;
using LIB.API.Application.Contracts.Persistent;
using LIB.API.Application.CQRS.Transaction.Request.Queries;
using LIB.API.Application.DTOs.Transaction;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.Transaction.Handler.Queries
{
    public class GetTransactionListRequestHandler : IRequestHandler<GetTransactionListRequest, List<TransactionDto>>
    {
        private ITransactionSqlRepository _TransactionSqlRepository;
        private IMapper _mapper;
        public GetTransactionListRequestHandler(ITransactionSqlRepository TransactionSqlRepository, IMapper mapper)
        {
            _TransactionSqlRepository = TransactionSqlRepository;
            _mapper = mapper;
        }
        public async Task<List<TransactionDto>> Handle(GetTransactionListRequest request, CancellationToken cancellationToken)
        {
            var Transaction = await _TransactionSqlRepository.GetAll();
            var fur = Transaction.Where(s => s.Status != "1").ToList();
            return _mapper.Map<List<TransactionDto>>(fur);
        }
    }
}
