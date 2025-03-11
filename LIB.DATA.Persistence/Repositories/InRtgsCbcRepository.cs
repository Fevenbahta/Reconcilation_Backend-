
using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.DTOs.InRtgsCbc;
using LIB.API.Application.DTOs.OutRtgsCbc;
using LIB.API.Domain;
using LIBPROPERTY.Persistence.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LIB.API.Persistence.Repositories
{
    public class InRtgsCbcRepository : GenericRepository<InRtgsCbcs>, IInRtgsCbcRepository
    {
        public readonly LIBAPIDbSQLContext _context;
        public readonly IMapper _mapper;

        public readonly IMediator _mediator;

        public InRtgsCbcRepository(LIBAPIDbSQLContext context, IMapper mapper, IMediator mediator) : base(context)
        {
            _context = context;
            _mapper = mapper;
            //     _InRtgsCbcsqlRepository = InRtgsCbcsqlRepository;
    
            _mediator = mediator;
        }


        public async Task<InRtgsCbcs> GetInRtgsCbcByReferenceNoAsync(string referenceNo)
        {
            var InRtgsCbc = await _context.InRtgsCbcs
                .FirstOrDefaultAsync(t => t.REFNO == referenceNo);

            if (InRtgsCbc == null) return null;

            return new InRtgsCbcs
            {
               
            };
        }



        public async Task<List<InRtgsCbcDto>> GetInRtgsCbcDByDateIntervalAsync(DateTime startDate, DateTime endDate)
        {
           
            return await _context.InRtgsCbcs
                .Where(t =>
               t.TRANSACTION_DATE.Date >= startDate.Date && t.TRANSACTION_DATE.Date <= endDate.Date)
                         .Select(t => new InRtgsCbcDto
                {
                    REFNO = t.REFNO,
                    TRANSACTION_DATE = t.TRANSACTION_DATE,
                    INPUTING_BRANCH = t.INPUTING_BRANCH,
                    AMOUNT = t.AMOUNT,
                    DISCRIPTION = t.DISCRIPTION,
                    DEBITOR_NAME = t.DEBITOR_NAME,
                    ACCOUNT = t.ACCOUNT,
                    BRANCH = t.BRANCH
                })
                .ToListAsync();
        }



        public async Task<DateTime?> GetLastProcessedDateAsync()
        {
            var lastRecord = await _context.InRtgsCbcs
                .OrderByDescending(l => l.TRANSACTION_DATE)  // Assuming DATET is the date field
                .Select(l => l.TRANSACTION_DATE)  // Select only the date field
                .FirstOrDefaultAsync();

            return lastRecord;
        }


    }
}
