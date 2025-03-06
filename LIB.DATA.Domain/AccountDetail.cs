using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Domain
{
    public class AccountDetail
    {
        public string Branch { get; set; }
        public string Currency { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }

    }
}
