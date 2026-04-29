using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace SecondService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecondController : ControllerBase
    {
        private readonly HttpClient _http;
        private readonly IMemoryCache _cache;
        private readonly List<Product> products = new List<Product>()
        {
            new Product{Name="TV",price=13000},
            new Product{Name="mobile",price=80000}
        };
        public SecondController(HttpClient http,IMemoryCache cache)
        {
            _http = http;
            _cache = cache;
        }
        [HttpGet]
        [Route("Productlist")]
        public ActionResult Productlist()
        {
            return Ok(products);
        }
        [HttpGet]
        [Route("GreetMe")]
        public async Task<ActionResult> GreetMe()
        {
            await Task.Delay(10000);
            return Ok("Good Evening Students! This is from second microservice");
        }

        [HttpGet]
        [Route("Greetings")]
        public async Task<IActionResult> Greetings()
        {
            var httpclient=new HttpClient();
            string response = await _http.GetStringAsync("http://localhost:5000/First/Courselist");

            return Ok(response);    
        }

        internal class Product
        {
            public string Name { get; set; }
            public int price { get; set; }
        }
    }
}
