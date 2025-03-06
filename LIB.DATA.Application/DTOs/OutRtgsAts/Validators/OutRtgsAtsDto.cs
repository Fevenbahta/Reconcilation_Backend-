﻿using LIB.API.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.DTOs.OutRtgsAts.Validators
{
    public class OutRtgsAtsDto 
    {
        public int No { get; set; }
        public string Type { get; set; }
        public string Reference { get; set; }
        public string Debitor { get; set; }
        public string Creditor { get; set; }
        public string OrderingAccount { get; set; }
        public string BeneficiaryAccount { get; set; }
        public DateTime BusinessDate { get; set; }
        public DateTime EntryDate { get; set; }
        public string Currency { get; set; }

        public string Amount { get; set; }
        public string ProcessingStatus { get; set; }
        public string Status { get; set; }
    }
}
