/*using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.InRtgsCbc.Request.Command;
using LIB.API.Application.DTOs.InRtgsCbc.Validators;
using LIB.API.Domain;
using LIBPROPERTY.Application.Response;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.InRtgsCbc.Handler.Command
{
    public class CreateInRtgsCbcCommandHandler : IRequestHandler<CreateInRtgsCbcCommand, BaseCommandResponse>
    {
        BaseCommandResponse response;
        public IInRtgsCbcRepository _InRtgsCbcRepository;
        private readonly IOracleDataRepository _oracleDataRepository;
        public IMapper _mapper;
        public CreateInRtgsCbcCommandHandler(IInRtgsCbcRepository InRtgsCbcRepository,IOracleDataRepository oracleDataRepository, IMapper mapper)
        {
            _InRtgsCbcRepository = InRtgsCbcRepository;
         _oracleDataRepository = oracleDataRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommandResponse> Handle(CreateInRtgsCbcCommand request, CancellationToken cancellationToken)
        {
            response = new BaseCommandResponse();
            var validator = new InRtgsCbcDtoValidators();
            var validationResult = await validator.ValidateAsync(request.InRtgsCbcDto);

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

                var InRtgsCbc = _mapper.Map<InRtgsCbcs>(request.InRtgsCbcDto);

                // Process the data and create InRtgsCbc as needed

                var data = await _InRtgsCbcRepository.Add(InRtgsCbc);
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
using LIB.API.Application.CQRS.InRtgsCbc.Request.Command;
using LIB.API.Application.DTOs.InRtgsCbc.Validators;
using LIB.API.Domain;
using LIBPROPERTY.Application.Response;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.InRtgsCbc.Handler.Command
{
    public class CreateInRtgsCbcCommandHandler : IRequestHandler<CreateInRtgsCbcCommand, BaseCommandResponse>
    {
        private readonly IInRtgsCbcOracleRepository _InRtgsCbcOracleRepository;
        private readonly IInRtgsCbcRepository _InRtgsCbcRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly string _remoteConnectionString;

        public CreateInRtgsCbcCommandHandler(
            IInRtgsCbcOracleRepository InRtgsCbcOracleRepository, IInRtgsCbcRepository InRtgsCbcRepository,
            IMapper mapper,
            IConfiguration configuration)
        {
            _InRtgsCbcOracleRepository = InRtgsCbcOracleRepository;
            _InRtgsCbcRepository = InRtgsCbcRepository;
            _mapper = mapper;
            _configuration = configuration;
            _remoteConnectionString = _configuration.GetConnectionString("LIBAPIConnectionString");
        }

        public async Task<BaseCommandResponse> Handle(CreateInRtgsCbcCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new InRtgsCbcDtoValidators();
            var validationResult = await validator.ValidateAsync(request.InRtgsCbcDto);

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


                var yesterday = DateTime.Now.AddDays(-1).Date;
                // Fetch data from the repository

                var formattedYesterday = yesterday.ToString("yyMMdd");

                // Get the last processed date from the repository
                var lastProcessedDateStr = await _InRtgsCbcRepository.GetLastProcessedDateAsync();

                // Compare the dates
                if (lastProcessedDateStr == null || lastProcessedDateStr != yesterday)
                {

                    var InRtgsCbcsFromRemote = await _InRtgsCbcOracleRepository.GetInRtgsCbcsByDateAsync(yesterday);

                    foreach (var InRtgsCbcRemote in InRtgsCbcsFromRemote)

                    {
                        var InRtgsCbc = _mapper.Map<InRtgsCbcs>(InRtgsCbcRemote);
                        await _InRtgsCbcRepository.Add(InRtgsCbc);
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
