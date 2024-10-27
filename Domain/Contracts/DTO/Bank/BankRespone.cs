using Domain.Contracts.Abstracts.Bank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Domain.Contracts.DTO.Bank
{
    public class BankRespone
    {
        public string code { get; set; }
        public string desc { get; set; }
        public DataRequest data { get; set; }
    }
}
