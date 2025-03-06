using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Domain
{
    public class TransactionResult
    {
        public bool Success { get; set; }
        public int TransactionId { get; set; }
        public string Message { get; set; }
    }

}
