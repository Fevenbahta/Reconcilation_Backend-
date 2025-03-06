using LIB.API.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Domain
{
    public class Lotteries : BaseDomainEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string EqubType { get; set; }
        public string EqubMember { get; set; }
        public DateTime LotteryDate { get; set; }
        public string Status { get; set; }
        public int LotteryRound { get; set; }
    }
}
