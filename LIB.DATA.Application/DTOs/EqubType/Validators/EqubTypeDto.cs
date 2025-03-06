using LIB.API.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.DTOs.EqubType.Validators
{
    public class EqubTypeDto : BaseDtos
    {
        public string Id { get; set; }
        public string EqubName { get; set; }
        public string Amount { get; set; }
        public string TimeUnits { get; set; }
        public string TimeQuantity { get; set; }
        public decimal PenalityAmount { get; set; }
        public string PenalityType { get; set; }
        public string PenalityFrequency { get; set; }
        public DateTime EqupStartDay { get; set; }
        public DateTime EqupEndDay { get; set; }
        public string EqubAccountNumber { get; set; }
        public string AmountRequired { get; set; } // Total amount needed to be fulfilled
        public string Branch { get; set; }
        public string PenaltyStatus { get; set; }
        public string Status { get; set; }
    }
}
