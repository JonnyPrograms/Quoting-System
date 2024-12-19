using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesSystem.Domain.databases
{
    public class LineItem
    {   
        public string itemId { get; set; }
        public string itemName { get; set; }
        public decimal itemPrice { get; set; }
        public string? quoteID { get; set; }

    }
}
