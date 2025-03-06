
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using LIB.API.Application.Contracts.Persistent;
using LIB.API.Application.CQRS.Transaction.Request.Command;
using LIB.API.Application.DTOs.Transaction.Validators;
using LIBPROPERTY.Application.Exceptions;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.ComponentModel.Design;
using LIB.API.Application.Contracts.Persistence  ;
using LIB.API.Domain;
using AutoMapper.Internal;

public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand, Unit>
{
    private readonly ITransactionSqlRepository _transactionSqlRepository;
    private readonly IEqubMemberRepository _equbMemberRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IEqubTypeRepository _equbTypeRepository;
    private readonly IMapper _mapper;
    private readonly SoapClient _soapClient;

    public UpdateTransactionCommandHandler(ITransactionSqlRepository transactionSqlRepository,
        IEqubMemberRepository equbMemberRepository,
        ITransactionRepository transactionRepository,
         IEqubTypeRepository equbTypeRepository,
        IMapper mapper, SoapClient soapClient)
    {
        _transactionSqlRepository = transactionSqlRepository;
        _equbMemberRepository = equbMemberRepository;
        _transactionRepository = transactionRepository;
        _equbTypeRepository = equbTypeRepository;
        _mapper = mapper;
        _soapClient = soapClient;

        ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

    }



    private string GenerateRequestId() => GenerateNumericId(17);

    private string GenerateMsgId() => GenerateAlphanumericId(21);


    private string GeneratePmtInfId(string equbTypeId, string equbMemberId)
    {
        
        // Ensure equbMemberId is not null and padded to a minimum length
        string memberIdString = equbMemberId.PadLeft(8, '0'); // Pads with zeros if less than 4 digits

        // Generate 12 alphanumeric characters
        string alphanumericPart = GenerateAlphanumericId(12); // Adjust length if needed

        // Combine the EqubTypeId, EqubMemberId, and the alphanumeric part
        string pmtInfId =  memberIdString + alphanumericPart;

        return pmtInfId;
    }


    private string GenerateInstrId() => GenerateAlphanumericId(32);

    private string GenerateEndToEndId() => GenerateAlphanumericId(30);

    private string GenerateNumericId(int length)
    {
        Random random = new Random();
        return new string(Enumerable.Range(0, length)
            .Select(_ => (char)('0' + random.Next(10))).ToArray());
    }

    private string GenerateAlphanumericId(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        Random random = new Random();
        return new string(Enumerable.Range(0, length)
            .Select(_ => chars[random.Next(chars.Length)]).ToArray());
    }
    private int GetTotalPeriods(string timeUnits, string timeQuantity, DateTime startDate, DateTime endDate)
    {
        int totalDays = (endDate - startDate).Days + 1;
        int periodQuantity = int.Parse(timeQuantity);

        switch (timeUnits.ToLower())
        {
            case "day":
                return totalDays / periodQuantity;
            case "week":
                return totalDays / (7 * periodQuantity);
            case "month":
                return totalDays / (30 * periodQuantity); // Approximation
            default:
                throw new ArgumentException("Invalid time units");
        }
    }
    private string GetCurrentTimestamp()
    {
        DateTime now = DateTime.UtcNow;
        string formattedTimestamp = now.ToString("yyyy-MM-ddTHH:mm:ss") +
                                    now.ToString(".fff"); // For milliseconds

        // Optionally, add more precision if needed (e.g., nanoseconds)
        // Since DateTime does not support nanoseconds, you might need to manually append them
        // Example: formattedTimestamp += "000000000"; // Assuming nanoseconds are not available

        return formattedTimestamp;
    }



    public async Task<Unit> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        var validator = new TransactionDtoValidators();
        var validationResult = await validator.ValidateAsync(request.TransactionDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult);


        var transaction = await _transactionSqlRepository.GetById(request.TransactionDto.Id);
        string requestId = GenerateRequestId();
        string msgId = GenerateMsgId();
        string pmtInfId = GeneratePmtInfId(request.TransactionDto.EqupType, request.TransactionDto.MemberId);
        string instrId = GenerateInstrId();
        string endToEndId = GenerateEndToEndId();


        string creDtTm = GetCurrentTimestamp();

        request.TransactionDto.TransDate = DateTime.Parse(creDtTm);

        if (request.TransactionDto.Status == "Approved")
        {



            request.TransactionDto.ReferenceNo = requestId;
            request.TransactionDto.MesssageNo = msgId;
            request.TransactionDto.PaymentNo = pmtInfId;

            // Build XML request with values from DTO


            string soapUrl = "https://10.1.7.85:8095/createTransfer";
            string soapAction = "createTransfer";
            string xmlRequest =$@"
<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:amp='http://soprabanking.com/amplitude'>
  <soapenv:Header/>
  <soapenv:Body>
    <amp:createTransferRequestFlow>
      <amp:requestHeader>
        <amp:requestId>{request.TransactionDto.ReferenceNo}</amp:requestId>
        <amp:serviceName>createTransfer</amp:serviceName>
        <amp:timestamp>{creDtTm}</amp:timestamp>
        <amp:originalName>EQUBAPP</amp:originalName>
        <amp:userCode>EQUBAPP</amp:userCode>
      </amp:requestHeader>
      <amp:createTransferRequest>
        <amp:canal>EQUB_IN</amp:canal>
        <amp:pain001><![CDATA[
          <Document xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns='urn:iso:std:iso:20022:tech:xsd:pain.001.001.03DB'>
            <CstmrCdtTrfInitn>
              <GrpHdr>
                <MsgId>{request.TransactionDto.MesssageNo}</MsgId>
                <CreDtTm>{creDtTm}</CreDtTm>
                <NbOfTxs>1</NbOfTxs>
                <CtrlSum>{request.TransactionDto.Amount}</CtrlSum>
                <InitgPty/>
                <DltPrvtData>
                  <FlwInd>PROD</FlwInd>
                  <DltPrvtDataDtl>
                    <PrvtDtInf>EQUB_IN</PrvtDtInf>
                    <Tp>
                      <CdOrPrtry>
                        <Cd>CHANNEL</Cd>
                      </CdOrPrtry>
                    </Tp>
                  </DltPrvtDataDtl>
                </DltPrvtData>
              </GrpHdr>
              <PmtInf>
                <PmtInfId>{request.TransactionDto.PaymentNo}</PmtInfId>
                <PmtMtd>TRF</PmtMtd>
                <BtchBookg>0</BtchBookg>
                <NbOfTxs>1</NbOfTxs>
                <CtrlSum>{request.TransactionDto.Amount}</CtrlSum>
                <DltPrvtData>
                  <OrdrPrties>
                    <Tp>IMM</Tp>
                    <Md>CREATE</Md>
                  </OrdrPrties>
                </DltPrvtData>
                <PmtTpInf>
                  <InstrPrty>NORM</InstrPrty>
                  <SvcLvl>
                    <Prtry>INTERNAL</Prtry>
                  </SvcLvl>
                </PmtTpInf>
                <ReqdExctnDt>1901-01-01</ReqdExctnDt>
                <Dbtr/>
                <DbtrAcct>
                  <Id>
                    <Othr>
                      <Id>{request.TransactionDto.DAccountNo}</Id>
                      <SchmeNm>
                        <Prtry>BKCOM_ACCOUNT</Prtry>
                      </SchmeNm>
                    </Othr>
                  </Id>
                  <Ccy>ETB</Ccy>
                </DbtrAcct>
                <DbtrAgt>
                  <FinInstnId>
                    <Nm>BANQUE</Nm>
                    <Othr>
                      <Id>00011</Id>
                      <SchmeNm>
                        <Prtry>ITF_DELTAMOP_IDETAB</Prtry>
                      </SchmeNm>
                    </Othr>
                  </FinInstnId>
                  <BrnchId>
                    <Id>{request.TransactionDto.CAccountBranch}</Id>
                    <Nm>Agence</Nm>
                  </BrnchId>
                </DbtrAgt>
                <CdtTrfTxInf>
                  <PmtId>
                    <InstrId>{instrId}</InstrId>
                    <EndToEndId>{endToEndId}</EndToEndId>
                  </PmtId>
                  <Amt>
                    <InstdAmt Ccy='ETB'>{request.TransactionDto.Amount}</InstdAmt>
                  </Amt>
                  <CdtrAgt>
                    <FinInstnId>
                      <Nm>BANQUE</Nm>
                      <Othr>
                        <Id>00011</Id>
                        <SchmeNm>
                          <Prtry>ITF_DELTAMOP_IDETAB</Prtry>
                        </SchmeNm>
                      </Othr>
                    </FinInstnId>
                    <BrnchId>
                      <Id>00003</Id>
                      <Nm>Agence</Nm>
                    </BrnchId>
                  </CdtrAgt>
                  <Cdtr/>
                  <CdtrAcct>
                    <Id>
                      <Othr>
                        <Id>{request.TransactionDto.CAccountNo}</Id>
                        <SchmeNm>
                          <Prtry>BKCOM_ACCOUNT</Prtry>
                        </SchmeNm>
                      </Othr>
                    </Id>
                    <Ccy>ETB</Ccy>
                  </CdtrAcct>
                  <RmtInf>
                    <Ustrd>{request.TransactionDto.PaymentNo}</Ustrd>
                  </RmtInf>
                </CdtTrfTxInf>
              </PmtInf>
            </CstmrCdtTrfInitn>
          </Document>
        ]]></amp:pain001>
      </amp:createTransferRequest>
    </amp:createTransferRequestFlow>
  </soapenv:Body>
</soapenv:Envelope>";

            var soapResponse = await _soapClient.SendSoapRequestAsync("https://10.1.7.85:8095/createTransfer", xmlRequest, soapAction);


            /*   if (!_soapClient.IsSuccessfulResponse(soapResponse))
               {
                   throw new Exception("SOAP request failed. Required elements did not match success criteria.");
               }*/

            var (isSuccess, reason) = _soapClient.IsSuccessfulResponse(soapResponse);

            if (!isSuccess)
            {
                throw new Exception($"SOAP request failed: {reason}");


            }

            string memberId = request.TransactionDto.MemberId;
            var equbMember = await _equbMemberRepository.GetByIdString(memberId);

            if (equbMember == null)
            {
                throw new Exception($"EqubMember with Id {request.TransactionDto.Id} not found.");
            }

            // Fetch corresponding EqubType data
            var equbType = await _equbTypeRepository.GetByName(equbMember.EqubType);
            if (equbType == null)
            {
                throw new Exception($"EqubType with Name {equbMember.EqubType} not found.");
            }

            DateTime currentDate = DateTime.Now;
            DateTime startDate = equbType.EqupStartDay;

            // Calculate the grace period
            int gracePeriodDays = GetGracePeriodDays(equbType.TimeUnits, equbType.TimeQuantity);

            // Calculate time elapsed and grace period elapsed
            TimeSpan timeElapsed = currentDate - startDate;
            TimeSpan gracePeriodElapsed = timeElapsed - TimeSpan.FromDays(gracePeriodDays);

            decimal totalPenaltyAmount = decimal.Parse(equbMember.PenalityAmount);
            DateTime endDate = equbType.EqupEndDay;
            int totalPeriods = GetTotalPeriods(equbType.TimeUnits, equbType.TimeQuantity, startDate, endDate);
            decimal totalAmountToPay = decimal.Parse(equbType.Amount) * totalPeriods;
            /*
                        if (gracePeriodElapsed.TotalDays > 0)
                        {
                            // Calculate penalty based on frequency
                            int penaltyPeriods = 0;

                            switch (equbType.PenalityFrequency)
                            {
                                case "Daily":
                                    int penaltyDays = (int)Math.Floor(gracePeriodElapsed.TotalDays);
                                    if (penaltyDays > 0)
                                    {
                                        penaltyPeriods = penaltyDays;
                                    }
                                    break;

                                case "Weekly":
                                    int weeksElapsed = (int)Math.Floor(gracePeriodElapsed.TotalDays / 7);
                                    penaltyPeriods = weeksElapsed;
                                    break;

                                case "Monthly":
                                    int monthsElapsed = GetElapsedMonths(startDate, currentDate) - (int)Math.Floor(gracePeriodElapsed.TotalDays / 30);
                                    penaltyPeriods = monthsElapsed;
                                    break;

                                default:
                                    throw new Exception("Invalid penalty frequency.");
                            }

                            // Calculate penalty amount based on type
                            switch (equbType.PenalityType)
                            {
                                case "Fixed ETB":
                                    totalPenaltyAmount = equbType.PenalityAmount * penaltyPeriods;
                                    break;

                                case "Percent":
                                    decimal dailyPenaltyPercentage = equbType.PenalityAmount / 100;
                                    totalPenaltyAmount = (decimal.Parse(equbType.Amount) * dailyPenaltyPercentage) * penaltyPeriods;
                                    break;

                                default:
                                    throw new Exception("Invalid penalty type.");
                            }
                        }

            */
         

            decimal totalAmountPaid = equbMember.TotalAmountPaid + request.TransactionDto.Amount;
            equbMember.TotalAmountPaid = totalAmountPaid;
            decimal totalAmountLeft = totalAmountToPay - totalAmountPaid;

            if (equbType.PenaltyStatus == "true")
            {
                equbMember.NoOfTimesPaid += 1;
                decimal penaltyToReduce = request.TransactionDto.Amount - decimal.Parse(equbType.Amount);

                if (penaltyToReduce > 0)
                {

                    decimal paidPenaltyAmount = string.IsNullOrEmpty(equbMember.PaidPenalityAmount)
                              ? 0 : decimal.Parse(equbMember.PaidPenalityAmount);

                    // Reduce from penalty first
                    decimal amountToDeductFromPenalty = Math.Min(totalPenaltyAmount, penaltyToReduce);
                    totalPenaltyAmount -= amountToDeductFromPenalty;
                    penaltyToReduce -= amountToDeductFromPenalty;


                    paidPenaltyAmount += amountToDeductFromPenalty;
                    equbMember.PaidPenalityAmount = paidPenaltyAmount.ToString();

                    totalAmountLeft= totalAmountToPay -(totalAmountPaid-paidPenaltyAmount);
                }
                else
                {
                    // Standard reduction if no extra payment over the equbType amount
                    totalAmountLeft = totalAmountToPay - totalAmountPaid;
                }
            }
            else
            {
                // If penaltyStatus is false, only reduce from totalAmountLeft
                totalAmountLeft = totalAmountToPay - totalAmountPaid;
            }
            equbMember.NoOfTimesPaid = (int)((totalAmountPaid - decimal.Parse(equbMember.PaidPenalityAmount)) / decimal.Parse(equbType.Amount));


            // Ensure totalAmountLeft and totalPenaltyAmount are not negative
            totalAmountLeft = Math.Max(0, totalAmountLeft);
            totalPenaltyAmount = Math.Max(0, totalPenaltyAmount);

            // Update equbMemberDetail with adjusted values
            equbMember.TotalAmountLeft = totalAmountLeft;
            equbMember.PenalityAmount = totalPenaltyAmount.ToString();


            // Determine completed status
            if (equbType.PenaltyStatus == "true")
            {
                if (equbMember.TotalAmountLeft == 0)
                {
                    equbMember.CompletedStatus = "Completed";
                }
                else
                {
                    equbMember.CompletedStatus = "Incomplete";
                }
            }
            else
            {
                if (equbMember.TotalAmountPaid >= totalAmountToPay)
                {
                    equbMember.CompletedStatus = "Completed";
                }
                else
                {
                    equbMember.CompletedStatus = "Incomplete";
                }
            }


            // Save changes to EqubMember
            await _equbMemberRepository.Update(equbMember);

     
            // If successful, update transaction
            transaction = _mapper.Map(request.TransactionDto, transaction);
            await _transactionSqlRepository.Update(transaction);




        }
        else if (request.TransactionDto.Status == "Canceled")
        {
           
            // Update transaction details
            transaction = _mapper.Map(request.TransactionDto, transaction);
            await _transactionSqlRepository.Update(transaction);
        }

        int GetGracePeriodDays(string timeUnits, string timeQuantity)
        {
            int timeQty = int.Parse(timeQuantity);
            int gracePeriodDays = 0;
            switch (timeUnits)
            {
                case "Day":
                    gracePeriodDays = timeQty;
                    break;
                case "Week":
                    gracePeriodDays = timeQty * 7;
                    break;
                case "Month":
                    gracePeriodDays = timeQty * 30; // Approximation, adjust as needed
                    break;
                default:
                    throw new Exception("Invalid time units.");
            }
            return gracePeriodDays;
        }

        int GetElapsedMonths(DateTime startDate, DateTime currentDate)
        {
            int totalMonthsDifference = ((currentDate.Year - startDate.Year) * 12) + (currentDate.Month - startDate.Month);
            if (currentDate.Day < startDate.Day)
            {
                totalMonthsDifference--;
            }
            return totalMonthsDifference;
        }


        return Unit.Value;



    }

}