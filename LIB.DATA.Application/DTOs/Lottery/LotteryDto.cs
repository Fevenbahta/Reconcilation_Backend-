using LIB.API.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.DTOs.Lottery
{
    public class LotteryDto :BaseDtos
    {
        public int Id { get; set; }
        public string EqubType { get; set; }
        public string EqubMember { get; set; }
        public DateTime LotteryDate { get; set; }
        public string Status { get; set; }
        public int LotteryRound { get; set; }
    }
}
