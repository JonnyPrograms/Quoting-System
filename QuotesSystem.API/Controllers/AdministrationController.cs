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
    public class AdministrationController : ControllerBase
    {
        private readonly IAdministrationService _testService;
       

        public AdministrationController(IAdministrationService testService)
        {
            _testService = testService;
        }

        [HttpPost("CreateAssociate")]
        public async Task<IActionResult> CreateAssociate([FromBody] Credentials newAssociate)
        {
            if (newAssociate == null)
            {
                return BadRequest("Invalid quote data sent.");
            }

            // Use your service to validate the credentials
            bool isValid = _testService.CreateAssociate(newAssociate);

            return Ok(isValid);
        }

        [HttpGet("ReadAssociates")]
        public async Task<IActionResult> ReadAssociates()
        {

            // Use your service to validate the credentials
            List<Credentials> allAssociates = _testService.ReadAssociates();

            return Ok(allAssociates);
        }


        [HttpPost("UpdateAssociate")]
        public async Task<IActionResult> UpdateAssociate(Credentials editAssociate)
        {

            // Use your service to validate the credentials
            bool isValid = _testService.UpdateAssociate(editAssociate);

            return Ok(isValid);
        }

        [HttpPost("DeleteAssociate")]
        public async Task<IActionResult> DeleteAssociate(Credentials deleteAssociate)
        {

            // Use your service to validate the credentials
            bool isValid = _testService.DeleteAssociate(deleteAssociate);

            return Ok(isValid);
        }

        /*
         * 
         *      Quotes CRUD
         * 
         */
        [HttpPost("ReadQuotes")]
        public async Task<IActionResult> ReadQuotes([FromBody] Filtered filterQuote)
        {

            // Use your service to validate the credentials
            List<Quote> quoteList = _testService.ReadQuotes(filterQuote);

            return Ok(quoteList);
        }

        [HttpGet("ReadCompletedQuotes")]
        public async Task<IActionResult> ReadCompletedQuotes()
        {

            // Use your service to validate the credentials
            List<Quote> quoteList = _testService.ReadCompletedQuotes();

            return Ok(quoteList);
        }

        [HttpPost("FinalizeQuote")]
        public async Task<IActionResult> FinalizeQuote(Quote completeQuote)
        {

            // Use your service to validate the credentials
            bool isValid = _testService.FinalizeQuote(completeQuote);

            return Ok(isValid);
        }

        [HttpPost("ProcessQuote")]
        public async Task<IActionResult> ProcessQuote(LineItem processQuote)
        {

            // Use your service to validate the credentials
            bool isValid = _testService.ProcessQuote(processQuote);

            return Ok(isValid);
        }

        [HttpPost("AssociateCommissionCalc")]
        public async Task<IActionResult> AssociateCommissionCalc(AssociateCommission associate)
        {

            // Use your service to validate the credentials
            bool isValid = _testService.AssociateCommissionCalc(associate);

            return Ok(isValid);
        }
    }
}
