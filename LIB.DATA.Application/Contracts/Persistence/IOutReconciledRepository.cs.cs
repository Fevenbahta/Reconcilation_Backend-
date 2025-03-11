using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.Contracts.Persistent;
using LIB.API.Application.DTOs.OutReconciled;
using LIB.API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.Contracts.Persistence
{
    public interface IOutReconciledRepository : IGenericRepository<OutReconcileds>
    {

        Task<List<OutReconciledDto>> GetInReconciledByDateIntervalAsync(DateTime startDate, DateTime endDate);

        Task<OutReconcileds> GetByRefNoAmountAndDate(string reference, decimal amount, DateTime businessDate);
         }
}

