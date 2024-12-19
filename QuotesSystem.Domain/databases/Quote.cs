using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesSystem.Domain.databases
{
    public class Quote
    {
        public string quoteID { get; set; }
        public int associateID { get; set; }
        public int customerID { get; set; }
        public string? customer { get; set; }
        public string? associate { get; set; }
        public double quotePrice { get; set; }
        public string quoteEmail { get; set; }
        public string? secretNotes { get; set; }
        public double discount { get; set; }
        public int percentage { get; set; }
        public string? workFlow { get; set; }
        public DateTime dateCreated { get; set; }
        public double? associateCommission { get; set; }
        public DateTime? dateFinalized { get; set; }

    }
}
