/*using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.OutReconciled.Request.Command;
using LIB.API.Application.DTOs.OutReconciled.Validators;
using LIB.API.Domain;
using LIBPROPERTY.Application.Response;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.OutReconciled.Handler.Command
{
    public class CreateOutReconciledCommandHandler : IRequestHandler<CreateOutReconciledCommand, BaseCommandResponse>
    {
        BaseCommandResponse response;
        public IOutReconciledRepository _OutReconciledRepository;
        private readonly IOracleDataRepository _oracleDataRepository;
        public IMapper _mapper;
        public CreateOutReconciledCommandHandler(IOutReconciledRepository OutReconciledRepository,IOracleDataRepository oracleDataRepository, IMapper mapper)
        {
            _OutReconciledRepository = OutReconciledRepository;
         _oracleDataRepository = oracleDataRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommandResponse> Handle(CreateOutReconciledCommand request, CancellationToken cancellationToken)
        {
            response = new BaseCommandResponse();
            var validator = new OutReconciledDtoValidators();
            var validationResult = await validator.ValidateAsync(request.OutReconciledDto);

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

                var OutReconciled = _mapper.Map<OutReconcileds>(request.OutReconciledDto);

                // Process the data and create OutReconciled as needed

                var data = await _OutReconciledRepository.Add(OutReconciled);
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
using LIB.API.Application.CQRS.OutReconciled.Request.Command;

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

namespace LIB.API.Application.CQRS.OutReconciled.Handler.Command
{
    public class CreateOutReconciledCommandHandler : IRequestHandler<CreateOutReconciledCommand, BaseCommandResponse>
    {
         private readonly IOutReconciledRepository _OutReconciledRepository;
        private readonly IOutRtgsAtsRepository _OutRtgsAtsRepository;
        private readonly IOutRtgsCbcRepository _OutRtgsCbcRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly string _remoteConnectionString;

        public CreateOutReconciledCommandHandler(
           IOutReconciledRepository OutReconciledRepository,IOutRtgsAtsRepository outRtgsAtsRepository,IOutRtgsCbcRepository outRtgsCbcRepository,
            IMapper mapper,
            IConfiguration configuration)
        {
          
            _OutReconciledRepository = OutReconciledRepository;
            _OutRtgsAtsRepository = outRtgsAtsRepository;
            _OutRtgsCbcRepository = outRtgsCbcRepository;
            _mapper = mapper;
            _configuration = configuration;
            _remoteConnectionString = _configuration.GetConnectionString("LIBAPIConnectionString");
        }

        public async Task<BaseCommandResponse> Handle(CreateOutReconciledCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new OutReconciledDtoValidators();
            var validationResult = await validator.ValidateAsync(request.OutReconciledDto);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Creation Failed";
                response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return response;
            }

            try
            {
                // Fetch all data from both repositories
                var outrtgsatsData = await _OutRtgsAtsRepository.GetAll();
                var outrtgscbcData = await _OutRtgsCbcRepository.GetAll();

                var reconciledItems = new List<OutReconcileds>();

                // Loop through the OutRtgsAts data
                foreach (var atsItem in outrtgsatsData)
                {
                    // Find a matching item in OutRtgsCbc data using the Reference field
                    var matchingCbcItem = outrtgscbcData.FirstOrDefault(cbcItem =>
                    {
                        // Step 1: Compare the reference number
                        string refNo = cbcItem.REFNO;
                        string atsRef = atsItem.Reference;

                   

                        if (refNo.Length >= 16 && atsRef.Length >= 16)
                        {
                            // Strip the last digit from both references if they are 16 or more digits long
                            refNo = refNo.Substring(0, refNo.Length - 1);
                            atsRef = atsRef.Substring(0, atsRef.Length - 1);
                            if (refNo == atsRef)
                            {
                                var k = 0;
                            }
                        }

                        // Step 2: Compare the account number (assuming it's a string)
               /*         string cbcDate =cbcItem.DATET;

                        string referenceStr = atsItem.BusinessDate.ToString();

                        // Extract the year, month, and day
                        string year = referenceStr.Substring(0, 4); // 2024
                        string month = referenceStr.Substring(4, 2); // 09
                        string day = referenceStr.Substring(6, 2); // 05*/

                        // Convert to DateTime and format as DD-MM-YY
                   /*     DateTime date = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
                        string formattedDate = date.ToString("dd-MM-yy");

                        string atsDate = formattedDate;*/

            
                        // Step 3: Compare the amount (assuming amount is a decimal or numeric type)
                        decimal cbcAmount = cbcItem.AMOUNT;
                        decimal atsAmount = decimal.Parse(atsItem.Amount);

                        // Step 4: Check all conditions: reference, account, and amount
                        return refNo == atsRef  && cbcAmount == atsAmount;
                    });



                    if (matchingCbcItem != null)
                    {
                        // Create a new reconciled record, taking specific fields from the CBC item
                        var reconciledItem = new OutReconcileds
                        {
                    
                            BRANCH = matchingCbcItem.BRANCH, // Get BRANCH from CBC
                            ACCOUNT = matchingCbcItem.ACCOUNT, // Get ACCOUNT from CBC
                            DISCRIPTION = matchingCbcItem.DISCRIPTION, // Get DISCRIPTION from CBC
                            AMOUNT = matchingCbcItem.AMOUNT, // Get AMOUNT from CBC
                            INPUTING_BRANCH = matchingCbcItem.INPUTING_BRANCH, // Get INPUTING_BRANCH from CBC
                            DATET = matchingCbcItem.DATET, // Get DATET from CBC
                            Type = atsItem.Type, // Keep the Type from the atsItem
                            Reference = atsItem.Reference, // Keep the Reference from the atsItem
                            Debitor = atsItem.Debitor, // Keep the Debitor from the atsItem
                            Creditor = atsItem.Creditor, // Keep the Creditor from the atsItem
                            OrderingAccount = atsItem.OrderingAccount, // Keep OrderingAccount from the atsItem
                            BeneficiaryAccount = atsItem.BeneficiaryAccount, // Keep BeneficiaryAccount from the atsItem
                            BusinessDate = atsItem.BusinessDate, // Keep BusinessDate from the atsItem
                            EntryDate = atsItem.EntryDate, // Keep EntryDate from the atsItem
                            Currency = atsItem.Currency, // Keep Currency from the atsItem
                            ProcessingStatus = atsItem.ProcessingStatus, // Keep ProcessingStatus from the atsItem
                            Status = atsItem.Status // Keep Status from the atsItem
                        };

                        // Add the reconciled item to the list
             
                        await _OutReconciledRepository.Add(reconciledItem);

                        // Remove the matched items from both repositories
                        await _OutRtgsAtsRepository.Delete(atsItem);
                        await _OutRtgsCbcRepository.Delete(matchingCbcItem);
                    }
                }

                // Insert all reconciled items into OutReconciled repository
                /*foreach (var item in reconciledItems)
                {
                    await _OutReconciledRepository.Add(item);
                }*/

                response.Success = true;
                response.Message = "Reconciliation Successful";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Reconciliation Failed due to an unexpected error";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }

    }
}
