using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.DTOs.Transaction
{
    public class TransactionSummaryDto
    {
        public int Id { get; set; }
        public string EqubType { get; set; }
        public DateTime TransDate { get; set; }
        public string TransType { get; set; }
    
        public string Branch { get; set; }
        public string ApprovedBy { get; set; }
        public string DDepositeName { get; set; }
        public string DAccountNo { get; set; }
        public string CAccountBranch { get; set; }
        public string DepositorPhone { get; set; }
        public string MemberId { get; set; }
        public string CAccountOwner { get; set; }
        public string CAccountNo { get; set; }
        public decimal Amount { get; set; }
    }
}
