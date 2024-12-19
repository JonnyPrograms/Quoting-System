using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesSystem.Domain.databases
{
    public class AssociateCommission
    {
        public int associateId { get; set; }
        public double priceAmount { get; set; }
        public double commission { get; set; }
    }
}
