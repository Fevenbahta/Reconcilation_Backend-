using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.Contracts.Persistent;
using LIB.API.Application.DTOs.OutRtgsCbc;
using LIB.API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.Contracts.Persistence
{
    public interface IOutRtgsCbcRepository : IGenericRepository<OutRtgsCbcs>
    {
        Task<string> GenerateNextIdAsync();
        Task<List<OutRtgsCbcDto>> GetOutRtgsCbcDByDateIntervalAsync(DateTime startDate, DateTime endDate);

        Task<string> GetLastProcessedDateAsync();

    }
}
