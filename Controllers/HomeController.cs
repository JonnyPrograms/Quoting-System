using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using QuotesSystem.Domain.databases;
using QuotesSystem.Website.Models;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using Telerik.SvgIcons;

namespace QuotesSystem.Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        /// <summary>
        /// Shows the Home html 
        /// </summary>
        /// <returns> the Index.cshtml </returns>
        public IActionResult Index()
        {
            return View();
        }

        // delete Later 
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// If there is an error, it will show the error page 
        /// </summary>
        /// <returns> returns View with the Error Type </returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Gets the Names from the API Controller
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetNames()
        {
            // Construct the API URL with the query parameter
            var apiUrl = "http://localhost:5118/api/Test";

            // Send the GET request to the API
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var names = JsonSerializer.Deserialize<List<Customer>>(data, options);
                return Ok(names);
            }
            else
            {
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
        }

       

        /// <summary>
        /// Reads in from the Customers Section of the Domain
        /// </summary>
        /// <param name="request"></param>
        /// <returns> A list of Customers for the Kendo Grid</returns>
        public async Task<ActionResult> ReadNames([DataSourceRequest] DataSourceRequest request)
        {
            var result = await GetNames() as OkObjectResult;
            if (result != null)
            {
                var names = result.Value as List<Customer>;
                if (names != null)
                {
                    return Json(names.ToDataSourceResult(request)); 
                }
            }

            return StatusCode(500, "Failed to retrieve names");
        }
    }
}
