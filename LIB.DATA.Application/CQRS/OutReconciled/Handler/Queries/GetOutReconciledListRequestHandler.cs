using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.OutReconciled.Request.Queries;

using LIB.API.Application.DTOs.OutReconciled;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.OutReconciled.Handler.Queries
{
    public class GetOutReconciledListRequestHandler : IRequestHandler<GetOutReconciledListRequest, List<OutReconciledDto>>
    {
        public IOutReconciledRepository _OutReconciledRepository;
        public IMapper _mapper;
        public GetOutReconciledListRequestHandler(IOutReconciledRepository OutReconciledRepository, IMapper mapper)
        {
            _OutReconciledRepository = OutReconciledRepository;
            _mapper = mapper;
        }
        public async Task<List<OutReconciledDto>> Handle(GetOutReconciledListRequest request, CancellationToken cancellationToken)
        {
            var OutReconciled = await _OutReconciledRepository.GetAll();
            var fur = OutReconciled.ToList();
            return _mapper.Map<List<OutReconciledDto>>(fur);
        }
    }
}
