/*using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.OutRtgsCbc.Request.Command;
using LIB.API.Application.DTOs.OutRtgsCbc.Validators;
using LIB.API.Domain;
using LIBPROPERTY.Application.Response;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.OutRtgsCbc.Handler.Command
{
    public class CreateOutRtgsCbcCommandHandler : IRequestHandler<CreateOutRtgsCbcCommand, BaseCommandResponse>
    {
        BaseCommandResponse response;
        public IOutRtgsCbcRepository _OutRtgsCbcRepository;
        private readonly IOracleDataRepository _oracleDataRepository;
        public IMapper _mapper;
        public CreateOutRtgsCbcCommandHandler(IOutRtgsCbcRepository OutRtgsCbcRepository,IOracleDataRepository oracleDataRepository, IMapper mapper)
        {
            _OutRtgsCbcRepository = OutRtgsCbcRepository;
         _oracleDataRepository = oracleDataRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommandResponse> Handle(CreateOutRtgsCbcCommand request, CancellationToken cancellationToken)
        {
            response = new BaseCommandResponse();
            var validator = new OutRtgsCbcDtoValidators();
            var validationResult = await validator.ValidateAsync(request.OutRtgsCbcDto);

            try
            {
                if (!validationResult.IsValid)
                {
                    response.Success = false;
                    response.Message = "Creation Failed";
                    response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    return response;
                }
                var yesterday = DateTime.Now.AddDays(-1).Date;
                var formattedDate = yesterday.ToString("dd-MMM-yy", System.Globalization.CultureInfo.InvariantCulture);

                var oracleData = await _oracleDataRepository.GetDataAsync(formattedDate);

                var OutRtgsCbc = _mapper.Map<OutRtgsCbcs>(request.OutRtgsCbcDto);

                // Process the data and create OutRtgsCbc as needed

                var data = await _OutRtgsCbcRepository.Add(OutRtgsCbc);
                response.Success = true;
                response.Message = "Creation Successful";
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
}*/



using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.OutRtgsCbc.Request.Command;
using LIB.API.Application.DTOs.OutRtgsCbc.Validators;
using LIB.API.Domain;
using LIBPROPERTY.Application.Response;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.OutRtgsCbc.Handler.Command
{
    public class CreateOutRtgsCbcCommandHandler : IRequestHandler<CreateOutRtgsCbcCommand, BaseCommandResponse>
    {
        private readonly IOutRtgsCbcOracleRepository _outRtgsCbcOracleRepository;
        private readonly IOutRtgsCbcRepository _outRtgsCbcRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly string _remoteConnectionString;

        public CreateOutRtgsCbcCommandHandler(
            IOutRtgsCbcOracleRepository outRtgsCbcOracleRepository, IOutRtgsCbcRepository outRtgsCbcRepository,
            IMapper mapper,
            IConfiguration configuration)
        {
            _outRtgsCbcOracleRepository = outRtgsCbcOracleRepository;
            _outRtgsCbcRepository = outRtgsCbcRepository;
            _mapper = mapper;
            _configuration = configuration;
            _remoteConnectionString = _configuration.GetConnectionString("LIBAPIConnectionString");
        }

        public async Task<BaseCommandResponse> Handle(CreateOutRtgsCbcCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new OutRtgsCbcDtoValidators();
            var validationResult = await validator.ValidateAsync(request.OutRtgsCbcDto);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Creation Failed";
                response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return response;
            }
            try
            {
                // Fetch yesterday's date


                var yesterday = DateTime.Now.AddDays(-6).Date;
                // Fetch data from the repository

                var formattedYesterday = yesterday.ToString("yyMMdd");

                // Get the last processed date from the repository
                var lastProcessedDateStr = await _outRtgsCbcRepository.GetLastProcessedDateAsync();

                // Compare the dates
                if (lastProcessedDateStr == null || lastProcessedDateStr != formattedYesterday)
                {

                    var outRtgsCbcsFromRemote = await _outRtgsCbcOracleRepository.GetOutRtgsCbcsByDateAsync(yesterday);

                    foreach (var outRtgsCbcRemote in outRtgsCbcsFromRemote)
                    {
                        var outRtgsCbc = _mapper.Map<OutRtgsCbcs>(outRtgsCbcRemote);
                        await _outRtgsCbcRepository.Add(outRtgsCbc);
                    }

                    response.Success = true;
                    response.Message = "Creation Successful";
                }
                else
                {
                    response.Success = true;
                    response.Message = "No new data to process";
                }
                response.Success = true;
                response.Message = "Creation Successful";
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
