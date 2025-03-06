using LIB.API.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.DTOs.Transaction
{
    public class TransactionDto : BaseDtos
    {
        public int Id { get; set; }
        public string TransType { get; set; }
        public DateTime TransDate { get; set; }
        public string MemberId { get; set; }

        public string CAccountBranch { get; set; }
        public string CAccountNo { get; set; }
        public string CAccountOwner { get; set; }
        public string DAccountNo { get; set; }
        public string DDepositeName { get; set; }
        public string DepositorPhone { get; set; }
        public decimal Amount { get; set; }
        public string Branch { get; set; }
        public string CreatedBy { get; set; }
        public string ApprovedBy { get; set; }
        public string ReferenceNo { get; set; }
        public string MesssageNo { get; set; }
        public string PaymentNo { get; set; }
        public string Status { get; set; }
        public string EqupType { get; set; }
    }
}
