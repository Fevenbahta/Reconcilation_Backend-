using LIB.API.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.DTOs.AccountInfo
{
    public class AccountInfoDto
    {
   
        public string CUSTOMER_ID { get; set; }
   
        public string FULL_NAME { get; set; }
   
        public string ACCOUNTNUMBER { get; set; }
 
        public string Branch { get; set; }
  
        public string TELEPHONENUMBER { get; set; }
    }
}
