using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.Contracts.Persistent;
using LIB.API.Application.DTOs.InReconciled;
using LIB.API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.Contracts.Persistence
{
    public interface IInReconciledRepository : IGenericRepository<InReconcileds>
    {

        Task<List<InReconciledDto>> GetInReconciledByDateIntervalAsync(DateTime startDate, DateTime endDate);


    }
}
