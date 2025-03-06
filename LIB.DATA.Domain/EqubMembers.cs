using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIB.API.Domain.Common;

namespace LIB.API.Domain
{
    public class EqubMembers: BaseDomainEntity
    {
 
  
        public string  Id { get; set; }
        public string EqubType { get; set; }
        public string FullName { get; set; }
        public string PhoneNo { get; set; }
        public string CompletedStatus { get; set; }
        public decimal NoOfTimesPaid { get; set; }
        public decimal TotalAmountPaid { get; set; }
        public string PenalityAmount { get; set; }
        public string PaidPenalityAmount { get; set; }
        public string PenalityType { get; set; }
        public decimal TotalAmountLeft { get; set; }
     
        public string Status { get; set; }
    }
}
