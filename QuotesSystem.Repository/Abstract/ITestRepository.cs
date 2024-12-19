using QuotesSystem.Domain.databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesSystem.Repository.Abstract
{
    public interface ITestRepository
    {
        // gets the name of people 
        Task<List<Customer>> GetNamesAsync();
    }
}
