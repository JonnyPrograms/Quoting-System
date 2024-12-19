using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesSystem.Domain.databases
{
    public class Credentials
    {
        public int associateId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public bool? passed { get; set; }
        public double? totalCommission { get; set; }
        public string? fullName { get; set; }
    }
}
