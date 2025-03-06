using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Domain
{
    public class CreateTransactionRequest
    {
        public string DAccountNo { get; set; }
        public string MemberId { get; set; }
        public decimal Amount { get; set; }
    }

}
