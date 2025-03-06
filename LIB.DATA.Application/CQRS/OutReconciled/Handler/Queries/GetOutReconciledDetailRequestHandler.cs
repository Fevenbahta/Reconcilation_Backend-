using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.OutReconciled.Request.Queries;
using LIB.API.Application.DTOs.OutReconciled;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.OutReconciled.Handler.Queries
{
    public class GetOutReconciledDetailRequestHandler : IRequestHandler<GetOutReconciledDetailRequest, OutReconciledDto>
    {
        public IOutReconciledRepository _OutReconciledRepository;
        public IMapper _mapper;
        public GetOutReconciledDetailRequestHandler(IOutReconciledRepository OutReconciledRepository, IMapper mapper)
        {
            _OutReconciledRepository = OutReconciledRepository;
            _mapper = mapper;
        }
        public async Task<OutReconciledDto> Handle(GetOutReconciledDetailRequest request, CancellationToken cancellationToken)
        {
            var OutReconciled = await _OutReconciledRepository.GetById(request.Id);
            if (OutReconciled == null )
                throw new NotFoundException(nameof(OutReconciled), request.Id);
            return _mapper.Map<OutReconciledDto>(OutReconciled);
        }
    }
}
