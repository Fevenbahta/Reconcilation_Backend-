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
    public class EqubTypes : BaseDomainEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
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
