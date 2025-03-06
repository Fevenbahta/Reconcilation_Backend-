using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.DTOs.EqubMember
{
    public class EqubMemberSummaryDto
    {
        public string FullName { get; set; }
        public string EqubType { get; set; }
        public decimal TotalAmountPaid { get; set; }
        public decimal EqubAmountLeft { get; set; }
        public string PenalityAmount { get; set; }
        public string PaidPenalityAmount { get; set; }
        public decimal TotalAmountLeft { get; set; }
    }

}
