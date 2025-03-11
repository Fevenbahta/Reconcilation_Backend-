using LIB.API.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.DTOs.OutRtgsCbc
{
    public class OutRtgsCbcDto 
    {

        public string REFNO { get; set; }
        public string BRANCH { get; set; }
        public string ACCOUNT { get; set; }
        public string DEBITOR_NAME { get; set; }
        public string DISCRIPTION { get; set; }
        public decimal AMOUNT { get; set; }
        public string INPUTING_BRANCH { get; set; }
        public string DATET { get; set; }
    }
}
