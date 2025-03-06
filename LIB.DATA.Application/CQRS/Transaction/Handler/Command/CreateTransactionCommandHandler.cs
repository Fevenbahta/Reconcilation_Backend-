using AutoMapper;
using LIB.API.Application.Contracts.Persistent;
using LIB.API.Application.CQRS.Transaction.Request.Command;
using LIB.API.Application.DTOs.Transaction;
using LIB.API.Application.DTOs.Transaction.Validators;
using LIB.API.Domain;
using LIBPROPERTY.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.Transaction.Handler.Command
{
    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, BaseCommandResponse>
    {
        BaseCommandResponse response;
        private ITransactionSqlRepository _TransactionSqlRepository;
        private IMapper _mapper;
        public CreateTransactionCommandHandler(ITransactionSqlRepository TransactionSqlRepository, IMapper mapper)
        {
            _TransactionSqlRepository = TransactionSqlRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommandResponse> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            response = new BaseCommandResponse();
            var validator = new TransactionDtoValidators();
            var validationResult = await validator.ValidateAsync(request.TransactionDto);
            try
            {
                if (validationResult.IsValid == false)
                {
                    response.Success = false;
                    response.Message = "Creation Faild";
                    response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                }

                var Transaction = _mapper.Map<Transactions>(request.TransactionDto);


                var data = await _TransactionSqlRepository.Add(Transaction);
                response.Success = true;
                response.Message = "Creation Successfull";


            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Creation Failed due to an unexpected error";
                response.Errors = new List<string> { ex.Message };
            }
                return response;
        }
    }
}
