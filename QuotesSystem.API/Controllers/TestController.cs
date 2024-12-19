using QuotesSystem.Infrastructure.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using QuotesSystem.Domain.databases;


namespace QuotesSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
        }

        [HttpGet(Name = "GetNames")]
        public IEnumerable<Customer> GetNames()
        {
            return _testService.GetNames();
        }

        [HttpGet("Work")]
        public IActionResult Work()
        {
            return Ok("It works");
        }
        
    }
}
