using QuotesSystem.Domain.databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesSystem.Infrastructure.Abstract
{
    public interface ITestService
    {
        // gets the name of people 
        List<Customer> GetNames();
    }
}
