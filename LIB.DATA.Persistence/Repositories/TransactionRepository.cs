
using LIB.API.Application.Contracts.Persistent;
using LIB.API.Domain;
using LIB.API.Persistence;
using LIBPROPERTY.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LIB.API.Persistence.Repositories
{
    public class TransactionRepository : GenericRepositoryOracle<Transactions>, ITransactionRepository
    {
        private readonly LIBAPIDbContext _context;
        private readonly SoapClient2 _soapClient;
        private readonly HttpClient _httpClient;
        public TransactionRepository(LIBAPIDbContext context, SoapClient2 soapClient) : base(context)
        {
            _context = context;
            _soapClient = soapClient;
        }


        public async Task<AccountInfos> GetUserDetailsByAccountNumberAsync(string accountNumber)
        {


            var query2 = @"
SELECT *
FROM anbesaprod.valid_accounts
WHERE ACCOUNTNUMBER = :accountNumber";

            var accountNumberParameter = new OracleParameter("accountNumber", accountNumber);
            var userDetails2 = await _context.AccountInfos
                .FromSqlRaw(query2, accountNumberParameter)
                .FirstOrDefaultAsync();




            return (userDetails2);
        }



        public async Task<UserData> GetUserDetailAsync(string branchCode, string userName, string role)
        {


            var query2 = @"
    SELECT *
    FROM anbesaprod.users2
    WHERE BRANCH = :branchCode AND USER_NAME = :userName AND ROLE = :role";

            var branchCodeParameter = new OracleParameter("branchCode", branchCode);
            var userNameParameter = new OracleParameter("userName", userName);
            var roleParameter = new OracleParameter("role", role);

            var userDetails2 = await _context.userDatas
                .FromSqlRaw(query2, branchCodeParameter, userNameParameter, roleParameter)
                .FirstOrDefaultAsync();


            return (userDetails2);
        }
        public async Task<UserData> GetUserDetailAsyncByUserName(string userName)
        {
            var query = @"
SELECT *
FROM anbesaprod.users2
WHERE USER_NAME = :userName";

            var userNameParameter = new OracleParameter("userName", userName);

            var userDetails = await _context.userDatas
                .FromSqlRaw(query, userNameParameter)
                .FirstOrDefaultAsync();

            return userDetails;
        }

        public async Task<string> CheckAccountBalanceAsync(string branch, string account, decimal amount)
        {
            try
            {
                // Construct the SOAP request XML
                string soapRequest = $@"
                <soapenv:Envelope xmlns:amp=""http://soprabanking.com/amplitude"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
                    <soapenv:Header />
                    <soapenv:Body>
                        <amp:getAccountDetailRequestFlow>
                            <amp:requestHeader>
                                <amp:requestId>20240715165243092</amp:requestId>
                                <amp:serviceName>getAccountDetail</amp:serviceName>
                                <amp:timestamp>2024-07-15T16:52:43.092248+03:00</amp:timestamp>
                                <amp:originalName>CHQPNTAPP</amp:originalName>
                                <amp:userCode>CHQPNTUSER</amp:userCode>
                            </amp:requestHeader>
                            <amp:getAccountDetailRequest>
                                <amp:accountIdentifier>
                                    <amp:branch>{branch}</amp:branch>
                                         <amp:currency>001</amp:currency>
                                    <amp:account>{account}</amp:account>
                                </amp:accountIdentifier>
                            </amp:getAccountDetailRequest>
                        </amp:getAccountDetailRequestFlow>
                    </soapenv:Body>
                </soapenv:Envelope>";


                string soapAction = "getAccountDetail";

                // Use the utility method to send the SOAP request
                var soapResponse = await _soapClient.SendSoapRequestAsync("https://10.1.7.85:8095/getAccountDetail", soapRequest, soapAction);


                var (isSuccess, reason) = _soapClient.IsSuccessfulResponse(soapResponse);

                if (!isSuccess)
                {
                    throw new Exception($"SOAP request failed: {reason}");
                }

                var accountDetail = reason;

                // Compare amount with balance
                if (decimal.Parse(accountDetail) >= amount)
                {
                    return "true";
                }
                else
                {
                    return "insufficient";
                }
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while checking account balance.", ex);
            }
        }


        // Method to parse SOAP response and extract account details
        private AccountDetail ParseSoapResponse(string soapResponse)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(soapResponse);

            // Check if status code is not 0
            string statusCode = xmlDocument.SelectSingleNode("//fjs1:statusCode")?.InnerText;
            if (statusCode != "0")
            {
                throw new Exception($"Invalid response. Status code: {statusCode}");
            }

            // Extract account details from XML
            string branch = xmlDocument.SelectSingleNode("//fjs1:branch/fjs1:code")?.InnerText;
            string accountNumber = xmlDocument.SelectSingleNode("//fjs1:accountNumber")?.InnerText;
            decimal balance = decimal.Parse(xmlDocument.SelectSingleNode("//fjs1:balance")?.InnerText);

            return new AccountDetail
            {
                Branch = branch,
                AccountNumber = accountNumber,
                Balance = balance
            };
        }


    }
}
