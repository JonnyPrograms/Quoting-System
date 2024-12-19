using QuotesSystem.Infrastructure.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using QuotesSystem.Domain.databases;
using Microsoft.Extensions.Options;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;


namespace QuotesSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssociateController : ControllerBase
    {
        private readonly IAssociateService _testService;

        public AssociateController(IAssociateService testService)
        {
            _testService = testService;
        }

        [HttpPost("validateUser")]
        public async Task<IActionResult> validateUser([FromBody] Credentials credentials)
        {
            if (credentials == null)
            {
                return BadRequest("Invalid credentials data.");
            }

            // Use your service to validate the credentials
            Credentials isValid = _testService.validateUser(credentials);

            return Ok(isValid);
        }

        [HttpPost("CreateQuote")]
        public async Task<IActionResult> CreateQuote([FromBody] Quote quote)
        {
            if (quote == null)
            {
                return BadRequest("Invalid quote data sent.");
            }

            // Use your service to validate the credentials
            Quote isValid = _testService.CreateQuote(quote);

            return Ok(isValid);
        }

        [HttpPost("ReadQuotes")]
        public async Task<IActionResult> ReadQuote([FromBody] Credentials credentials)
        {
            if (credentials == null)
            {
                return BadRequest("Invalid quote data sent.");
            }

            // Use your service to validate the credentials
            List<Quote> quotes = _testService.ReadQuotes(credentials);

            return Ok(quotes);
        }

        [HttpPost("CreateLineItems")]
        public async Task<IActionResult> CreateLineItems([FromBody] List<LineItem> lineItems)
        {
            if (lineItems == null)
            {
                return BadRequest("Invalid quote data sent.");
            }

            // Use your service to validate the credentials
            bool isValid = _testService.CreateLineItems(lineItems);

            return Ok(isValid);
        }
        [HttpPost("DeleteCurrentLineItem")]
        public async Task<IActionResult> DeleteCurrentLineItem([FromBody] LineItem deleteItem)
        {
            if (deleteItem == null)
            {
                return BadRequest("Invalid quote data sent.");
            }

            // Use your service to validate the credentials
            bool lineItems = _testService.DeleteCurrentLineItem(deleteItem);

            return Ok(lineItems);
        }


        
        [HttpPost("ReadCurrentLineItems")]
        public async Task<IActionResult> ReadCurrentLineItems([FromBody] LineItem lineItem)
        {
            if (lineItem == null)
            {
                return BadRequest("Invalid quote data sent.");
            }

            // Use your service to validate the credentials
            List<LineItem> lineItems = _testService.ReadCurrentLineItems(lineItem);

            return Ok(lineItems);
        }
        [HttpPost("AddCurrentLineItem")]
        public async Task<IActionResult> AddCurrentLineItem([FromBody] LineItem newItem)
        {
            if (newItem == null)
            {
                return BadRequest("Invalid quote data sent.");
            }

            // Use your service to validate the credentials
            LineItem lineItems = _testService.AddCurrentLineItem(newItem);

            return Ok(lineItems);
        }
        
        [HttpPost("UpdateQuote")]
        public async Task<IActionResult> UpdateQuote([FromBody] Quote updateQuote)
        {
            if (updateQuote == null)
            {
                return BadRequest("Invalid quote data sent.");
            }

            // Use your service to validate the credentials
            bool isValid = _testService.UpdateQuote(updateQuote);

            return Ok(isValid);
        }
        [HttpPost("DeleteQuote")]
        public async Task<IActionResult> DeleteQuote([FromBody] LineItem deletedItem)
        {
            if (deletedItem == null)
            {
                return BadRequest("Invalid quote data sent.");
            }

            // Use your service to validate the credentials
            bool isValid = _testService.DeleteQuote(deletedItem);

            return Ok(isValid);
        }
    }
}
