
using LIB.API.Application.Contracts.Persistent;
using LIB.API.Application.DTOs.InRtgsCbc;
using LIB.API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.Contracts.Persistence
{
    public interface IInRtgsCbcRepository : IGenericRepository<InRtgsCbcs>
    {
             Task<InRtgsCbcs> GetInRtgsCbcByReferenceNoAsync(string referenceNo);
        Task<DateTime?> GetLastProcessedDateAsync();
    }
}
