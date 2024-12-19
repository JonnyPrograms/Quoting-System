using QuotesSystem.Domain.databases;
using QuotesSystem.Infrastructure.Abstract;
using QuotesSystem.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesSystem.Infrastructure.Concrete
{
    public class TestService : ITestService
    {
        readonly ITestRepository _testRepository;

        public TestService(ITestRepository testRepository)
        {
            _testRepository = testRepository;
        }
        public List<Customer> GetNames()
        {
            Task<List<Customer>> task = _testRepository.GetNamesAsync();
            return task.Result;  
            
        }
    }
}
