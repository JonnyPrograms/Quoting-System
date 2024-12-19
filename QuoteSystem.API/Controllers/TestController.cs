using QuotesSystem.Infrastructure.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Demo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly ITestService _testService;

        public DemoController(ITestService demoService)
        {
            _testService = demoService;
        }

        [HttpGet(Name = "GetName")]
        public IEnumerable<string> GetName()
        {
            List<string> names = _testService.GetName();

            return names;
        }

    }
}
