using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesSystem.Domain.databases
{
    public class ProcessQuoteClass
    {
        public string order { get; set; }
        public string associate { get; set; }
        public int custid { get; set; }
        public double amount { get; set; }
        public string name { get; set; }
        public string processDay { get; set; }
        public string commission { get; set; }
        
    }
}
