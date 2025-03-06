using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.DTOs.AccountInfo
{
    public class UserDataDto
    {
        public int BRANCH { get; set; }
        public string BRANCH_NAME { get; set; }
        public string USER_NAME { get; set; }
        public string FULL_NAME { get; set; }
        public int ROLE { get; set; }
        public string? CREATED_DATE { get; set; }
        public string? UPDATED_DATE { get; set; }
        public string STATUS { get; set; }
    }
}
