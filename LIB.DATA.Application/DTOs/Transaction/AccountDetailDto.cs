using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.DTOs.Transaction
{
    public class AccountDetailDto
    {
        public string Branch { get; set; }
        public string Currency { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        // Add other properties as needed
    }
}
