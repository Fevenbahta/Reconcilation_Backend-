using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.InReconciled.Request.Queries;

using LIB.API.Application.DTOs.InReconciled;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.InReconciled.Handler.Queries
{
    public class GetInReconciledListRequestHandler : IRequestHandler<GetInReconciledListRequest, List<InReconciledDto>>
    {
        public IInReconciledRepository _InReconciledRepository;
        public IMapper _mapper;
        public GetInReconciledListRequestHandler(IInReconciledRepository InReconciledRepository, IMapper mapper)
        {
            _InReconciledRepository = InReconciledRepository;
            _mapper = mapper;
        }
        public async Task<List<InReconciledDto>> Handle(GetInReconciledListRequest request, CancellationToken cancellationToken)
        {
            var InReconciled = await _InReconciledRepository.GetAll();
            var fur = InReconciled.ToList();
            return _mapper.Map<List<InReconciledDto>>(fur);
        }
    }
}
