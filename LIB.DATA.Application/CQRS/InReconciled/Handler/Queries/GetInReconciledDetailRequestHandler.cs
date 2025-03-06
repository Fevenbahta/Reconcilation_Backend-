using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.InReconciled.Request.Queries;
using LIB.API.Application.DTOs.InReconciled;
using LIBPROPERTY.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.InReconciled.Handler.Queries
{
    public class GetInReconciledDetailRequestHandler : IRequestHandler<GetInReconciledDetailRequest, InReconciledDto>
    {
        public IInReconciledRepository _InReconciledRepository;
        public IMapper _mapper;
        public GetInReconciledDetailRequestHandler(IInReconciledRepository InReconciledRepository, IMapper mapper)
        {
            _InReconciledRepository = InReconciledRepository;
            _mapper = mapper;
        }
        public async Task<InReconciledDto> Handle(GetInReconciledDetailRequest request, CancellationToken cancellationToken)
        {
            var InReconciled = await _InReconciledRepository.GetById(request.Id);
            if (InReconciled == null )
                throw new NotFoundException(nameof(InReconciled), request.Id);
            return _mapper.Map<InReconciledDto>(InReconciled);
        }
    }
}
