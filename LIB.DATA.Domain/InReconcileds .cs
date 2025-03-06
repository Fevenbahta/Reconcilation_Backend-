using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Domain
{
    public class InReconcileds
    {
        [Key]
        public int No { get; set; }
        public string BRANCH { get; set; }
        public string ACCOUNT { get; set; }

        public string DISCRIPTION { get; set; }
        public decimal AMOUNT { get; set; }
        public string INPUTING_BRANCH { get; set; }
        public DateTime TRANSACTION_DATE { get; set; }

        public string Type { get; set; }
        public string Reference { get; set; }
        public string Debitor { get; set; }
        public string Creditor { get; set; }
        public string OrderingAccount { get; set; }
        public string BeneficiaryAccount { get; set; }
        public DateTime BusinessDate { get; set; }
        public DateTime EntryDate { get; set; }
        public string Currency { get; set; }
        public string ProcessingStatus { get; set; }
        public string Status { get; set; }

    }
}
